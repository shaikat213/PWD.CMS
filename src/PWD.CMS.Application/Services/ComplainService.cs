using PWD.CMS.CMSEnums;
using PWD.CMS.DtoModels;
using PWD.CMS.Interfaces;
using PWD.CMS.Models;
using PWD.CMS.Permissions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
using Volo.Abp.Emailing;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using System.Net.Http.Headers;
using System.Net.Http;
//using System.Timers;

namespace PWD.CMS.Services
{
    public class ComplainService : CMSAppService, IComplainAppService
    {
        private readonly IRepository<Complain, int> repository;
        private readonly IRepository<ComplainHistory, int> repositoryHistory;
        private readonly IRepository<Apartment, int> apartmentRepository;
        private readonly IRepository<Allotment, int> allotmentRepository;
        private readonly IRepository<PwdTenant, int> tenantRepository;
        private readonly IRepository<Building, int> buildingRepository;
        private readonly IRepository<ProblemType, int> problemTypeRepository;
        private readonly IRepository<Otp, int> repositoryOtp;
        private readonly IRepository<Quarter, int> quarterRepository;
        private readonly IRepository<District, int> districtRepository;
        private readonly IOrganizaitonUnitAppService organizaitonUnitAppService;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly NotificationAppService notificationAppService;
        private readonly OrganizaitonUnitAppService organizaitonUnitService;
        //private readonly IEmailSender _emailSender;

        //IEmailSender emailSender
        public ComplainService(
            IRepository<Complain, int> repository,
            IRepository<ComplainHistory, int> repositoryHistory,
            IRepository<Apartment, int> apartmentRepository,
            IRepository<Allotment, int> allotmentRepository,
            IRepository<PwdTenant, int> tenantRepository,
            IRepository<Building, int> buildingRepository,
            IRepository<ProblemType, int> problemTypeRepository,
            IRepository<Otp, int> repositoryOtp,
            IRepository<Quarter, int> quarterRepository,
            IRepository<District, int> districtRepository,
            IOrganizaitonUnitAppService organizaitonUnitAppService,
            OrganizaitonUnitAppService organizaitonUnitService,
            NotificationAppService notificationAppService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            this.repository = repository;
            this.repositoryHistory = repositoryHistory;
            this.apartmentRepository = apartmentRepository;
            this.allotmentRepository = allotmentRepository;
            this.tenantRepository = tenantRepository;
            this.buildingRepository = buildingRepository;
            this.problemTypeRepository = problemTypeRepository;
            this.repositoryOtp = repositoryOtp;
            this.quarterRepository = quarterRepository;
            this.districtRepository = districtRepository;
            this.organizaitonUnitAppService = organizaitonUnitAppService;
            this.organizaitonUnitService = organizaitonUnitService;
            this.notificationAppService = notificationAppService;
            this.unitOfWorkManager = unitOfWorkManager;
            //Thread ThreadObject1 = new Thread(Example1);
            //ThreadObject1.Start();
            //this._emailSender = emailSender;
        }
        //public ComplainService() //
        //{
        //   // this.repository1 = repository1;
        //}
        private static System.Threading.Timer timer;
        
