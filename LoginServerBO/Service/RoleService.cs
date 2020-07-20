using LoginServerBO.BO;
using LoginServerBO.BO.Interface;
using LoginServerBO.Service.Interface;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Service
{
    public class RoleService : IRoleService
    {
        #region 屬性

        IRoleBO _roleBO;

        #endregion

        #region 建構子

        public RoleService()
        {
            _roleBO = new RoleBO();
        }

        public RoleService(IRoleBO roleBO)
        {
            _roleBO = roleBO;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得Role資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleVO> GetRoleData()
        {   
            return _roleBO.GetRoleData();
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public string AddRole(RoleVO roleVO)
        {
            return _roleBO.AddRole(roleVO);
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteRole(string id)
        {
            return _roleBO.DeleteRole(id);
        }

        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public string EditRole(RoleVO roleVO)
        {
            return _roleBO.EditRole(roleVO);
        }

        /// <summary>
        /// 取得角色設定使用者的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IEnumerable<UserCheckVO> GetUserCheckByRole(string roleID)
        {
            return _roleBO.GetUserCheckByRole(roleID);
        }

        /// <summary>
        /// 角色編輯使用者
        /// 儲存勾選使用者時的變更
        /// </summary>
        /// <param name="userCheckVO"></param>
        /// <returns></returns>
        public string SaveRoleUserSetting(IEnumerable<UserCheckVO> userCheckVO)
        {
            return _roleBO.SaveRoleUserSetting(userCheckVO);
        }

        /// <summary>
        /// 角色編輯使用者
        /// 儲存清空使用者時的變更
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public string ClearRoleUserByRoleID(string roleID)
        {
            return _roleBO.ClearRoleUserByRoleID(roleID);
        }

        #endregion
    }
}
