using PriceMonitoring.Core.Data.EntityFramework;
using PriceMonitoring.Data.Abstract;
using PriceMonitoring.Data.Concrete.EntityFramework.Contexts;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Concrete.EntityFramework
{
    public class EfCoreUserRepository : EfEntityRepositoryBase<User, PriceMonitoringContext>, IUserRepository
    {
        public EfCoreUserRepository(PriceMonitoringContext context) : base(context: context)
        {

        }
    }
}
