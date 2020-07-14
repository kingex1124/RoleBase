using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.Interface;
using KevanFramework.DataAccessDAL.SQLDAL;
using LoginDTO.DTO;
using LoginServerBO.BO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO
{
    public class SecurityBO : ISecurityBO
    {
        #region 屬性

        private DataAccess _dataAccess = null;

        #endregion

        #region 建構子

        public SecurityBO()
        {
            DataAccessIO.Register<IDataAccess, DataAccess>();
            _dataAccess = (DataAccess)DataAccessIO.Resolve<IDataAccess>("AccountConn");
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得該角色ID所具備的權限功能
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId)
        {
            List<string> param = new List<string>();

            string sqlStr = @"select R.RoleName,F.Url,F.Description
                                from [dbo].[Role] R  
                                join [dbo].[RoleFunction] RF on R.RoleID=RF.RoleID 
                                join [dbo].[Function]  F on RF.FunctionID=F.FunctionID  
                                where R.RoleID= @p0";

            param.Add(roleId);

            return _dataAccess.QueryDataTable<SecurityRoleFunctionDTO>(sqlStr, param.ToArray());
        }

        #endregion
    }
}
