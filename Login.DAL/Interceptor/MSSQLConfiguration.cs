using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public class MSSQLConfiguration : DbConfiguration
    {
        public MSSQLConfiguration()
        {
            DbInterception.Add(new MSSQLInterceptorLogging());
        }
    }
}
