using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoginDAL.EfRepository.Interface
{
	public interface IRepository<T>
	{
		void Insert(T entity);
		void InsertRange(IEnumerable<T> entities);
		void Update(T entity);
		void Delete(T entity);
		IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
		IQueryable<T> GetAll();
		IQueryable<T> GetAvailable();
		T Get(Expression<Func<T, bool>> predicate);
		T Find(params object[] keys);
		T Find(Expression<Func<T, bool>> predicate);
		void DeleteByKey(params object[] keys);
		void DeleteItemsByKeys(object[] keys);
		void BulkInsert(IEnumerable<T> entities, string createUser, SqlBulkCopyOptions sqlBulkCopyOptions = SqlBulkCopyOptions.Default, int? batchSize = null);
		void ExecuteSqlCommand(string sql, params object[] parameters);
		int SaveChanges();
		void SetDbAutoDetectChangesEnabled(bool value);
	}
}
