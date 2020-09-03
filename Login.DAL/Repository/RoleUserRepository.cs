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
    public class RoleUserRepository : IRoleUserRepository
    {
        #region 屬性

        private IDataAccess _dataAccess = null;

        #endregion

        #region 建構子

        public RoleUserRepository()
        {
            UnityContainer.Register<IDataAccess, DataAccess>();
            _dataAccess = UnityContainer.Resolve<IDataAccess, DataAccess>("AccountConn");
        }

        public RoleUserRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得角色設定使用者的資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<UserCheckDTO> GetUserCheckByRole(string id, PageDataVO pageDataVO)
        {
            List<string> param = new List<string>();

            string sqlStr = string.Format(@"SELECT 
case when A.[RoleID] IS NULL then CAST(0 AS BIT) Else CAST(1 AS BIT) end AS 'Check' ,
      B.[UserID],B.AccountName,B.UserName
  FROM 
  (Select * From [RoleUser] where RoleID=@p0) A 
  Right join [User] B on A.UserID = B.UserID 
  Order By B.[{0}] {1}", pageDataVO.OrderByColumn, pageDataVO.OrderByType);

            param.Add(id);

            return _dataAccess.QueryDataTable<UserCheckDTO>(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 透過角色ID清空RoleUer的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteRoleUserByRoleID(string roleID, ref SqlConnection conn, ref SqlTransaction tran)
        {
            try
            {
                List<string> param = new List<string>();

                string sqlStr = @"Delete [RoleUser] where RoleID = @p0 ";

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
        /// 透過角色ID新增RoleUser的資料
        /// </summary>
        /// <param name="roleUserDTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int InsertRoleUser(RoleUserDTO roleUserDTO, ref SqlConnection conn, ref SqlTransaction tran)
        {
            try
            {
                List<string> param1 = new List<string>();

                string sqlStr1 = @"Insert Into [dbo].[RoleUser] (RoleID,UserID) Values(@p0,@p1)";

                param1.Add(roleUserDTO.RoleID.ToString());
                param1.Add(roleUserDTO.UserID.ToString());

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
