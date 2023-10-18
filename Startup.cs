using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using smartlocker.software.api.Services.Implements;
using smartlocker.software.api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using SmartLocker.Software.API.Services.Implements;
using SmartLocker.Software.API.Services.Interfaces;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Repositories.Implements;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.Implements;
using SmartLocker.Software.Backend.Services.Interfaces;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.DataAccess;
using Microsoft.Extensions.FileProviders;
using System.IO;
using smartlocker.software.api.Services.DataAccess.Interface;
using smartlocker.software.api.Services.DataAccess;
using smartlocker.software.api.Models.Email;
using Microsoft.AspNetCore.HttpOverrides;

namespace SmartLocker.Software.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDirectoryBrowser();
            services.AddAutoMapper(cd => cd.CreateMap<FormRegisterDto, Accounts>());
            services.AddAutoMapper(cd => cd.CreateMap<Accounts, AccountDto>());
            services.AddAutoMapper(cd => cd.CreateMap<ServiceRate, ServiceRate>());
            services.AddAutoMapper(cd => cd.CreateMap<AccountPaymentInfo, PaymentInfoDto>().ForMember(c => c.InfoId, opt => opt.Ignore()));
            services.AddAutoMapper(cd => cd.CreateMap<PaymentInfoDto, AccountPaymentInfo>());
            services.AddAutoMapper(cd => cd.CreateMap<BookingDto, Booking>());
            services.AddAutoMapper(cd => cd.CreateMap<Booking, BookingDto>());
            services.AddAutoMapper(cd => cd.CreateMap<CardType, CardTypeDto>());
            services.AddAutoMapper(cd => cd.CreateMap<LockerRooms, LockerRoomDto>());
            services.AddAutoMapper(cd => cd.CreateMap<RateTypes, RateTypeDto>());
            services.AddAutoMapper(cd => cd.CreateMap<EventTypes, EventTypeDto>());
            services.AddCors();
            services.AddControllers(options =>
            {

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetConnectionString("SecretKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddScoped<IBaseRepository, BaseRepository>(c => new BaseRepository(Configuration.GetConnectionString("MSSQL")));
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IDashBoardService, DashBoardService>();
            services.AddScoped<IDashboardDA, DashboardDA>();
            services.AddScoped<IAccountDA, AccountDA>();
            services.AddScoped<IRoleDA, RoleDA>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILockerDA, LockerDA>();
            services.AddScoped<ISqlDA, SqlDA>();
            services.AddScoped<ILockerService, LockerService>();
            services.AddScoped<IPaymentInfoDA, PaymentInfoDA>();
            services.AddScoped<IPaymentInfoService, PaymentInfoService>();
            services.AddScoped<IBookingDA, BookingDA>();
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IFormRequestLockerService, FormRequestLockerService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationDA, NotificationDA>();
            services.AddScoped<ILocationDA, LocationDA>();
            services.AddScoped<IFormRequestLockerDA, FormRequestLockerDA>();
            services.AddScoped<IMailer, Mailer>();
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IIncomeDA, IncomeDA>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IRepairFormService, RepairFormService>();
            services.AddScoped<IRepairFormDA, RepairFormDA>();
            services.AddScoped<IUserDashboardDA, UserDashboardDA>();
            services.AddScoped<IUserDashboardService, UserDashboardService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            app.UseForwardedHeaders(new ForwardedHeadersOptions{
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/img"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports")),
                RequestPath = "/report"
            });

            app.UseStaticFiles();

            app.UseRouting();


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
