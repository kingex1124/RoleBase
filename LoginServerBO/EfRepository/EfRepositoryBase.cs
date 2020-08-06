using EntityFramework.BulkInsert.Extensions;
using LoginServerBO.EfRepository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.EfRepository
{
    public class EfRepositoryBase<TEntity, TDbContext> : IRepository<TEntity> where TEntity : class where TDbContext : DbContext
    {
        protected TDbContext _db;
        protected DbSet<TEntity> Set;
        private int _updateCount = 0;

        public EfRepositoryBase(TDbContext db)
        {
            _db = db;
            Set = db.Set<TEntity>();
        }

        public void Insert(TEntity entity)
        {
            Set.Add(entity);
            _updateCount++;
        }

        public void Update(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            _updateCount++;
        }
        public void Delete(TEntity entity)
        {
            Set.Remove(entity);
            _updateCount++;
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
            _updateCount += entities.Count();
        }

        public IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Where(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return Set;
        }

        public IQueryable<TEntity> GetAvailable()
        {
            var queryable = GetAll();

            if (typeof(IDeleteState).IsAssignableFrom(typeof(TEntity)))
            {
                var alternate = (IQueryable<IDeleteState>)queryable;

                queryable = alternate.Where(x => x.IsDeleted == false).Cast<TEntity>();
            }

            return queryable;
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.FirstOrDefault(predicate);
        }

        public TEntity Find(params object[] keys)
        {
            return Set.Find(keys);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.SingleOrDefault(predicate);
        }

        public int SaveChanges()
        {
            try
            {
                _db.SaveChanges();
                return _updateCount;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public void DeleteByKey(params object[] keys)
        {
            var item = Set.Find(keys);
            if (item == null) return;
            if (typeof(IDeleteState).IsAssignableFrom(typeof(TEntity)))
            {
                var alternate = (IDeleteState)item;
                alternate.IsDeleted = true;
                Update(item);
            }
            else
                Delete(item);
        }

        public void DeleteItemsByKeys(object[] keys)
        {
            foreach (var key in keys)
                DeleteByKey(key);
        }

        public void BulkInsert(IEnumerable<TEntity> entities, string createUser, SqlBulkCopyOptions sqlBulkCopyOptions = SqlBulkCopyOptions.Default, int? batchSize = null)
        {
            foreach (var entity in entities)
            {
                typeof(TEntity).GetProperty("Gid", typeof(Guid))?.SetValue(entity, Guid.NewGuid(), null);
                typeof(TEntity).GetProperty("CreatedUser", typeof(string))?.SetValue(entity, createUser, null);
                typeof(TEntity).GetProperty("CreatedDate", typeof(DateTime))?.SetValue(entity, DateTime.Now, null);
                typeof(TEntity).GetProperty("UpdatedUser", typeof(string))?.SetValue(entity, createUser, null);
                typeof(TEntity).GetProperty("UpdatedDate", typeof(DateTime))?.SetValue(entity, DateTime.Now, null);
            }
            _db.BulkInsert(entities, sqlBulkCopyOptions, batchSize);
        }

        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            _db.Database.ExecuteSqlCommand(sql, parameters);
        }

        public void SetDbContext(TDbContext db)
        {
            _db = db;
        }

        public void SetDbAutoDetectChangesEnabled(bool value)
        {
            _db.Configuration.AutoDetectChangesEnabled = value;
        }
    }
}
