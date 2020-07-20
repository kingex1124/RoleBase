using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Helper
{
    public interface ISqlConnectionHelper
    {
        SqlConnection GetSQLConnection();
        SqlTransaction GetSqlTransaction(SqlConnection conn);
        void CommitTrans(SqlTransaction trans);
        void Rollback(SqlTransaction trans);
    }
}
