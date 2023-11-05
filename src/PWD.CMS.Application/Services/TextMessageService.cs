using PWD.CMS.DtoModels;
using PWD.CMS.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PWD.CMS.Services
{
    public class TextMessageService : CrudAppService<TextMessage, TextMessageDto, int>
    {
        public TextMessageService(IRepository<TextMessage, int> repository) : base(repository)
        {

        }
    }
}
