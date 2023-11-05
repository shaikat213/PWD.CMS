using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PWD.CMS.Common;
using PWD.CMS.DtoModels;
using PWD.CMS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.PermissionManagement;

namespace PWD.CMS.Services
{
    public class NotificationAppService : CMSAppService, INotificationAppService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        public NotificationAppService(ILogger<PermissionAppService> logger)
        {
            _logger = logger;
            _httpClient = CreateClient();
        }

        //public async Task<SmsResponse> SendSmsNotification(SmsRequestInput input)
        //{
        //    HttpClient client = new HttpClient();
        //    var requestInput = new Dictionary<string, string>
        //    {
        //        { "api_token", "pnij27vt-wixyw8qu-a4rwdexc-kqvpgude-tt32mdiv" },
        //        { "sid", "PWDNONAPI" }
        //    };

        //    requestInput.Add("msisdn", input.Msisdn);
        //    requestInput.Add("sms", input.Sms);
        //    requestInput.Add("csms_id", input.CsmsId);

        //    string url = "https://smsplus.sslwireless.com/api/v3/send-sms";
        //    var data = new FormUrlEncodedContent(requestInput);
        //    var httpResponse = await client.PostAsync(url, data);
        //    if (httpResponse.Content != null)
        //    {
        //        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        //        dynamic response = JObject.Parse(responseContent);
        //        return new SmsResponse
        //        {
        //            status = response.status,
        //            status_code = response.status_code,
        //            error_message = response.error_message,
        //            smsinfo = response.smsinfo?.ToObject<List<SmsInfo>>()
        //            //JsonConvert.DeserializeObject<List<SmsInfo>>(response.smsinfo)
        //            //response.smsinfo?.ToObject<string[]>()
        //        };
        //    }
        //    return new SmsResponse();
        //}

        //[DontWrapResult]
        public async Task<SmsResponse> SendSmsNotification(SmsRequestInput input)
        {
            HttpClient client = new HttpClient();
            var requestInput = new Dictionary<string, string>
            {
                { "api_token", "pnij27vt-wixyw8qu-a4rwdexc-kqvpgude-tt32mdiv" },
                { "sid", "PWDNONAPI" }
            };

            requestInput.Add("msisdn", input.Msisdn);
            requestInput.Add("sms", input.Sms);
            requestInput.Add("csms_id", input.CsmsId);

            string url = "https://smsplus.sslwireless.com/api/v3/send-sms";
            var data = new FormUrlEncodedContent(requestInput);
            var httpResponse = await client.PostAsync(url, data);
            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                dynamic response = JObject.Parse(responseContent);
                return new SmsResponse
                {
                    status = response.status,
                    status_code = response.status_code,
                    error_message = response.error_message,
                    smsinfo = response.smsinfo?.ToObject<List<SmsInfo>>()
                    //JsonConvert.DeserializeObject<List<SmsInfo>>(response.smsinfo)
                    //response.smsinfo?.ToObject<string[]>()
                };
            }
            return new SmsResponse();
        }

        public async Task<SmsResponse> SendSmsTestAlpha(SmsRequestInput input)
        {
            HttpClient client = new HttpClient();
            var requestInput = new Dictionary<string, string>
            {
                //{ "api_key_1", "u2AE4IjCC87Z7vDfMnSO8r5b147V4fQYD9055hkP" }
                { "api_key", "g694IRVD6e72jw2bBT1NKqgJfn6Af03Y68YtXMIe" }
            };

            requestInput.Add("msg", input.Sms);
            requestInput.Add("to", input.CsmsId);
            //requestInput.Add("schedule", DateTime.Now.ToString());

            string url = "https://api.sms.net.bd/sendsms";
            var data = new FormUrlEncodedContent(requestInput);
            var httpResponse = await client.PostAsync(url, data);
            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                dynamic response = JObject.Parse(responseContent);
                return new SmsResponse
                {
                    status = response.data.request_status,
                    //status_code = response.status_code,
                    error_message = response.error,
                    //smsinfo = response.smsinfo?.ToObject<List<SmsInfo>>()
                    //JsonConvert.DeserializeObject<List<SmsInfo>>(response.smsinfo)
                    //response.smsinfo?.ToObject<string[]>()
                };
            }
            return new SmsResponse();
        }

        public async Task CreateAsync(NotificationDto input)
        {
            var response = await _httpClient.PostAsJsonAsync(CmsCommonConsts.NotificationUrl, input);
            if (response != null && response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Notification pushed successfuly for user : {input.CreatedBy} ");
            }
            else
            {
                _logger.LogError($"Notification pushed failed for user : {input.CreatedBy} ");
            }
        }

        private static HttpClient CreateClient()
        {
            Uri baseAddress = null;
#if DEBUG
            baseAddress = new Uri("https://localhost:44379/");
#else
            baseAddress = new Uri("https://erpapi.mis1pwd.com/");; 
#endif
            var client = new HttpClient
            {
                BaseAddress = baseAddress
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public async Task<IEnumerable<NotificationDto>> GetListAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<NotificationDto>>(CmsCommonConsts.NotificationUrl);
            return response;
        }

        public async Task<IEnumerable<NotificationDto>> GetListByClientIdAsync(string clientId, int maxRecord = 10)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<NotificationDto>>(CmsCommonConsts.NotificationUrl + $"{clientId}/{maxRecord}");
            return response;
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync(CmsCommonConsts.NotificationUrl + $"{id}");
            if (response != null && response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Notification deleted successfuly for id : {id} ");
            }
            else
            {
                _logger.LogError($"Notification delete failed for id : {id} ");
            }
        }
        public NotificationDto ConvertToNotification(NotificaitonCommonDto input)
        {
            var noficationInputDto = new NotificationDto
            {
                Id = GuidGenerator.Create(),
                ClientId = CmsCommonConsts.ClientId,
                Message = input.Message,
                CreatedBy = input.CreatedBy,
                CreatedFor = input.CreatedFor,
                CreatedAt = DateTime.Now,
                CreatorName = string.IsNullOrEmpty(input.CreatorName) ? "" : input.CreatorName,
                ReceiverName = string.IsNullOrEmpty(input.ReceiverName) ? "" : input.ReceiverName,
                Source = input.Source,
                Destination = input.Destination,
                Priority = input.Priority,
                Status = input.Status,
                ResourceUrl = CmsCommonConsts.ResourceUrl,
                Unread = true
            };

            return noficationInputDto;
        }
    }
}
