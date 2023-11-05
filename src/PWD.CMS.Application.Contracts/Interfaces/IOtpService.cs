using PWD.CMS.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PWD.CMS.Interfaces
{
    public interface IOtpService : IApplicationService
    {
        Task<bool> ApplyOtp(string clientKey, string mobileNo);
        Task<bool> VarifyOtpAsync(int otp);   //string mobile,      
        Task<OtpDto> UpdateAsync(OtpDto input);
    }
}
