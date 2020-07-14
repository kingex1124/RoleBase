using AutoMapper;
using KevanFramework.DataAccessDAL.Common;
using LoginDTO.DTO;
using LoginServerBO.BO;
using LoginServerBO.BO.Interface;
using LoginServerBO.Helper;
using LoginServerBO.Service.Interface;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Service
{
    public class RoleService : IRoleService
    {
        #region 屬性

        IRoleBO _roleBO;
        IFunctionBO _functionBO;

        #endregion

        #region 建構子

        public RoleService()
        {
            _roleBO = new RoleBO();
            _functionBO = new FunctionBO();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得Role資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleVO> GetRoleData()
        {
            IEnumerable<RoleVO> result = Utility.MigrationIEnumerable<RoleDTO, RoleVO>(_roleBO.GetRoleData());

            return result;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public string AddRole(RoleVO roleVO)
        {
            int result = _roleBO.AddRole(roleVO);

            if (result > 0)
                return "";
            else
                return "新增失敗。";
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteRole(string id)
        {
            string result = string.Empty;
            SqlConnection conn = new SqlConnection(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);
            conn.Open();

            SqlTransaction tran = conn.BeginTransaction();
              
            int deleteRoleUserResult = _roleBO.DeleteRoleUserByRoleID(id, ref conn, ref tran);

            int deleteRoleFunctionResult = _functionBO.DeleteRoleFunctionByRoleID(id, ref conn, ref tran);

            int deleteRoleResult = _roleBO.DeleteRole(id, ref conn, ref tran);

            if (deleteRoleUserResult >= 0 && deleteRoleFunctionResult >= 0 && deleteRoleResult > 0)
                result = "";
            else
                result = "刪除失敗。";

            tran.Commit();

            return result;
        }

        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public string EditRole(RoleVO roleVO)
        {
            int result = _roleBO.EditRole(roleVO);
            if (result > 0)
                return "";
            else
                return "編輯失敗。";
        }

        /// <summary>
        /// 取得角色設定使用者的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IEnumerable<UserCheckVO> GetUserCheckByRole(string roleID)
        {
            IEnumerable<UserCheckVO> result = Utility.MigrationIEnumerable<UserCheckDTO, UserCheckVO>(_roleBO.GetUserCheckByRole(roleID));
            return result;
        }

        /// <summary>
        /// 角色編輯使用者
        /// 儲存勾選使用者時的變更
        /// </summary>
        /// <param name="userCheckVO"></param>
        /// <returns></returns>
        public string SaveRoleUserSetting(IEnumerable<UserCheckVO> userCheckVO)
        {
            string result = string.Empty;
            string roleID;
            if (userCheckVO != null && userCheckVO.Any())
            {
                roleID = userCheckVO.First().RoleID.ToString();
                List<RoleUserDTO> roleUserDTOs = new List<RoleUserDTO>();
                foreach (var item in userCheckVO)
                {
                    RoleUserDTO roleUserDTO = new RoleUserDTO();
                    roleUserDTO.RoleID = item.RoleID;
                    roleUserDTO.UserID = item.UserID;
                    roleUserDTOs.Add(roleUserDTO);
                }
              
                SqlConnection conn = new SqlConnection(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);
                conn.Open();

                SqlTransaction tran = conn.BeginTransaction();
                int deleteResult = _roleBO.DeleteRoleUserByRoleID(roleID, ref conn, ref tran);

                if (deleteResult < 0)
                {
                    tran.Rollback();
                    result = "刪除失敗。";
                    return result;
                }

                int insertResult = 0;
                foreach (var item in roleUserDTOs)
                    insertResult += _roleBO.InsertRoleUser(item, ref conn, ref tran);

                tran.Commit();
                if (insertResult < 0)
                {
                    tran.Rollback();
                    result = "設定失敗。";
                }
            }

            return result;
        }

        /// <summary>
        /// 角色編輯使用者
        /// 儲存清空使用者時的變更
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public string ClearRoleUserByRoleID(string roleID)
        {
            string result = string.Empty;
            SqlConnection conn = new SqlConnection(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);
            conn.Open();

            SqlTransaction tran = conn.BeginTransaction();
            int deleteResult = _roleBO.DeleteRoleUserByRoleID(roleID, ref conn, ref tran);

           
            if (deleteResult < 0)
            {
                tran.Rollback();
                result = "刪除失敗。";
            }

            tran.Commit();

            return result; 
        }

        #endregion
    }
}
