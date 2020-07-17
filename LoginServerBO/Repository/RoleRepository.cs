using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.Interface;
using KevanFramework.DataAccessDAL.SQLDAL;
using LoginDTO.DTO;
using LoginServerBO.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Repository
{
    public class RoleRepository : IRoleRepository
    {
        #region 屬性

        private DataAccess _dataAccess = null;

        #endregion

        #region 建構子

        public RoleRepository()
        {
            DataAccessIO.Register<IDataAccess, DataAccess>();
            _dataAccess = (DataAccess)DataAccessIO.Resolve<IDataAccess>("AccountConn");
        }

        #endregion

        #region 方法

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

        #endregion
    }
}
