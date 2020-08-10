using EntityFramework.BulkInsert.Extensions;
using LoginDAL.EfRepository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoginDAL.EfRepository
{
    public class EfRepositoryBase<TEntity, TDbContext> : IRepository<TEntity> where TEntity : class where TDbContext : DbContext
    {
        #region 屬性

        protected TDbContext _db;
        protected DbSet<TEntity> _set;
        private int _updateCount = 0;

        #endregion

        #region 建構子

        public EfRepositoryBase(TDbContext db)
        {
            _db = db;
            _set = db.Set<TEntity>();
        }

        #endregion

        #region 方法

        #region 新增寫入

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(TEntity entity)
        {
            _set.Add(entity);
            _updateCount++;
        }

        /// <summary>
        /// 多筆資料寫入
        /// </summary>
        /// <param name="entities"></param>
        public void InsertRange(IEnumerable<TEntity> entities)
        {
            _set.AddRange(entities);
            _updateCount += entities.Count();
        }

        /// <summary>
        /// 大量寫入的方法
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="createUser"></param>
        /// <param name="sqlBulkCopyOptions"></param>
        /// <param name="batchSize"></param>
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

        #endregion

        #region 修改

        /// <summary>
        /// 單筆資料異動
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            _updateCount++;
        }

        #endregion

        #region 刪除

        /// <summary>
        /// 單筆資料刪除
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            _set.Remove(entity);
            _updateCount++;
        }

        /// <summary>
        /// 透過key刪除資料
        /// </summary>
        /// <param name="keys"></param>
        public void DeleteItemsByKeys(object[] keys)
        {
            foreach (var key in keys)
                DeleteByKey(key);
        }

        /// <summary>
        /// 透過key刪除資料
        /// </summary>
        /// <param name="keys"></param>
        public void DeleteByKey(params object[] keys)
        {
            var item = _set.Find(keys);
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


        #endregion

        #region 查詢

        /// <summary>
        /// 查找條件資料
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        /// <summary>
        /// 取得單筆資料
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate);
        }

        /// <summary>
        /// 取得List資料
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 取得所有資料
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return _set;
        }

        /// <summary>
        /// 透過key查找資料
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public TEntity Find(params object[] keys)
        {
            return _set.Find(keys);
        }

        /// <summary>
        /// 查找資料
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.SingleOrDefault(predicate);
        }

        #endregion

        /// <summary>
        /// 儲存異動
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 透過SQLString做新刪修
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            _db.Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// 設置DbContext
        /// </summary>
        /// <param name="db"></param>
        public void SetDbContext(TDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// EF的異動偵測DetectChanges機制
        /// DetectChanges為true時，每次異動都會做差異比對
        /// 所以大量處理資料時會先設為false,最後再改為true做一次差異比對
        /// </summary>
        /// <param name="value"></param>
        public void SetDbAutoDetectChangesEnabled(bool value)
        {
            _db.Configuration.AutoDetectChangesEnabled = value;
        }

        #endregion
    }
}
