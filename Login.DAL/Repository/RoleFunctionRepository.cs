using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.SQLDAL;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using Login.DTO;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public class RoleFunctionRepository : IRoleFunctionRepository
    {
        #region 屬性

        private IDataAccess _dataAccess = null;

        #endregion

        #region 建構子

        public RoleFunctionRepository()
        {
            UnityContainer.Register<IDataAccess, DataAccess>();
            _dataAccess = UnityContainer.Resolve<IDataAccess>("AccountConn");
        }

        public RoleFunctionRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得該角色ID所具備的權限功能
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string userID)
        {
            List<string> param = new List<string>();

            string sqlStr = @"Select distinct D.Url,D.Description from (select A.RoleID from [dbo].[RoleUser] A
where A.UserID = @p0) B 
Join [dbo].[RoleFunction] C
on B.RoleID = C.RoleID 
join [dbo].[Function] D
on C.FunctionID = D.FunctionID";

            param.Add(userID);

            return _dataAccess.QueryDataTable<SecurityRoleFunctionDTO>(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 透過FunctionID刪除RoleFunction資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteRoleFunctionByFunctionID(string functionID, ref SqlConnection conn, ref SqlTransaction tran)
        {
            try
            {
                List<string> param = new List<string>();

                string sqlStr = @"Delete [RoleFunction] where functionID = @p0 ";

                param.Add(functionID);

                return _dataAccess.ExcuteSQL(sqlStr, ref conn, ref tran, param.ToArray());
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 取得角色設定功能的資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<FunctionCheckDTO> GetFunctionCheckByRole(string id, PageDataVO pageDataVO)
        {
            List<string> param = new List<string>();

            string sqlStr = string.Format(@"SELECT 
case when A.[RoleID] IS NULL then CAST(0 AS BIT) Else CAST(1 AS BIT) end AS 'Check' ,
      B.[FunctionID],B.Url,B.Title, B.Description, B.IsMenu , 
case when B.[Parent] = -1
then 'Not Menu'
when  B.[Parent] = 0 
then 'No'
else (select C.[Title] from [Function] C where C.FunctionID = B.[Parent]) end as 'ParentName'
  FROM 
  (Select * From [RoleFunction] where RoleID = @p0 ) A 
  Right join [Function] B on A.FunctionID = B.FunctionID 
  Order By B.[{0}] {1} ", pageDataVO.OrderByColumn, pageDataVO.OrderByType);

            param.Add(id);

            return _dataAccess.QueryDataTable<FunctionCheckDTO>(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 透過角色ID清空RoleFunciton的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteRoleFunctionByRoleID(string roleID, ref SqlConnection conn, ref SqlTransaction tran)
        {
            try
            {
                List<string> param = new List<string>();

                string sqlStr = @"Delete [RoleFunction] where RoleID = @p0 ";

                param.Add(roleID);

                return _dataAccess.ExcuteSQL(sqlStr, ref conn, ref tran, param.ToArray());
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 透過角色ID新增RoleFunction資料
        /// </summary>
        /// <param name="roleFunctionDTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int InsertRoleFunction(RoleFunctionDTO roleFunctionDTO, ref SqlConnection conn, ref SqlTransaction tran)
        {
            try
            {
                List<string> param1 = new List<string>();

                string sqlStr1 = @"Insert Into [dbo].[RoleFunction] (RoleID,FunctionID) Values(@p0,@p1)";

                param1.Add(roleFunctionDTO.RoleID.ToString());
                param1.Add(roleFunctionDTO.FunctionID.ToString());

                return _dataAccess.ExcuteSQL(sqlStr1, ref conn, ref tran, param1.ToArray());
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }

        #endregion
    }
}
