using KevanFramework.DataAccessDAL.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Helper
{
    public class SqlConnectionHelper : ISqlConnectionHelper
    {
        public SqlConnection GetSQLConnection()
        {
            var result = new SqlConnection(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);
            result.Open();
            return result;
        }

        public SqlTransaction GetSqlTransaction(SqlConnection conn)
        {
            return conn.BeginTransaction();
        }

        public void CommitTrans(SqlTransaction trans)
        {
            trans.Commit();
        }

        public void Rollback(SqlTransaction trans)
        {
            trans.Rollback();
        }
    }
}
