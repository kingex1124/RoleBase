using LoginDTO.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Repository
{
    public class RoleBaseEfRepository<TEntity> : EfRepository<TEntity, RoleBaseEntities> where TEntity : class
    {
        public RoleBaseEfRepository(RoleBaseEntities db) : base(db)
        {
            _db = db;
        }
    }
}
