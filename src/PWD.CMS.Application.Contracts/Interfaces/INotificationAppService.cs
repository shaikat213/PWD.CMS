using PWD.CMS.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PWD.CMS.Interfaces
{
    public interface INotificationAppService : IApplicationService
    {
        Task<SmsResponse> SendSmsNotification(SmsRequestInput input);
        Task<SmsResponse> SendSmsTestAlpha(SmsRequestInput input);
        //start erp notification
        Task CreateAsync(NotificationDto input);
        Task<IEnumerable<NotificationDto>> GetListAsync();
        Task<IEnumerable<NotificationDto>> GetListByClientIdAsync(string clientId, int maxRecord = 10);
        Task DeleteAsync(Guid id);
        NotificationDto ConvertToNotification(NotificaitonCommonDto input);

        // end erp notification
    }
}
