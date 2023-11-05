using Volo.Abp.Application.Dtos;

namespace PWD.CMS.DtoModels
{
    public class ApartmentDto : EntityDto<int>
    {
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int Floor { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AllotmentId { get; set; }
        public string Allotment { get; set; }
    }
}
