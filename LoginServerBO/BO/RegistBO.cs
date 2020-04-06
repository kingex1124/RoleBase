using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.Interface;
using KevanFramework.DataAccessDAL.SQLDAL;
using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO
{
    public class RegistBO
    {
        private DataAccess _dataAccess = null;

        public RegistBO()
        {
            DataAccessIO.Register<IDataAccess, DataAccess>();
            _dataAccess = (DataAccess)DataAccessIO.Resolve<IDataAccess>("AccountConn");
        }

        /// <summary>
        /// 驗證帳號是否重複
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public IEnumerable<UserDTO> FindAccountName(string accountName)
        {
            List<string> param = new List<string>();

            string sqlStr = "Select * From [User] Where AccountName = @p0";

            param.Add(accountName);

            return _dataAccess.QueryDataTable<UserDTO>(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int UserInsert(Account account)
        {
            List<string> param = new List<string>() { 
                account.AccountName,
                account.UserName,
                account.Password,
                account.Phone,
                account.Email
            };
            string sqlStr = "Insert into [dbo].[User] (AccountName,UserName,Password,Phone,Email) Values(@p0,@p1,@p2,@p3,@p4)";
            return _dataAccess.ExcuteSQL(sqlStr, param.ToArray());
        }
    }
}
