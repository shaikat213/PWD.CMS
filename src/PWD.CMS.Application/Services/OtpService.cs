using PWD.CMS.CMSEnums;
using PWD.CMS.DtoModels;
using PWD.CMS.Interfaces;
using PWD.CMS.Models;
using PWD.CMS.Permissions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;
using System.Linq;

namespace PWD.CMS.Services
{
    public class OtpService : ApplicationService, IOtpService
    {
        private readonly IRepository<Otp, int> repository;
        private readonly INotificationAppService notificationAppService;
        private readonly IRepository<PwdTenant, int> tenantRepository; 
        private readonly IUnitOfWorkManager unitOfWorkManager;
        public OtpService(IRepository<Otp, int> repository,
            INotificationAppService notificationAppService,
            IRepository<PwdTenant, int> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            this.repository = repository;
            this.notificationAppService = notificationAppService;
            this.tenantRepository = tenantRepository;
            this.unitOfWorkManager = unitOfWorkManager;
        }


        [HttpGet]
        public async Task<bool> ApplyOtp(string clientKey, string mobileNo)
        {
            if (mobileNo != null)
            {

                if (!string.IsNullOrEmpty(clientKey) && clientKey.Equals("CMS_App", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(mobileNo))
                {
                    int otp = CmsUtility.GetRandomNo(1000, 9999);
                    Otp otpEntity = new Otp();
                    otpEntity.OtpNo = otp;
                    otpEntity.MobileNo = mobileNo;
                    otpEntity.ExpireDateTime = DateTime.Now.AddMinutes(3);
                    otpEntity.OtpStatus = OtpStatus.New;
                    await repository.InsertAsync(otpEntity);
                    // stp start
                    SmsRequestInput otpInput = new SmsRequestInput();
                    otpInput.Sms = String.Format("Dear Allotee, Your PWD OTP for complaint is " + otp + ". Please use this OTP to complete your complaint.");                    
                    otpInput.Msisdn = mobileNo;
                    otpInput.CsmsId = GenerateTransactionId(16);
                    try
                    {
                        //var res = await notificationAppService.SendSmsTestAlpha(otpInput);
                        var res = await notificationAppService.SendSmsNotification(otpInput);
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                        throw new Exception(e.Message);
                        
                    }
                    //end otp
                    //return true;
                }
            }
            return false;
        }
        private static string GenerateTransactionId(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpGet]
        public async Task<bool> VarifyOtpAsync(int otp)
        {
            if (otp > 0)
            {
                var item = await repository.FirstOrDefaultAsync(x => x.OtpNo == otp && x.OtpStatus == OtpStatus.New && x.ExpireDateTime >= DateTime.Now);
                if (item != null)
                {
                    item.OtpStatus = OtpStatus.Varified;
                    await repository.UpdateAsync(item);                    
                    await unitOfWorkManager.Current.SaveChangesAsync();
                    return true;
                }
            }

            return false;

        }

        public async Task<OtpDto> UpdateAsync(OtpDto input)
        {
            var item = await repository.FirstOrDefaultAsync(x => x.OtpNo == input.OtpNo && x.ExpireDateTime >= DateTime.Now);
            if (item != null)
            {
                item.OtpStatus = input.OtpStatus;
                await repository.UpdateAsync(item);
                return ObjectMapper.Map<Otp, OtpDto>(item);
            }
            return input;
        }


    }
}
