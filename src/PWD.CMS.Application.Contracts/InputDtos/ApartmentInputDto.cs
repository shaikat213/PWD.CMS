namespace PWD.CMS.DtoModels
{
    public class ApartmentInputDto
    {
        public int BuildingId { get; set; }
        public int Floor { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AllotmentId { get; set; }
    }
}
