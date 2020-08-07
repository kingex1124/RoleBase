using LoginDTO.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.EfRepository
{
    public class RoleBaseEfContext<TEntity> : EfRepository<TEntity, RoleBaseEntities> where TEntity : class
    {
        public RoleBaseEfContext(RoleBaseEntities db) : base(db)
        {
            _db = db;
        }
    }
}