        public async Task<ComplainDto> CreateAsync(CreateComplainDto input)
        {
            var sdePhone = "";
            var sdeOffice = "";
            var sdeEmail = "";
            var complain = new Complain();
            var apartment = await this.apartmentRepository.GetAsync(input.ApartmentId);
            if (apartment != null)
            {
                complain.AllotmentId = (int)apartment.AllotmentId;
                complain.ProblemTypeId = input.ProblemTypeId;
                complain.Description = input.Description;
                complain.ComplainStatus = ComplainStatus.New;
                complain.Date = DateTime.Now;
                complain.TicketNumber = CmsUtility.GetRandomNo(10000, 99999).ToString();//.RandomString(8);
                complain.PostingId = input.PostingId;
                complain.OrganizationalUnitId = input.OrganizationalUnitId;
                sdePhone = organizaitonUnitService.GetPostingById(complain.PostingId).Result.EmpPhoneMobile;
                sdeOffice = organizaitonUnitService.GetPostingById(complain.PostingId).Result.Office;
                sdeEmail = organizaitonUnitService.GetPostingById(complain.PostingId).Result.EmpEmail;
            }

            var newComplain = await repository.InsertAsync(complain, true);
            if (newComplain != null)
            {
                //using (var uow = unitOfWorkManager.Begin(
                //    requiresNew: true, isTransactional: false
                //))
                //{
                //    try
                //    {
                //        if (input.TokenNo != null)
                //        {
                //            var item = await repositoryOtp.FirstOrDefaultAsync(x => x.OtpNo == input.TokenNo && x.ExpireDateTime >= DateTime.Now);
                //            if (item != null)
                //            {
                //                item.OtpStatus = OtpStatus.Varified;
                //                await repositoryOtp.UpdateAsync(item);
                //                await unitOfWorkManager.Current.SaveChangesAsync();
                //            }
                //        }
                //    }
                //    finally
                //    {
                //        await uow.CompleteAsync();
                //    }
                //}
                // Send SMS notification
                if (!string.IsNullOrEmpty(input.TenantMobile))
                {
                    SmsRequestInput otpInput = new SmsRequestInput();
                    otpInput.Sms = String.Format("Your complaint submitted successfully. Your complaint ticket number is " + complain.TicketNumber + ". Please preserve this complaint ticket for further reference.");
                    otpInput.Msisdn = input.TenantMobile;
                    otpInput.CsmsId = GenerateTransactionId(16);


                    try
                    {
                        //var res = await notificationAppService.SendSmsTestAlpha(otpInput);
                        var res = await notificationAppService.SendSmsNotification(otpInput);
                        if (!string.IsNullOrEmpty(sdePhone))
                        {
                            SmsRequestInput otpInput2 = new SmsRequestInput();
                            otpInput2.Sms = String.Format("A new complain submitted to your office, " + sdeOffice + " from CMS. The complaint ticket number is " + complain.TicketNumber + ". Please take necessary action for the complaint immediately.");
                            otpInput2.Msisdn = sdePhone;
                            otpInput2.CsmsId = GenerateTransactionId(16);

                            var res2 = await notificationAppService.SendSmsNotification(otpInput2);
                        }
                        //if (!string.IsNullOrEmpty(sdeEmail))
                        //{
                        //    await _emailSender.SendAsync(sdeEmail,     // target email address
                        //                                 "New Complaint from CMS",         // subject
                        //                                  String.Format("A new complain submitted to your office, " + sdeOffice + " from CMS. The complaint id "
                        //                                  + complain.TicketNumber + ". Please take necessary action for the complain immediately."));  // email body
                        //}
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
                //end 
            }
            //call notification service
            //NotificaitonCommonDto ndto = new NotificaitonCommonDto();
            //ndto.Message = $"Leave has just been created by Mojidul";
            //ndto.CreatedBy = GuidGenerator.Create();// CurrentUser.Id.Value;
            //ndto.CreatedFor = GuidGenerator.Create();
            //ndto.CreatorName = "EE";
            //ndto.ReceiverName = "SE";
            //ndto.Source = "SD Office Location Source";
            //ndto.Destination = "SE Office Location PWD";
            //ndto.Priority = Priority.High;
            //ndto.Status = Status.Created;
            //await notificationAppService.CreateAsync(notificationAppService.ConvertToNotification(ndto));

            var result = ObjectMapper.Map<Complain, ComplainDto>(newComplain);
            return result;

        }

        public async Task<ComplainDto> GetAsync(int id)
        {
            var c = new ComplainDto();
            var item = await repository.WithDetailsAsync(p => p.ComplainHistories);
            var complain = item.FirstOrDefault(c => c.Id == id);
            return ObjectMapper.Map<Complain, ComplainDto>(complain);
        }

        public async Task<ComplainDto> GetByTicketAsync(string ticket)
        {
            var c = new ComplainDto();
            var item = await repository.WithDetailsAsync(p => p.ComplainHistories);
            var complain = item.FirstOrDefault(c => c.TicketNumber == ticket);
            return ObjectMapper.Map<Complain, ComplainDto>(complain);
        }

        public async Task<ComplainDto> GetComplainAsync(int id)
        {
            var c = new ComplainDto();
            var item = await repository.WithDetailsAsync(p => p.ComplainHistories);
            var complain = item.FirstOrDefault(c => c.Id == id);
            var allotment = await allotmentRepository.FindAsync(complain.AllotmentId);
            var apartment = await apartmentRepository.FindAsync(allotment.ApartmentId);
            var tenant = await tenantRepository.FindAsync(allotment.PwdTenantId);
            var building = await buildingRepository.FindAsync(apartment.BuildingId);
            var quarters = await quarterRepository.WithDetailsAsync(q => q.District);// .FindAsync(building.QuarterId);
            var quarter = quarters.FirstOrDefault(d => d.Id == building.QuarterId);

            //zvar appartment = await apartmentRepository.FindAsync(allotment.)
            if (complain != null)
            {
                c.TenantName = tenant.Name;
                c.TenantMobile = tenant.Mobile;
                c.Address = building.Name + ", "
                            + quarter.Name + ", "
                            + quarter.District.Name;
                c.Date = complain.Date;
                c.Description = complain.Description;
                c.ComplainStatus = complain.ComplainStatus;
                c.Remark = complain.Remark;
                c.Cost = complain.Cost;
                c.MaterialsUsed = complain.MaterialsUsed;
                //c.ComplainHistories =  complain.ComplainHistories;
            }
            return c;//ObjectMapper.Map<Complain, ComplainDto>(complain);
        }

        public async Task<ComplainSearchResultDto> GetComplainByTicketAsync(string ticket)
        {
            //var item = await repository.WithDetailsAsync(p => p.ComplainHistories);
            //var complain = item.FirstOrDefault(c => c.TicketNumber == ticket);
            //return ObjectMapper.Map<Complain, ComplainDto>(complain);
            ComplainSearchResultDto complainSearchResult = null;
            Building building = null;
            Quarter quarter = null;
            District district = null;
            var item = await repository.WithDetailsAsync(c => c.Allotment, c => c.ProblemType);
            var complain = item.FirstOrDefault(c => c.TicketNumber == ticket);
            if (complain != null)
            {
                var tenant = tenantRepository.FirstOrDefaultAsync(t => t.Id == complain.Allotment.PwdTenantId)?.Result;
                var appartment = apartmentRepository.FirstOrDefaultAsync(t => t.AllotmentId == complain.AllotmentId)?.Result;
                complainSearchResult = new ComplainSearchResultDto();
                complainSearchResult.Id = complain.Id;
                complainSearchResult.TicketNo = complain.TicketNumber;
                complainSearchResult.ComplainStatus = complain.ComplainStatus.ToString();
                string cmpSts = complain.ComplainStatus.ToString();
                if (cmpSts == "SiteVisit")
                {
                    complainSearchResult.ComplainStatus = "Site Visit";
                }
                else if (cmpSts == "InProgress")
                {
                    complainSearchResult.ComplainStatus = "In Progress";
                }
                else if (cmpSts == "TenantFeedback")
                {
                    complainSearchResult.ComplainStatus = "Allotee Feedback";
                }
                else
                {
                    complainSearchResult.ComplainStatus = complain.ComplainStatus.ToString();
                }
                complainSearchResult.ProblemDescription = complain.Description;
                complainSearchResult.SubmitedDate = complain.CreationTime.ToString("dd/MM/yyyy");
                complainSearchResult.TenantFeedback = complain.TenantFeedback;
                complainSearchResult.TenantId = tenant.Id;
                complainSearchResult.TenantName = tenant.Name;
                complainSearchResult.TenantMobileNo = tenant.Mobile;
                complainSearchResult.ProblemTypeId = complain.ProblemTypeId;
                complainSearchResult.ProblemTypeStr = complain.ProblemType?.Name;
                complainSearchResult.ApartmentId = appartment?.Id;
                complainSearchResult.ApartmentName = appartment?.Name;
                if (appartment != null)
                {
                    building = buildingRepository.FirstOrDefaultAsync(b => b.Id == appartment.BuildingId)?.Result;
                    if (building != null)
                    {
                        complainSearchResult.BuildingId = building.Id;
                        complainSearchResult.BuildingName = building.Name;
                        quarter = quarterRepository.FirstOrDefaultAsync(b => b.Id == building.QuarterId)?.Result;
                        if (quarter != null)
                        {
                            complainSearchResult.QuarterId = quarter.Id;
                            complainSearchResult.QuarterName = quarter.Name;
                            district = districtRepository.FirstOrDefaultAsync(d => d.Id == quarter.DistrictId)?.Result;
                            if (district != null)
                            {
                                complainSearchResult.DistrictId = district.Id;
                                complainSearchResult.DistrictName = district.Name;
                            }
                        }
                    }
                }
            }

            return complainSearchResult;
        }

        public async Task<List<ComplainSearchResultDto>> GetComplainByMobileNoAsync(string mobileNo)
        {
            List<ComplainSearchResultDto> complainSearchResultList = null;
            if (!string.IsNullOrEmpty(mobileNo))
            {
                var tenant = tenantRepository.FirstOrDefaultAsync(t => t.Mobile == mobileNo)?.Result;
                if (tenant != null && tenant.AllotmentId.HasValue)
                {
                    var items = await repository.WithDetailsAsync(c => c.Allotment, c => c.ProblemType);
                    var complaints = items.Where(x => x.AllotmentId == tenant.AllotmentId);
                    if (complaints.Any())
                    {
                        complainSearchResultList = (from complain in complaints
                                                    join apartment in await apartmentRepository.WithDetailsAsync()
                                                    on complain.AllotmentId equals apartment.AllotmentId
                                                    select new ComplainSearchResultDto
                                                    {
                                                        Id = complain.Id,
                                                        TicketNo = complain.TicketNumber,
                                                        ComplainStatus = ((ComplainStatus)complain.ComplainStatus).ToString(),
                                                        ProblemDescription = complain.Description,
                                                        SubmitedDate = complain.CreationTime.ToString("dd/MM/yyyy"),
                                                        TenantFeedback = complain.TenantFeedback,
                                                        TenantId = tenant.Id,
                                                        TenantName = tenant.Name,
                                                        TenantMobileNo = tenant.Mobile,
                                                        ProblemTypeId = complain.ProblemTypeId,
                                                        ProblemTypeStr = complain.ProblemType.Name,
                                                        ApartmentId = apartment.Id,
                                                        ApartmentName = apartment.Name
                                                    }).ToList();

                    }
                }
            }

            return complainSearchResultList;
        }

        public async Task<ComplainDto> UpdateAsync(ComplainDto input)
        {
            var oldItem = await repository.FindAsync(input.Id);
            if (oldItem == null)
            {
                return new ComplainDto();
            }
            else
            {
                if (oldItem.ComplainStatus != input.ComplainStatus)
                {
                    ComplainHistory newComplainHistory = new ComplainHistory()
                    {
                        ComplainId = oldItem.Id,
                        Date = DateTime.Now,
                        Remark = oldItem.Remark,
                        ComplainStatus = oldItem.ComplainStatus,
                        PostingId = oldItem.PostingId,
                        AllotmentId = oldItem.AllotmentId
                    };
                    await repositoryHistory.InsertAsync(newComplainHistory, true);
                }
                Complain item = null;
                using (var uow = unitOfWorkManager.Begin(
                    requiresNew: true, isTransactional: false
                ))
                {
                    try
                    {
                        if (input.Cost.HasValue)
                        {
                            oldItem.Cost = input.Cost.Value;
                            oldItem.MaterialsUsed = input.MaterialsUsed;
                        }

                        if (oldItem.ComplainStatus != input.ComplainStatus)
                        {
                            oldItem.ComplainStatus = input.ComplainStatus;
                            oldItem.Remark = input.Remark;
                        }
                        item = await repository.UpdateAsync(oldItem);
                        await unitOfWorkManager.Current.SaveChangesAsync();
                    }
                    finally
                    {
                        await uow.CompleteAsync();
                    }
                }

                return ObjectMapper.Map<Complain, ComplainDto>(item);
            }
        }

        public async Task<bool> UpdateTenantFeedbackAsync(int id, string feedback)
        {
            var oldItem = await repository.FindAsync(id);
            if (oldItem == null)
            {
                return false;
            }
            else
            {
                oldItem.TenantFeedback = feedback;
                await repository.UpdateAsync(oldItem);
                return true;
            }
        }

        public async Task<IEnumerable<ComplaintListDto>> GetComplainListAsync(ComplainSearchDto search, FilterModel filterModel)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            var allotments = await allotmentRepository.WithDetailsAsync(p => p.PwdTenant);
            //var apartments = await apartmentRepository.WithDetailsAsync();
            try
            {
                var joinData = complaints.Where(c => c.IsDeleted == false).Join(allotments, c => c.AllotmentId, e => e.Id, (c, e) => new ComplaintListDto()
                {
                    Id = c.Id,
                    Comment = c.Remark,
                    TicketNumber = c.TicketNumber,
                    ComplainStatusId = (int?)c.ComplainStatus,
                    ComplainStatusStr = ((ComplainStatus)c.ComplainStatus).ToString(),
                    SubmittedDate = c.CreationTime.ToString("dd/MM/yyyy"),
                    ProblemTypeId = c.ProblemTypeId,
                    ProblemTypeStr = c.ProblemType == null ? "" : c.ProblemType.Name,
                    ProblemDescription = c.Description,
                    TenantId = e.Id,
                    TenantName = e.PwdTenant.Name,
                    MobileNo = e.PwdTenant.Mobile,
                    Address = "Building: " + e.Apartment.Building.Name + ", "
                              + "Quarter: " + e.Apartment.Building.Quarter.Name + ", "
                              + e.Apartment.Building.Quarter.District.Name,//"Dhaka", //TODO should be get data from ?? entity
                    CreateDate = c.CreationTime,
                    Cost = c.Cost,
                    MaterialsUsed = c.MaterialsUsed
                });

                if (!string.IsNullOrEmpty(search.StartDate) && !string.IsNullOrEmpty(search.EndDate))
                {
                    joinData = joinData.Where(p => p.CreateDate.Value.Date >= DateTime.ParseExact(search.StartDate, "dd/MM/yyyy", provider, DateTimeStyles.None)
                    && p.CreateDate.Value.Date <= DateTime.ParseExact(search.EndDate, "dd/MM/yyyy", provider, DateTimeStyles.None));
                }
                if (search.StatusId > 0)
                {
                    joinData = joinData.Where(p => p.ComplainStatusId == search.StatusId);

                }
                joinData = joinData.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
                return joinData.ToList();
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return null;
            }
        }

        //public async Task<IEnumerable<ComplaintListDto>> GetComplainListBySDIdAsync(ComplainSearchDto search, Guid? civilSDId, FilterModel filterModel)
        public async Task<IEnumerable<ComplaintListDto>> GetComplainListBySDIdAsync(ComplainSearchDto search, FilterModel filterModel)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            var allotments = await allotmentRepository.WithDetailsAsync(p => p.PwdTenant);
            //var apartments = await apartmentRepository.WithDetailsAsync(p => p.Allotment);
            try
            {
                var joinData = complaints.Where(c => c.IsDeleted == false && 
                (c.OrganizationalUnitId == search.SubDivisionId
                //c.Allotment.Apartment.Building.CivilSubDivisionId == search.SubDivisionId
                //|| c.Allotment.Apartment.Building.EmSubDivisionId == search.SubDivisionId
                )
                || c.Allotment.Apartment.Building.QuarterId == search.QuarterId
                || c.Allotment.Apartment.BuildingId == search.BuildingId).Join(allotments, c => c.AllotmentId, e => e.Id, (c, e) => new ComplaintListDto()
                {
                    Id = c.Id,
                    Comment = c.Remark,
                    TicketNumber = c.TicketNumber,
                    ComplainStatusId = (int?)c.ComplainStatus,
                    ComplainStatusStr = ((ComplainStatus)c.ComplainStatus).ToString(),
                    FeedBack = string.IsNullOrEmpty(c.TenantFeedback) ? "NA" : c.TenantFeedback,
                    SubmittedDate = c.CreationTime.ToString("dd/MM/yyyy"),
                    ProblemTypeId = c.ProblemTypeId,
                    ProblemTypeStr = c.ProblemType == null ? "" : c.ProblemType.Name,
                    ProblemDescription = c.Description,
                    TenantId = e.PwdTenant.Id,//e.Id,
                    TenantName = e.PwdTenant.Name,
                    MobileNo = e.PwdTenant.Mobile,
                    Address = "Building: " + e.Apartment.Building.Name + ", "
                              + "Quarter: " + e.Apartment.Building.Quarter.Name + ", "
                              + e.Apartment.Building.Quarter.District.Name,//"Dhaka", //TODO should be get data from ?? entity
                    CreateDate = c.CreationTime,
                    Cost = c.Cost,
                    MaterialsUsed = c.MaterialsUsed
                });

                if (!string.IsNullOrEmpty(search.StartDate) && !string.IsNullOrEmpty(search.EndDate))
                {
                    joinData = joinData.Where(p => p.CreateDate.Value.Date >= DateTime.ParseExact(search.StartDate, "dd/MM/yyyy", provider, DateTimeStyles.None) && p.CreateDate.Value.Date <= DateTime.ParseExact(search.EndDate, "dd/MM/yyyy", provider, DateTimeStyles.None));
                }
                if (search.StatusId > 0)
                {
                    joinData = joinData.Where(p => p.ComplainStatusId == search.StatusId);

                }

                joinData = joinData.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
                return joinData.ToList();
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return null;
            }
        }

