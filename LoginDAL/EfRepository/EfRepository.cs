using LoginDAL.EfRepository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDAL.EfRepository
{
    public class EfRepository<TEntity, TDbContext> : EfRepositoryBase<TEntity, TDbContext>, IRepository<TEntity> where TEntity : class where TDbContext : DbContext
    {
        public EfRepository(TDbContext db) : base(db)
        {

        }
    }
}
