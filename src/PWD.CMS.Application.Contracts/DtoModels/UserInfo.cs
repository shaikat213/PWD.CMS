using System;

namespace PWD.CMS.DtoModels
{
    public class UserInfo
    {
        public object tenantId { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
        public object surname { get; set; }
        public string email { get; set; }
        public bool emailConfirmed { get; set; }
        public object phoneNumber { get; set; }
        public bool phoneNumberConfirmed { get; set; }
       
        public Guid id { get; set; }
        public Extraproperties extraProperties { get; set; }
    }
    public class Extraproperties
    {
    }
}
