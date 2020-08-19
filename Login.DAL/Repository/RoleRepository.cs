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
    public class RoleRepository : IRoleRepository
    {
        #region 屬性

        private IDataAccess _dataAccess = null;

        #endregion

        #region 建構子

        public RoleRepository()
        {
            UnityContainer.Register<IDataAccess, DataAccess>();
            _dataAccess = UnityContainer.Resolve<IDataAccess>("AccountConn");
        }

        public RoleRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
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

        /// <summary>
        /// 取得Role資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRoleData(PageDataVO pageDataVO)
        {
            List<string> param = new List<string>();

            string condition = string.Empty;

            if (pageDataVO.WhereCondition != null)
            {
                for (int i = 0; i < pageDataVO.WhereCondition.Count; i++)
                {
                    if (i != pageDataVO.WhereCondition.Count - 1)
                        condition = condition + pageDataVO.WhereCondition[i].Key + " like @p" + i.ToString() + " And ";
                    else
                        condition = condition + pageDataVO.WhereCondition[i].Key + " like @p" + i.ToString() + " ";
                    param.Add("%" + pageDataVO.WhereCondition[i].Value + "%");
                }
            }
            else
                condition = "1=1";

            string sqlStr = string.Format(@"Select [RoleID],[RoleName],[Description] From  
                             (Select ROW_NUMBER() OVER(ORDER BY RoleID ) AS row, * from [Role] where {0} ) as tb1
                              where row > @p{1}  and row < @p{2} ", condition, param.Count, param.Count + 1);

            param.Add(pageDataVO.LowerBound.ToString());
            param.Add(pageDataVO.UpperBound.ToString());

            return _dataAccess.QueryDataTable<RoleDTO>(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 取得資料總筆數
        /// 只有where條件會影響總筆數
        /// </summary>
        /// <returns></returns>
        public int GetRoleCount(PageDataVO pageDataVO)
        {
            List<string> param = new List<string>();

            string condition = string.Empty;

            if (pageDataVO.WhereCondition != null)
            {
                for (int i = 0; i < pageDataVO.WhereCondition.Count; i++)
                {
                    if (i != pageDataVO.WhereCondition.Count - 1)
                        condition = condition + pageDataVO.WhereCondition[i].Key + " like @p" + i.ToString() + " And ";
                    else
                        condition = condition + pageDataVO.WhereCondition[i].Key + " like @p" + i.ToString() + " ";
                    param.Add("%" + pageDataVO.WhereCondition[i].Value + "%");
                }
            }
            else
                condition = "1=1";

            string sqlStr = string.Format(@"Select count(*) From [Role] where {0}", condition);

            return (int)_dataAccess.ExecuteScalar(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public int AddRole(RoleVO roleVO)
        {
            List<string> param = new List<string>();
            string sqlStr = @"Insert Into [Role] (RoleName,Description) 
                              Values(@p0,@p1) ";

            param.Add(roleVO.RoleName);
            param.Add(roleVO.Description);

            return _dataAccess.ExcuteSQL(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteRole(string id, ref SqlConnection conn, ref SqlTransaction tran)
        {
            List<string> param = new List<string>();
            string sqlStr = @"Delete [Role]  Where RoleID = @p0 ";

            param.Add(id);

            return _dataAccess.ExcuteSQL(sqlStr, ref conn, ref tran, param.ToArray());
        }

        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public int EditRole(RoleVO roleVO)
        {
            List<string> param = new List<string>();
            string sqlStr = @"Update [Role]  
                            Set RoleName = @p0 , Description = @p1
                            Where RoleID = @p2 ";

            param.Add(roleVO.RoleName);
            param.Add(roleVO.Description);
            param.Add(roleVO.RoleID.ToString());

            return _dataAccess.ExcuteSQL(sqlStr, param.ToArray());
        }

        #endregion
    }
}
