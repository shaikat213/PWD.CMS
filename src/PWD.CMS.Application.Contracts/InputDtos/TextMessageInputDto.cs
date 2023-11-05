using System;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{

    public class TextMessageInputDto
    {
        public int ApartmentId { get; set; }
        public int ComplainId { get; set; }
        public string ToNumber { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