        public async Task<IEnumerable<ComplaintListDto>> GetComplainListBySearchAsync(ComplainSearchDto search, FilterModel filterModel)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                var complaints = await repository.WithDetailsAsync(p => p.Allotment);
                var allotments = await allotmentRepository.WithDetailsAsync(p => p.PwdTenant);

                var fromDate = DateTime.ParseExact(search.StartDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
                var toDate = DateTime.ParseExact(search.EndDate, "dd/MM/yyyy", provider, DateTimeStyles.None);



                var joinData = complaints.Where(c => c.IsDeleted == false
                && (c.Date.Date >= fromDate && c.Date.Date <= toDate)
                && (
                c.OrganizationalUnitId == search.SubDivisionId //|| c.Allotment.Apartment.Building.EmSubDivisionId == search.SubDivisionId
                //c.Allotment.Apartment.Building.CivilSubDivisionId == search.SubDivisionId
                //|| c.Allotment.Apartment.Building.EmSubDivisionId == search.SubDivisionId
                )
                || c.Allotment.Apartment.Building.QuarterId == search.QuarterId
                || c.Allotment.Apartment.BuildingId == search.BuildingId)
                    .Join(allotments, c => c.AllotmentId, e => e.Id, (c, e) => new ComplaintListDto()
                    {

                        Id = c.Id,
                        Comment = c.Remark,
                        TicketNumber = c.TicketNumber,
                        ComplainStatusId = (int?)c.ComplainStatus,
                        ComplainStatusStr = ((ComplainStatus)c.ComplainStatus).ToString(),
                        FeedBack = string.IsNullOrEmpty(c.TenantFeedback) ? "NA" : c.TenantFeedback,
                        SubmittedDate = c.Date.ToString("dd/MM/yyyy"),
                        ProblemTypeId = c.ProblemTypeId,
                        ProblemTypeStr = c.ProblemType == null ? "" : c.ProblemType.Name,
                        ProblemDescription = c.Description,
                        TenantId = e.PwdTenant.Id,//e.Id,
                        TenantName = e.PwdTenant.Name,
                        MobileNo = e.PwdTenant.Mobile,
                        Address = "Building: " + e.Apartment.Building.Name + ", "
                              + "Quarter: " + e.Apartment.Building.Quarter.Name + ", "
                              + e.Apartment.Building.Quarter.District.Name,//"Dhaka", //TODO should be get data from ?? entity
                        CreateDate = c.CreationTime,
                        Cost = c.Cost,
                        MaterialsUsed = c.MaterialsUsed
                    });

                //if (!string.IsNullOrEmpty(search.StartDate) && !string.IsNullOrEmpty(search.EndDate))
                //{
                //    joinData = joinData.Where(p => p.CreateDate.Value.Date >= DateTime.ParseExact(search.StartDate, "dd/MM/yyyy", provider, DateTimeStyles.None) 
                //    && p.CreateDate.Value.Date <= DateTime.ParseExact(search.EndDate, "dd/MM/yyyy", provider, DateTimeStyles.None));
                //}
                if (search.StatusId > 0)
                {
                    joinData = joinData.Where(p => p.ComplainStatusId == search.StatusId);

                }
                joinData = joinData.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
                return joinData.ToList();

            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return null;
            }
        }

