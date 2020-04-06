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
    public class LoginBO
    {
        private DataAccess _dataAccess = null;

        public LoginBO()
        {
            DataAccessIO.Register<IDataAccess, DataAccess>();
            _dataAccess = (DataAccess)DataAccessIO.Resolve<IDataAccess>("AccountConn");
        }

        /// <summary>
        /// 查找帳號
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
        /// 取得該帳號資料
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public UserDTO FindAccountData(string accountName)
        {
            List<string> param = new List<string>();

            string sqlStr = "Select * From [User] Where AccountName = @p0";

            param.Add(accountName);

            return _dataAccess.QueryDataTable<UserDTO>(sqlStr, param.ToArray()).FirstOrDefault();
        }

        /// <summary>
        /// 透過帳號查找角色
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRoleDataByAccountName(string userID)
        {
            List<string> param = new List<string>();

            string sqlStr = @"Select R.RoleID,R.RoleName,R.Description 
                                From[User] U
                                join[RoleUser] RU on  U.UserID = RU.UserID
                                join[Role] R on RU.RoleID = R.RoleID
                                where U.UserID = @p0";

            param.Add(userID);

            return _dataAccess.QueryDataTable<RoleDTO>(sqlStr, param.ToArray());
        }
    }
}
