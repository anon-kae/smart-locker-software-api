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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Implements
{

    public class FormRequestLockerService : IFormRequestLockerService
    {
        private readonly IFormRequestLockerDA formRequestLockerDA;

        public FormRequestLockerService(IFormRequestLockerDA formRequestLockerDA)
        {
            this.formRequestLockerDA = formRequestLockerDA;
        }
        public ResponseHeader AddFormRequestLocker(FormRequestDto formRequestDto)
        {
            
            Lockers lockerNew = new Lockers();
            LockerAmount lockerAmountS = new LockerAmount();
            LockerAmount lockerAmountM = new LockerAmount();
            LockerAmount lockerAmountL = new LockerAmount();
            FormRequestLockers formRequestLockers = new FormRequestLockers();
            try
            {
                using var context = new SmartLockerContext();
                //add function
                if (formRequestDto.FormId <= 0)
                {
                    //add locker first
                    lockerNew = new Lockers
                    {
                        LockerCode = GenerateFormCode(context.Lockers.ToList().Count, "LK"),
                        LocateId = formRequestDto.LocateId,
                        Status = "W",
                        CreateDate = DateTime.Now
                    };
                    context.Lockers.Add(lockerNew);
                    context.SaveChanges();

                    //then add form request
                    formRequestLockers = new FormRequestLockers
                    {
                        AccountId = formRequestDto.AccountId,
                        LockerId = lockerNew.LockerId,
                        Status = "VF",
                        CreateDate = DateTime.Now,
                        OptionalRequest = formRequestDto.OptionalRequest,
                        FormCode = GenerateFormCode(context.FormRequestLockers.ToList().Count, "F")
                    };
                    context.FormRequestLockers.Add(formRequestLockers);
                    context.SaveChanges();

                    //Prepare data to save locker_amount
                    List<LockerSizes> lockerSizeList = context.LockerSizes.ToList();

                    LockerAmount lockerAmount = new LockerAmount
                    {
                        FormId = formRequestLockers.FormId,
                        Status = "A",
                        CreateDate = DateTime.Now,
                        //size s save
                        LkSizeId = lockerSizeList.Find(c => c.LkSizeName.Equals("S")).LkSizeId,
                        Amount = formRequestDto.SAmount
                    };
                    context.LockerAmount.Add(lockerAmount);
                    context.SaveChanges();
                    lockerAmountS = lockerAmount;

                    lockerAmount = new LockerAmount
                    {
                        FormId = formRequestLockers.FormId,
                        Status = "A",
                        CreateDate = DateTime.Now,
                        //size m save
                        LkSizeId = lockerSizeList.Find(c => c.LkSizeName.Equals("M")).LkSizeId,
                        Amount = formRequestDto.MAmount
                    };
                    context.LockerAmount.Add(lockerAmount);
                    context.SaveChanges();
                    lockerAmountM = lockerAmount;

                    lockerAmount = new LockerAmount
                    {
                        FormId = formRequestLockers.FormId,
                        Status = "A",
                        CreateDate = DateTime.Now,
                        //size l save
                        LkSizeId = lockerSizeList.Find(c => c.LkSizeName.Equals("L")).LkSizeId,
                        Amount = formRequestDto.LAmount
                    };
                    context.LockerAmount.Add(lockerAmount);
                    context.SaveChanges();
                    lockerAmountL = lockerAmount;

                    ResponseHeader response = new ResponseHeader
                    {
                        Status = "S",
                        Message = "Add form locker request sucessful"
                    };
                    return response;
                }
                //edit function
                else
                {
                    //get form from database
                    FormRequestLockers form = context.FormRequestLockers.Where(c => c.FormId == formRequestDto.FormId).FirstOrDefault();

                    //check form is valid?
                    if (form == null)
                    {
                        throw new ArgumentException("FormId is not valid");
                    }
                    else if (!form.Status.Equals(FormStatus.RejectForm))
                    {
                        throw new ArgumentException("Form status not right for this function");
                    }
                    else 
                    {
                        formRequestLockers = form;
                        //edit optionalRequest in form 
                        form.Status = FormStatus.ValidateForm;
                        form.OptionalRequest = formRequestDto.OptionalRequest;
                        form.UpdateDate = DateTime.Now;
                        context.SaveChanges();

                        List<LockerSizes> lockerSizeList = context.LockerSizes.ToList();

                        //edit locateId in locker
                        Lockers editedLocker = context.Lockers.Where(c => c.LockerId == form.LockerId).FirstOrDefault();
                        lockerNew = editedLocker;
                        editedLocker.LocateId = formRequestDto.LocateId;
                        editedLocker.UpdateDate = DateTime.Now;
                        context.SaveChanges();

                        
                        List<LockerAmount> lockerAmount = context.LockerAmount.Where(c => c.FormId == form.FormId).ToList();

                        //update each size
                        foreach(LockerAmount amount in lockerAmount)
                        {
                            if(amount.LkSizeId == lockerSizeList.Find(c => c.LkSizeName.Equals("S")).LkSizeId){
                                //for rollback lockerAmount size s
                                lockerAmountS = amount;

                                amount.Amount = formRequestDto.SAmount;
                                amount.UpdateDate = DateTime.Now;
                                context.SaveChanges();
                            }
                            else if (amount.LkSizeId == lockerSizeList.Find(c => c.LkSizeName.Equals("M")).LkSizeId)
                            {
                                //for rollback lockerAmount size M
                                lockerAmountM = amount;

                                amount.Amount = formRequestDto.MAmount;
                                amount.UpdateDate = DateTime.Now;
                                context.SaveChanges();
                            }
                            else if (amount.LkSizeId == lockerSizeList.Find(c => c.LkSizeName.Equals("L")).LkSizeId)
                            {
                                //for rollback lockerAmount size L
                                lockerAmountL = amount;

                                amount.Amount = formRequestDto.LAmount;
                                amount.UpdateDate = DateTime.Now;
                                context.SaveChanges();
                            }
                        }
                        ResponseHeader response = new ResponseHeader
                        {
                            Status = "S",
                            Message = "Edit form locker request sucessful"
                        };
                        return response;
                    }
                }
                
            }
            catch(ArgumentException e)
            {
                throw new ArgumentException(e.Message, e.InnerException);
            }
            catch (Exception e)
            {
                using var context = new SmartLockerContext();
                //delete form data cause error
                if (formRequestDto.FormId <= 0)
                {
                    if (lockerAmountS.LkAmountId > 0)
                    {
                        Console.WriteLine("clear lockerAmountS");

                        context.LockerAmount.Remove(context.LockerAmount.Where(c => c.LkAmountId == lockerAmountS.LkAmountId).FirstOrDefault());
                        context.SaveChanges();
                    }
                    if (lockerAmountL.LkAmountId > 0)
                    {
                        Console.WriteLine("clear lockerAmountL" + lockerAmountL.LkAmountId);

                        context.LockerAmount.Remove(context.LockerAmount.Where(c => c.LkAmountId == lockerAmountL.LkAmountId).FirstOrDefault());
                        context.SaveChanges();

                    }
                    if (lockerAmountM.LkAmountId > 0)
                    {
                        Console.WriteLine("clear lockerAmountM");

                        context.LockerAmount.Remove(context.LockerAmount.Where(c => c.LkAmountId == lockerAmountM.LkAmountId).FirstOrDefault());
                        context.SaveChanges();

                    }
                    if (formRequestLockers.FormId > 0)
                    {
                        Console.WriteLine("clear formRequestLockers");
                        context.FormRequestLockers.Remove(context.FormRequestLockers.Where(c => c.FormId == formRequestLockers.FormId).FirstOrDefault());
                        context.SaveChanges();

                    }
                    if (lockerNew.LockerId > 0)
                    {
                        Console.WriteLine("clear lockerNew");
                        context.Lockers.Remove(context.Lockers.Where(c => c.LockerId == lockerNew.LockerId).FirstOrDefault());
                        context.SaveChanges();

                    }
                }
                //roll back form cause error
                else
                {
                    //roll back formRequestLocker
                    var formRollback =  context.FormRequestLockers.Where(c => c.FormId == formRequestDto.FormId).FirstOrDefault();
                    formRollback.Status = formRequestLockers.Status;
                    formRollback.OptionalRequest = formRequestLockers.OptionalRequest;
                    formRequestLockers.UpdateDate = formRequestLockers.UpdateDate;
                    context.SaveChanges();

                    //roll back locate in locker
                    Lockers rollBackLocker = context.Lockers.Where(c => c.LockerId == formRequestLockers.LockerId).FirstOrDefault();
                    rollBackLocker.LocateId = lockerNew.LocateId;
                    rollBackLocker.UpdateDate = DateTime.Now;
                    context.SaveChanges();

                    //roll back LockerAmount
                    List<LockerAmount> lockerAmount = context.LockerAmount.Where(c => c.FormId == formRequestLockers.FormId).ToList();
                    List<LockerSizes> lockerSizeList = context.LockerSizes.ToList();

                    foreach (LockerAmount amount in lockerAmount)
                    {
                        if (amount.LkSizeId == lockerSizeList.Find(c => c.LkSizeName.Equals("S")).LkSizeId)
                        {
                            //for rollback lockerAmount size s
                            amount.Amount = lockerAmountS.Amount;
                            amount.UpdateDate = lockerAmountS.UpdateDate;
                            context.SaveChanges();
                        }
                        else if (amount.LkSizeId == lockerSizeList.Find(c => c.LkSizeName.Equals("M")).LkSizeId)
                        {
                            //for rollback lockerAmount size M
                            amount.Amount = lockerAmountM.Amount;
                            amount.UpdateDate = lockerAmountM.UpdateDate;
                            context.SaveChanges();
                        }
                        else if (amount.LkSizeId == lockerSizeList.Find(c => c.LkSizeName.Equals("L")).LkSizeId)
                        {
                            //for rollback lockerAmount size L
                            amount.Amount = lockerAmountL.Amount;
                            amount.UpdateDate = lockerAmountL.UpdateDate;
                            context.SaveChanges();
                        }
                    }
                }
                throw new Exception(e.Message, e.InnerException);
            }



        }
        private string GenerateFormCode(int count, string pre)
        {
            StringBuilder formCode = new StringBuilder();
            formCode.Append(++count);
            for (; formCode.ToString().Length <= 6;)
            {
                formCode.Insert(0, "0");
            }
            formCode.Insert(0, pre);

            return formCode.ToString();
        }
        public ResponseHeader GetAllFormRequestByAccountId(int page, int perPage, int accountId, string keyWord)
        {
            //Get form list 
            var formListResult = formRequestLockerDA.GetAllFormRequestByAccountId(page, perPage, accountId, keyWord);
            List<FormRequestDto> forms = formListResult.Content;

            //Get locker amount for each size
            using var context = new SmartLockerContext();
            List<LockerSizes> lockerSizeList = context.LockerSizes.ToList();
            foreach (FormRequestDto form in forms)
            {
                int sizeId;

                //Size S amount
                sizeId = lockerSizeList.Find(c => c.LkSizeName.Equals("S")).LkSizeId;
                form.SAmount = formRequestLockerDA.GetRoomAmountByLkSizeIdAndFormId(sizeId, form.FormId);

                //Size M amount
                sizeId = lockerSizeList.Find(c => c.LkSizeName.Equals("M")).LkSizeId;
                form.MAmount = formRequestLockerDA.GetRoomAmountByLkSizeIdAndFormId(sizeId, form.FormId);

                //Size L amount
                sizeId = lockerSizeList.Find(c => c.LkSizeName.Equals("L")).LkSizeId;
                form.LAmount = formRequestLockerDA.GetRoomAmountByLkSizeIdAndFormId(sizeId, form.FormId);
            }

            //Return to controller
            return new ResponseHeader
            {
                Status = "S",
                Message = "Get list form successful",
                Content = forms,
                Page = formListResult.Page,
                PerPage = formListResult.PerPage,
                TotalElement = formListResult.TotalElement,
            };

        }
        public ResponseHeader SearchFormRequest(int page, int perPage, FormRequestDto keyWord)
        {
            //Get form list 
            var formListResult = formRequestLockerDA.SearchFormRequest(page, perPage, keyWord);
            List<FormRequestDto> forms = formListResult.Content;

            //Get locker amount for each size
            using var context = new SmartLockerContext();
            List<LockerSizes> lockerSizeList = context.LockerSizes.ToList();
            foreach (FormRequestDto form in forms)
            {
                int sizeId;

                //Size S amount
                sizeId = lockerSizeList.Find(c => c.LkSizeName.Equals("S")).LkSizeId;
                form.SAmount = formRequestLockerDA.GetRoomAmountByLkSizeIdAndFormId(sizeId, form.FormId);

                //Size M amount
                sizeId = lockerSizeList.Find(c => c.LkSizeName.Equals("M")).LkSizeId;
                form.MAmount = formRequestLockerDA.GetRoomAmountByLkSizeIdAndFormId(sizeId, form.FormId);

                //Size L amount
                sizeId = lockerSizeList.Find(c => c.LkSizeName.Equals("L")).LkSizeId;
                form.LAmount = formRequestLockerDA.GetRoomAmountByLkSizeIdAndFormId(sizeId, form.FormId);
            }

            //Return to controller
            return new ResponseHeader
            {
                Status = "S",
                Message = "Get list form successful",
                Content = forms,
                Page = formListResult.Page,
                PerPage = formListResult.PerPage,
                TotalElement = formListResult.TotalElement,
            };

        }
        public ResponseHeader ApproveForm(BaseUpdateDto baseUpdateDto)
        {
            using var context = new SmartLockerContext();
            var formResult = context.FormRequestLockers.Where(c => c.FormId == baseUpdateDto.Id).FirstOrDefault();
            if (formResult == null)
            {
                throw new Exception("FormId not found");
            }
            formResult.Status = baseUpdateDto.Status;
            formResult.Remark = baseUpdateDto.Remark;
            formResult.UpdateDate = DateTime.Now;
            context.SaveChanges();
            return new ResponseHeader
            {
                Status = "S",
                Message = "Change form status sucessful",
            };
        }

        public ResponseHeader ApproveFormAndInstallLocker(BaseUpdateDto baseUpdateDto)
        {
            using var context = new SmartLockerContext();
            var formResult = context.FormRequestLockers.Where(c => c.FormId == baseUpdateDto.Id).FirstOrDefault();
            if (formResult == null)
            {
                throw new ArgumentException("FormId not found");
            }else if(formResult.Status != "WA")
            {
                throw new ConfilctDataException("FormId cannot use this function.");
            }
            else
            {
                //change to finish form
                formResult.Status = "A";
                formResult.Remark = baseUpdateDto.Remark;
                formResult.UpdateDate = DateTime.Now;
                context.SaveChanges();

                //installation locker
                var locker = context.Lockers.Where(c => c.LockerId == formResult.LockerId).FirstOrDefault();
                locker.Status = "A";
                locker.UpdateDate = DateTime.Now;
                context.SaveChanges();
            }
            
            return new ResponseHeader
            {
                Status = "S",
                Message = "install locker sucessful",
            };
        }
        private List<LockerRoomDto> GenerateLockerRoomCode(List<LockerRoomDto> lockerRoomBefore, List<LockerSizes> lockerSizeList)
        {
            int SCount = 1;
            int MCount = 1;
            int LCount = 1;
            foreach (LockerRoomDto lockerRoom in lockerRoomBefore)
            {


                if (lockerRoom.LkSizeId == lockerSizeList.Where(c => c.LkSizeName.Equals("S")).FirstOrDefault().LkSizeId)
                {
                    string number = SCount < 10 ? ("0" + SCount) : SCount.ToString();
                    lockerRoom.LkRoomCode = "S" + number;
                    SCount++;
                }
                else if (lockerRoom.LkSizeId == lockerSizeList.Where(c => c.LkSizeName.Equals("M")).FirstOrDefault().LkSizeId)
                {
                    string number = SCount < 10 ? ("0" + MCount) : MCount.ToString();
                    lockerRoom.LkRoomCode = "M" + number;
                    MCount++;
                }
                else if (lockerRoom.LkSizeId == lockerSizeList.Where(c => c.LkSizeName.Equals("L")).FirstOrDefault().LkSizeId)
                {
                    string number = SCount < 10 ? ("0" + LCount) : LCount.ToString();
                    lockerRoom.LkRoomCode = "L" + number;
                    LCount++;
                }
            }
            return lockerRoomBefore;
        }
        public ResponseHeader CreateLockerDiagramByFormId(FormCreateDiagramDto formCreateDiagram)
        {
            List<LockerRooms> lockerRoomList = new List<LockerRooms>();
            List<LockerDiagrams> lockerDiagramList = new List<LockerDiagrams>();
            List<LockerSizes> lockerSizeList = new List<LockerSizes>();
            FormRequestLockers formResult = new FormRequestLockers();
            try
            {
                using var context = new SmartLockerContext();
                lockerSizeList = context.LockerSizes.ToList();
                formResult = context.FormRequestLockers.Where(c => c.FormId == formCreateDiagram.FormId).FirstOrDefault();

                //prepare: check exist form and status 
                if (formResult == null)
                {
                    throw new Exception("FormId not found");
                }
                //prepare: if this is WD form (Create diagram)
                else if (formResult.Status.Equals(FormStatus.WaitforDiagram))
                {
                    formResult.Status = "VD";
                    formResult.UpdateDate = DateTime.Now;

                    //prepare: generate lockerRoom code
                    formCreateDiagram.LockerRoomList = GenerateLockerRoomCode(formCreateDiagram.LockerRoomList, lockerSizeList);

                    //first: add lockerRoom
                    foreach (LockerRoomDto lockerRoom in formCreateDiagram.LockerRoomList)
                    {

                        LockerRooms lockerRoomNew = new LockerRooms
                        {
                            LockerId = formResult.LockerId,
                            LkSizeId = lockerRoom.LkSizeId,
                            Status = "A",
                            CreateDate = DateTime.Now,
                            LkRoomCode = lockerRoom.LkRoomCode
                        };
                        context.LockerRooms.Add(lockerRoomNew);
                        context.SaveChanges();

                        lockerRoom.LkRoomId = lockerRoomNew.LkRoomId;
                        lockerRoomList.Add(lockerRoomNew);
                    }


                    //second: add locker diagram
                    foreach (LockerRoomDto lockerRoom in formCreateDiagram.LockerRoomList)
                    {

                        LockerDiagrams lockerDiagram = new LockerDiagrams
                        {
                            ColumnPosition = lockerRoom.ColumnPosition,
                            RowPosition = lockerRoom.RowPosition,
                            LkRoomId = lockerRoom.LkRoomId,
                            Status = "A",
                            CreateDate = DateTime.Now,
                        };
                        context.LockerDiagrams.Add(lockerDiagram);
                        context.SaveChanges();
                        lockerDiagramList.Add(lockerDiagram);
                    }

                    return new ResponseHeader
                    {
                        Status = "S",
                        Message = "Add locker room and locker diagram by form id successful"
                    };
                }
                //prepare: if this is RD form (Edit diagram)
                else if (formResult.Status.Equals(FormStatus.RejectDiagram))
                {
                    formResult.Status = "VD";
                    formResult.UpdateDate = DateTime.Now;

                    //first: get locker diagram by locker diagram and edit diagram
                    foreach (LockerRoomDto lockerRoom in formCreateDiagram.LockerRoomList)
                    {
                        //get locker diagram by 

                        LockerDiagrams lockerDiagrams = context.LockerDiagrams.Where(c => c.LkRoomId == lockerRoom.LkRoomId).FirstOrDefault();
                        //add to lockerDiagramList for roll back
                        lockerDiagramList.Add(lockerDiagrams);

                        //edit diagram
                        lockerDiagrams.ColumnPosition = lockerRoom.ColumnPosition;
                        lockerDiagrams.RowPosition = lockerRoom.RowPosition;
                        lockerDiagrams.UpdateDate = DateTime.Now;

                        context.SaveChanges();
                    }
                    return new ResponseHeader
                    {
                        Status = "S",
                        Message = "Edit locker diagram by form id successful"
                    };

                }
                else
                {
                    throw new Exception("form's status is not correct for this function");
                }

            }
            catch (Exception e)
            {
                using var context = new SmartLockerContext();
                
                var formrollback = context.FormRequestLockers.Where(c => c.FormId == formCreateDiagram.FormId).FirstOrDefault();
                var statusForm = formrollback.Status;
                if (formrollback.FormId > 0)
                {
                    formrollback.Status = "WD";
                    formrollback.UpdateDate = DateTime.Now;
                    context.SaveChanges();
                }
                if (statusForm.Equals(FormStatus.WaitforDiagram)){
                    foreach (LockerDiagrams lockerDiagrams in lockerDiagramList)
                    {
                        var diagramRemove = context.LockerDiagrams.Where(c => c.LkDiagramId == lockerDiagrams.LkDiagramId).FirstOrDefault();
                        context.LockerDiagrams.Remove(diagramRemove);
                        context.SaveChanges();
                    }
                    foreach (LockerRooms lockerRoom in lockerRoomList)
                    {
                        var lockerRoomRemove = context.LockerRooms.Where(c => c.LkRoomId == lockerRoom.LkRoomId).FirstOrDefault();
                        context.LockerRooms.Remove(lockerRoomRemove);
                        context.SaveChanges();
                    }
                }else if (statusForm.Equals(FormStatus.RejectDiagram))
                {
                    foreach (LockerDiagrams lockerDiagrams in lockerDiagramList)
                    {
                        var diagramRollback = context.LockerDiagrams.Where(c => c.LkDiagramId == lockerDiagrams.LkDiagramId).FirstOrDefault();
                        diagramRollback.ColumnPosition = lockerDiagrams.ColumnPosition;
                        diagramRollback.RowPosition = lockerDiagrams.RowPosition;
                        diagramRollback.UpdateDate = lockerDiagrams.UpdateDate;
                        context.SaveChanges();
                    }
                }
                throw new Exception(e.Message, e.InnerException);
            }
        }
    }
}
