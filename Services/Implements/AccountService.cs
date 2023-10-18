using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using smartlocker.software.api.Models.Email;
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Models.Output.ErrorResponse;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _Config;
        private readonly IBaseRepository baseRepository;
        private readonly IMapper mapper;
        private readonly IAccountDA accountDA;
        private readonly IRoleDA roleDA;
        private readonly IMailer mailer;

        public AccountService(IBaseRepository baseRepository, IMailer mailer, IConfiguration config, IMapper mapper, IAccountDA accountDA, IRoleDA roleDA)
        {
            _Config = config;
            this.baseRepository = baseRepository;
            this.mapper = mapper;
            this.accountDA = accountDA;
            this.roleDA = roleDA;
            this.mailer = mailer;

        }

        private string GenerateAccountCode(string role, int count)
        {
            StringBuilder accountCode = new StringBuilder();
            accountCode.Append(++count);
            for (; accountCode.ToString().Length <= 6;)
            {
                accountCode.Insert(0, "0");
            }
            if (role == RoleUser.Partner)
            {
                accountCode.Insert(0, "P");
            }
            else if (role == RoleUser.User)
            {
                accountCode.Insert(0, "U");
            }
            else if (role == RoleUser.Admin)
            {
                accountCode.Insert(0, "A");
            }
            else
            {
                accountCode.Insert(0, "NONE");
            }
            return accountCode.ToString();
        }
        private string ComputeSha256Hash(string password)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
        private TokenResponse GenerateJSONWebToken(Accounts account, string roleUser)
        {

            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_Config.GetConnectionString("SecretKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, account.AccountId.ToString() ),
                    new Claim(ClaimTypes.Role,roleUser)
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            TokenResponse tokenDetail = new TokenResponse
            {
                Token = tokenHandler.WriteToken(token),
                ExpiredDate = tokenDescriptor.Expires.GetValueOrDefault(),
                AccountCode = account.AccountCode,
                FirstName = account.FirstName,
                LastName = account.LastName,
                AccountId = account.AccountId,
            };
            return tokenDetail;
        }
        public ResponseHeader LoginAccount(FormLoginDto formLoginDto)
        {
            if (String.IsNullOrEmpty(formLoginDto.Email) || String.IsNullOrEmpty(formLoginDto.Password))
            {
                throw new ArgumentException("Username or Password invalid value.");
            }
            var roleUser = roleDA.GetRoleByRoleName(formLoginDto.RoleUser);

            if (roleUser == null)
            {
                throw new Exception("Role is invalid");
            }
            
            string username = formLoginDto.Email;
            string password = formLoginDto.Password;

            var accountResult = accountDA.GetAccountByUserNameAndPasswordAndRoleId(username, password, roleUser.RoleId);
            Console.WriteLine(roleUser.RoleId);
            if (accountResult == null)
            {
                throw new UnauthorizedAccessException("Your username or password incorrect try again.");
            }
            else
            {
                return new ResponseHeader()
                {
                    Status = "S",
                    Message = $@"Login sucessful.",
                    Content = GenerateJSONWebToken(accountResult, roleUser.RoleName)
                };
            }

        }

        public async Task<ResponseHeader> RegisterAccountAsync(FormRegisterDto formRegister)
        {
            using var context = new SmartLockerContext();
            Accounts account = mapper.Map<FormRegisterDto, Accounts>(formRegister);
            account.Password = EncryptionUtilities.CreatePasswordSalt(account.Password);
            account.CreateDate = DateTime.Now;

            int accountAmount = context.Accounts.ToList().Count;
            account.AccountCode = GenerateAccountCode(formRegister.RoleUser, accountAmount);

            account.RoleId = context.Roles
                .Where(w => w.RoleName == formRegister.RoleUser)
                .FirstOrDefault()
                .RoleId;
            if (formRegister.RoleUser != RoleUser.Partner)
            {
                account.ApproveDate = DateTime.Now;
                account.Status = "A";
            }
            else
            {
                account.Status = "WV";
            }
            context.Accounts.Add(account);
            int validateInsert = context.SaveChanges();
            var data = context.Accounts.Where(s => s.Email == formRegister.Email && s.Status == "WV").FirstOrDefault();
            int validateDetail = 0;

            if (data != null)
            {
                var fullName = $@"{formRegister.FirstName} {formRegister.LastName}";

                var htmlBody = $@"<table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                                        <tr>
                                                            <td style='padding:10px 0 30px 0' >

                                                                <table
                                                                    align= 'center'
                                                                    border= '1'
                                                                    cellpadding= '0'
                                                                    cellspacing= '0'
                                                                    width= '600'
                                                                    style='border: 10px solid #cccccc; border-collapse: collapse'
                                                                >
                                                                    <tr>
                                                                    <td align='left' style='padding: 20px;color: #153643;font-size: 16px;line-height: 25px;font-family: Arial;'>
                                                                        <div style = 'white-space: pre-wrap; padding-left: 20px'>
                เรียน {fullName}
                กรุณายืนยันตัวตนกับทางระบบเพื่อไปสู้ขั้นตอนต่อไป
                <a href='https://webservice-locker.ml/api/account/VerifyAccount?email={formRegister.Email}&id={data.AccountId}'>Verify</a>
                วันที่ดำเนินการ {DateTime.Now.ToString("d MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH"))}
                --------------------------------------------------------------------------------------
                ขอแสดงความนับถือ อีเมลนี้เป็นอีเมลอัตโนมัติ กรุณาอย่าตอบกลับ
                                                                        </div>
                                                                    </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>";
                var builder = new BodyBuilder();
                builder.HtmlBody = htmlBody;
                string messageBody = string.Format(builder.HtmlBody);
                await mailer.SendEmailAsync(formRegister.Email, "ยืนยันตัวตนของทางระบบ", messageBody);
                string queryString = $@"INSERT INTO PARTNER_DETAIL 
                                           ([TelNo],[PartnerType],[PartnerNumber],[PartnerAddress],[AccountId],[CreateDate])
                                           VALUES
                                           ('{formRegister.TelNo}'
                                            ,'{formRegister.PartnerType}'
                                            ,'{formRegister.PartnerNumber}'
                                            ,'{formRegister.PartnerAddress}'
                                            ,{data.AccountId}
                                            ,GETDATE())";

                validateDetail = baseRepository.ExecuteString<int>(queryString);
            }
            if (validateInsert != 0 && validateDetail != 0)
            {
                return new ResponseHeader()
                {
                    Status = "S",
                    Message = "Register sucessful",
                    Content = validateInsert
                };
            }
            else
            {
                throw new Exception("Register failed.");
            }


        }

        public ResponseHeader VerifyAccount(string email, int id)
        {
            using var context = new SmartLockerContext();
            var data = context.Accounts.Where(s => s.Email == email && s.AccountId == id && s.Status == "WV").FirstOrDefault();

            if (data == null)
            {
                return new ResponseHeader()
                {
                    Status = "F",
                    Message = "verified account",
                    Content = null
                };
            }
            else
            {
                data.Status = "W";
                context.SaveChanges();
                return new ResponseHeader()
                {
                    Status = "S",
                    Message = "Verify sucessful",
                    Content = null
                };
            }


        }

        public ResponseHeader ValidateDuplicateEmail(string email)
        {
            var validateResult = accountDA.GetAccountByEmail(email);

            AccountDto result = mapper.Map<Accounts, AccountDto>(validateResult);

            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Email successful",
                Content = result,
                Page = 1,
                PerPage = 1,
                TotalElement = 1,
            };

        }
        public ResponseHeader GetAccountByAccountCode(string accountCode)
        {
            using var context = new SmartLockerContext();
            var resultAccount = context.Accounts.Where(c => c.AccountCode == accountCode && c.Status == "A").FirstOrDefault();
            AccountDto result = mapper.Map<Accounts, AccountDto>(resultAccount);
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Account By AccountCode successful",
                Content = result,
                Page = 1,
                PerPage = 1,
                TotalElement = 1,
            };
        }
        public ResponseHeader UpdateAccount(AccountDto accountDto)
        {
            using var context = new SmartLockerContext();
            Accounts accounts = context.Accounts.Where(a => a.AccountId == accountDto.AccountId && a.Status == "A").FirstOrDefault();
            if (accounts == null)
            {
                throw new ConfilctDataException("AccountId is invalid");
            }
            accounts.FirstName = accountDto.FirstName;
            accounts.LastName = accountDto.LastName;
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return new ResponseHeader("S", "update account successful");
        }
        public ResponseHeader GetPartnerAccount(int page, int perPage, AccountDto account)
        {
            var accountResult = accountDA.GetPartnerAccount(page, perPage, account);
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Partner account successful",
                Content = accountResult.Content,
                Page = accountResult.Page,
                PerPage = accountResult.PerPage,
                TotalElement = accountResult.TotalElement,
            };
        }
        public async Task<ResponseHeader> ApproveAccount(AccountDto accountDto)
        {
            using var context = new SmartLockerContext();
            Accounts accounts = context.Accounts.Where(a => a.AccountId == accountDto.AccountId).FirstOrDefault();
            if (accounts == null)
            {
                throw new ConfilctDataException("AccountId is invalid");
            }
            try
            {
                if (accounts.Status == "W" && accountDto.Status == "approve")
                {
                    var fullName = $@"{accounts.FirstName} {accounts.LastName}";

                    var htmlBody = $@"<table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                        <tr>
                                            <td style='padding:10px 0 30px 0' >
                                        
                                                <table
                                                    align= 'center'
                                                    border= '1'
                                                    cellpadding= '0'
                                                    cellspacing= '0'
                                                    width= '600'
                                                    style='border: 10px solid #cccccc; border-collapse: collapse'
                                                >
                                                    <tr>
                                                    <td align='left' style='padding: 20px;color: #153643;font-size: 16px;line-height: 25px;font-family: Arial;'>
                                                        <div style = 'white-space: pre-wrap; padding-left: 20px'>
เรียน {fullName}
ทางแอดมินได้ทำการตรวจสอบข้อมูลลงทะเบียนของคุณเรียบร้อยแล้ว
ข้อมูลการทะเบียนของคุณถูกต้องและครบถ้วน
ท่านสามารถเข้าใช้งานได้แล้ว http://webservice-locker.ml:8080/ วันที่ดำเนินการ
{DateTime.Now.ToString("d MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH"))}
--------------------------------------------------------------------------------------
ขอแสดงความนับถือ อีเมลนี้เป็นอีเมลอัตโนมัติ กรุณาอย่าตอบกลับ
                                                        </div>
                                                    </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>";
                    var builder = new BodyBuilder();
                    builder.HtmlBody = htmlBody;
                    string messageBody = string.Format(builder.HtmlBody);
                    await mailer.SendEmailAsync(accountDto.Email, "อนุมัติเข้าใช้งานระบบ", messageBody);
                    accounts.ApproveDate = DateTime.Now;
                    accounts.Status = "A";
                    context.SaveChanges();
                }
                else if (accounts.Status == "W" && accountDto.Status == "reject")
                {

                    var fullName = $@"{accounts.FirstName} {accounts.LastName}";

                    var htmlBody = $@"<table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                        <tr>
                                            <td style='padding:10px 0 30px 0' >
                                        
                                                <table
                                                    align= 'center'
                                                    border= '1'
                                                    cellpadding= '0'
                                                    cellspacing= '0'
                                                    width= '600'
                                                    style='border: 10px solid #cccccc; border-collapse: collapse'
                                                >
                                                    <tr>
                                                    <td align='left' style='padding: 20px;color: #153643;font-size: 16px;line-height: 25px;font-family: Arial;'>
                                                        <div style = 'white-space: pre-wrap; padding-left: 20px'>
เรียน {fullName}
ทางแอดมินได้ทำการตรวจสอบข้อมูลลงทะเบียนของคุณเรียบร้อยแล้ว
ข้อมูลการทะเบียนของคุณไม่ถูกต้องเนื่องจาก {accountDto.Reason}
วันที่ดำเนินการ {DateTime.Now.ToString("d MMMM yyyy", CultureInfo.CreateSpecificCulture("th-TH"))}
--------------------------------------------------------------------------------------
ขอแสดงความนับถือ อีเมลนี้เป็นอีเมลอัติโนมัติ กรุณาอย่าตอบกลับ
                                                        </div>
                                                    </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                ";
                    var builder = new BodyBuilder();
                    builder.HtmlBody = htmlBody;
                    string messageBody = string.Format(builder.HtmlBody);
                    await mailer.SendEmailAsync(accounts.Email, "อนุมัติเข้าใช้งานระบบ", messageBody);
                    accounts.ApproveDate = DateTime.Now;
                    accounts.Status = "R";
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return new ResponseHeader("S", "Approve account successful");
        }

        public ResponseHeader ValidateDuplicateEmailPartner(string email)
        {
            using var context = new SmartLockerContext();
            var data = context.Accounts.Where(s => s.Email == email && s.Status != "R" && s.RoleId == 6).FirstOrDefault();

            if (data == null)
            {
                return new ResponseHeader()
                {
                    Status = "F",
                    Message = "Get Email success",
                    Content = null
                };
            }
            else
            {
                data.Status = "W";
                context.SaveChanges();
                return new ResponseHeader()
                {
                    Status = "S",
                    Message = "Get Email unSucessful",
                    Content = data
                };
            }
        }
    }
}