        //public async Task<int> GetComplainCountBySearchAsync(ComplainSearchDto search, Guid? civilSDId)
        public async Task<int> GetComplainCountBySearchAsync(ComplainSearchDto search)
        {
            int count = 0;
            CultureInfo provider = CultureInfo.InvariantCulture;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            var allotments = await allotmentRepository.WithDetailsAsync(p => p.PwdTenant);
            try
            {
                var joinData = complaints.Where(c => c.IsDeleted == false
                                && (c.Allotment.Apartment.Building.CivilSubDivisionId == search.SubDivisionId
                                || c.Allotment.Apartment.Building.EmSubDivisionId == search.SubDivisionId)
                                || c.Allotment.Apartment.Building.QuarterId == search.QuarterId
                                || c.Allotment.Apartment.BuildingId == search.BuildingId)
                                .Join(allotments, c => c.AllotmentId, e => e.Id, (c, e) => new ComplaintListDto()
                                {
                                    Id = c.Id,
                                    Comment = c.Remark,
                                    TicketNumber = c.TicketNumber,
                                    ComplainStatusId = (int?)c.ComplainStatus,
                                    ComplainStatusStr = ((ComplainStatus)c.ComplainStatus).ToString(),
                                    SubmittedDate = c.CreationTime.ToString("dd/MM/yyyy"),
                                    ProblemTypeId = c.ProblemTypeId,
                                    ProblemTypeStr = c.ProblemType == null ? "" : c.ProblemType.Name,
                                    ProblemDescription = c.Description,
                                    TenantId = e.PwdTenant.Id,//e.Id,
                                    TenantName = e.PwdTenant.Name,
                                    MobileNo = e.PwdTenant.Mobile,
                                    Address = "Building: " + e.Apartment.Building.Name + ", "
                                            + "Quarter: " + e.Apartment.Building.Quarter.Name + ", "
                                            + e.Apartment.Building.Quarter.District.Name,//"Dhaka", //TODO should be get data from ?? entity
                                    CreateDate = c.CreationTime,
                                    Cost = c.Cost,
                                    MaterialsUsed = c.MaterialsUsed
                                });

                if (!string.IsNullOrEmpty(search.StartDate) && !string.IsNullOrEmpty(search.EndDate))
                {
                    joinData = joinData.Where(p => p.CreateDate.Value.Date >= DateTime.ParseExact(search.StartDate, "dd/MM/yyyy", provider, DateTimeStyles.None) && p.CreateDate.Value.Date <= DateTime.ParseExact(search.EndDate, "dd/MM/yyyy", provider, DateTimeStyles.None));
                }
                if (search.StatusId > 0)
                {
                    joinData = joinData.Where(p => p.ComplainStatusId == search.StatusId);

                }
                count = joinData.ToList().Count();
                return count;
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return 0;
            }
        }

