using PWD.CMS.CMSEnums;
using PWD.CMS.Models;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PWD.CMS.Services
{
    public class NotificationService : ApplicationService
    {
        private readonly IRepository<Complain, int> repository;

        public NotificationService(IRepository<Complain, int> repository) 
        {
            this.repository = repository;
        }

        public async Task Notify()
        {
           var complains=await  repository.GetListAsync();

            var newComplains = complains.Where(c => c.ComplainStatus == ComplainStatus.New).ToList();

            var divisionList=newComplains
                .Where(c => !c.NotifyDivision  && 
                c.Date <= System.DateTime.Today.AddDays(-3)).ToList();

            divisionList.ForEach(d =>
            {
                //notify Division
                d.NotifyDivision = true;
                d.NotifyDivisionDate = System.DateTime.Today;
            });

            var circleList = newComplains
                .Where(c => !c.NotifyCircle   && 
                c.Date <= System.DateTime.Today.AddDays(-6)).ToList();

            circleList.ForEach(d =>
            {
                //notify Circle
                d.NotifyCircle = true;
                d.NotifyCircleDate = System.DateTime.Today;
            });

            var zoneList = newComplains
                .Where(c => !c.NotifyZone  &&
                c.Date <= System.DateTime.Today.AddDays(-9)).ToList();
            
            zoneList.ForEach(d =>
            {
                //notify Zone
                d.NotifyZone = true;
                d.NotifyZoneDate= System.DateTime.Today;
            });


        }

    }
}
