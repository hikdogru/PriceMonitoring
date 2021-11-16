using PriceMonitoring.Core.Data;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Abstract
{
    public interface IUserRepository : IEntityRepository<User>
    {
    }
}