        public async Task<int> GetCountAsync()
        {
            var complains = await repository.WithDetailsAsync(p => p.ComplainHistories);
            int compCount = complains.Count();
            return compCount;
        }

        public async Task<List<ComplainDto>> GetSortedListAsync(FilterModel filterModel)
        {
            var complains = await repository.WithDetailsAsync(p => p.ComplainHistories);
            complains = complains.Skip(filterModel.Offset)
                            .Take(filterModel.Limit);
            return ObjectMapper.Map<List<Complain>, List<ComplainDto>>(complains.ToList());
        }

        public async Task<List<ComplainDto>> GetComplainListStatusDateAsync(string fromDate, string toDate, int statusId)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var complains = await repository.WithDetailsAsync(p => p.ComplainHistories);

            var stDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
            var endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", provider, DateTimeStyles.None);

            var items = complains.Where(c => c.Date >= stDate && c.Date <= endDate && (int)c.ComplainStatus == statusId).ToList();

            return ObjectMapper.Map<List<Complain>, List<ComplainDto>>(complains.ToList());
        }

        public async Task<IEnumerable<ComplaintListDto>> GetDashboardComplainListAsync()
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            var allotments = await allotmentRepository.WithDetailsAsync(p => p.PwdTenant);
            try
            {
                var joinData = complaints.Where(c => c.IsDeleted == false).Join(allotments, c => c.AllotmentId, e => e.Id, (c, e) => new ComplaintListDto()
                {
                    Id = c.Id,
                    Comment = c.Remark,
                    TicketNumber = c.TicketNumber,
                    ComplainStatusId = (int?)c.ComplainStatus,
                    ComplainStatusStr = ((ComplainStatus)c.ComplainStatus).ToString(),
                    SubmittedDate = c.CreationTime.ToString("dd/MM/yyyy"),
                    ProblemTypeId = c.ProblemTypeId,
                    ProblemTypeStr = c.ProblemType == null ? "" : c.ProblemType.Name,
                    ProblemDescription = c.Description,
                    TenantId = e.Id,
                    TenantName = e.PwdTenant.Name,
                    MobileNo = e.PwdTenant.Mobile,
                    Address = "Building: " + e.Apartment.Building.Name + ", "
                              + "Quarter: " + e.Apartment.Building.Quarter.Name + ", "
                              + e.Apartment.Building.Quarter.District.Name,//"Dhaka", //TODO should be get data from ?? entity
                    CreateDate = c.CreationTime,
                    Cost = c.Cost,
                    MaterialsUsed = c.MaterialsUsed
                });

                //if (!string.IsNullOrEmpty(search.StartDate) && !string.IsNullOrEmpty(search.EndDate))
                //joinData = joinData.Where(p => p.CreateDate.Value.Date >= DateTime.ParseExact(search.StartDate, "dd/MM/yyyy", provider, DateTimeStyles.None) && p.CreateDate.Value.Date <= DateTime.ParseExact(search.EndDate, "dd/MM/yyyy", provider, DateTimeStyles.None));

                return joinData.ToList();
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return null;
            }
        }

        public async Task<int> GetNewComplainCountCurrentDateAsync(Guid? sdId)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            int cmpCnt = 0;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            try
            {
                complaints = complaints.Where(c => c.IsDeleted == false && c.OrganizationalUnitId == sdId
                                                && c.ComplainStatus == ComplainStatus.New
                                                && c.Date.Date == DateTime.Now.Date);

                if (complaints.Any())
                {
                    cmpCnt = complaints.ToList().Count;
                }
                else
                {
                    cmpCnt = 0;
                }

                return cmpCnt;
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return 0;
            }
        }

        public async Task<int> GetInProgressComplainCountCurrentDateAsync(Guid? sdId)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            int cmpCnt = 0;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            try
            {
                complaints = complaints.Where(c => c.IsDeleted == false && c.OrganizationalUnitId == sdId
                                                && c.ComplainStatus == ComplainStatus.InProgress
                                                && c.Date.Date == DateTime.Now.Date);

                if (complaints.Any())
                {
                    cmpCnt = complaints.ToList().Count;
                }
                else
                {
                    cmpCnt = 0;
                }

                return cmpCnt;
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return 0;
            }
        }

        public async Task<int> GetCompletedComplainCountCurrentDateAsync(Guid? sdId)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            int cmpCnt = 0;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            try
            {
                complaints = complaints.Where(c => c.IsDeleted == false && c.OrganizationalUnitId == sdId
                                                && c.ComplainStatus == ComplainStatus.Complete
                                                && c.Date.Date == DateTime.Now.Date);

                if (complaints.Any())
                {
                    cmpCnt = complaints.ToList().Count;
                }
                else
                {
                    cmpCnt = 0;
                }

                return cmpCnt;
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return 0;
            }
        }

        public async Task<int> GetCenceledComplainCountCurrentDateAsync(Guid? sdId)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            int cmpCnt = 0;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            try
            {
                complaints = complaints.Where(c => c.IsDeleted == false && c.OrganizationalUnitId == sdId
                                                && c.ComplainStatus == ComplainStatus.Cancel
                                                && c.Date.Date == DateTime.Now.Date);

                if (complaints.Any())
                {
                    cmpCnt = complaints.ToList().Count;
                }
                else
                {
                    cmpCnt = 0;
                }

                return cmpCnt;
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return 0;
            }
        }

        public async Task<int> GetNewComplainCountCurrentMonthAsync(Guid? sdId)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            int cmpCnt = 0;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            try
            {
                complaints = complaints.Where(c => c.IsDeleted == false && c.OrganizationalUnitId == sdId
                                                && c.ComplainStatus == ComplainStatus.New
                                                && c.Date.Month == DateTime.Now.Month);

                if (complaints.Any())
                {
                    cmpCnt = complaints.ToList().Count;
                }
                else
                {
                    cmpCnt = 0;
                }

                return cmpCnt;
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return 0;
            }
        }

        public async Task<int> GetInProgressComplainCountCurrentMonthAsync(Guid? sdId)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            int cmpCnt = 0;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            try
            {
                complaints = complaints.Where(c => c.IsDeleted == false && c.OrganizationalUnitId == sdId
                                                && c.ComplainStatus == ComplainStatus.InProgress
                                                && c.Date.Month == DateTime.Now.Month);

                if (complaints.Any())
                {
                    cmpCnt = complaints.ToList().Count;
                }
                else
                {
                    cmpCnt = 0;
                }

                return cmpCnt;
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return 0;
            }
        }

        public async Task<int> GetCompletedComplainCountCurrentMonthAsync(Guid? sdId)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            int cmpCnt = 0;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            try
            {
                complaints = complaints.Where(c => c.IsDeleted == false && c.OrganizationalUnitId == sdId
                                                && c.ComplainStatus == ComplainStatus.Complete
                                                && c.Date.Month == DateTime.Now.Month);

                if (complaints.Any())
                {
                    cmpCnt = complaints.ToList().Count;
                }
                else
                {
                    cmpCnt = 0;
                }

                return cmpCnt;
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return 0;
            }
        }

        public async Task<int> GetCenceledComplainCountCurrentMonthAsync(Guid? sdId)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            int cmpCnt = 0;
            var complaints = await repository.WithDetailsAsync(p => p.Allotment);
            try
            {
                complaints = complaints.Where(c => c.IsDeleted == false && c.OrganizationalUnitId == sdId
                                                && c.ComplainStatus == ComplainStatus.Cancel
                                                && c.Date.Month == DateTime.Now.Month);

                if (complaints.Any())
                {
                    cmpCnt = complaints.ToList().Count;
                }
                else
                {
                    cmpCnt = 0;
                }

                return cmpCnt;
            }
            catch (Exception e)
            {
                var tt = e.Message.ToList();
                return 0;
            }
        }
        //public async Task<List<ComplainDto>> GetAllComplaintsAsync()
        //{
        //    var complains = await repository.WithDetailsAsync(p => p.ComplainHistories);
        //    complains = complains.Where(c => c.IsDeleted == false
        //                                    && c.ComplainStatus == ComplainStatus.New);
        //    return ObjectMapper.Map<List<Complain>, List<ComplainDto>>(complains.ToList());
        //}
        public async void Example2()
        {
            DateTime day = (DateTime.Now).Date;
            var sdePhone = "";
            var complaints = await repository.WithDetailsAsync(p => p.ComplainHistories);
            complaints = complaints.Where(c => c.IsDeleted == false
                                            && c.ComplainStatus == ComplainStatus.New);
            if (complaints.Count() > 0)
            {
                foreach (var c in complaints)
                {
                    var dc = (day - c.Date.Date).TotalDays;
                    if (dc >= 3)
                    {
                        sdePhone = organizaitonUnitService.GetPostingById(c.PostingId).Result.EmpPhoneMobile;
                        if (!string.IsNullOrEmpty(sdePhone))
                        {
                            SmsRequestInput otpInput2 = new SmsRequestInput();
                            otpInput2.Sms = String.Format("adfadfadf");
                            otpInput2.CsmsId = sdePhone;
                            otpInput2.Msisdn = "";

                            var res2 = await notificationAppService.SendSmsTestAlpha(otpInput2);
                        }

                    }
                }
            }

            //return complaints.ToList().Count;
        }

        //        //Console.WriteLine("Thread1 Started");

        //        System.IO.File.AppendAllText(@"log.txt", "#################################################Thread1 Executing\n");
        //                Console.WriteLine("#################################################Thread1 Executing");
        //                //Thread.Sleep(500); //Sleep is used to pause a thread and 5000 is MilliSeconds that means 5 Seconds    
        //            }

        //}

        private static string GenerateTransactionId(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //private static void Example1()
        //{
        //    for (int i = 0; i <= 99999; i++)
        //    {
        //        SetUpTimer(new TimeSpan(15, 04, 01));
        //    }
        //    Thread.Sleep(1000);
        //}
        //private static void SetUpTimer(TimeSpan alertTime)
        //{
        //    HttpClient client = new HttpClient();
        //    //string url = "https://localhost:44373/api​/app​/complain​/example2";
        //    DateTime current = DateTime.Now;
        //    TimeSpan timeToGo = alertTime - current.TimeOfDay;
        //    if (timeToGo < TimeSpan.Zero)
        //    {
        //        return;//time already passed
        //    }
        //    timer = new Timer(async x =>
        //    {
        //        ////////////////////////////
        //        // //var httpResponse = await client.PostAsync(url, null);
        //        ////////////////////////
        //        ///

        //        using (var client = new HttpClient())
        //        {
        //            //var tokenResponse = await GetToken();
        //            //client.BaseAddress = new Uri(clientUrl);
        //            //client.SetBearerToken(tokenResponse.AccessToken);
        //            //client.DefaultRequestHeaders.Accept.Clear();
        //            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            ////GET Method
        //            //HttpResponseMessage response = await client.PostAsync($"api​/app​/complain​/example2", null);
        //            ////HttpResponseMessage response = await client.PostAsync($"api​/common/example", null);
        //            //if (response.IsSuccessStatusCode)
        //            //{
        //            //    try
        //            //    {
        //            //        // put the code here that may raise exceptions
        //            //    }
        //            //    catch (Exception ex)
        //            //    {
        //            //        Console.WriteLine(ex.Message);
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    Console.WriteLine("Internal server Error");
        //            //}
        //        }

        //    }, null, timeToGo, Timeout.InfiniteTimeSpan);
        //}
    }
}
