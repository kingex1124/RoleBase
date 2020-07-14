using LoginServerBO.Service;
using LoginServerBO.Service.Interface;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RoleBase.Controllers
{
    public class RoleAnApiController : ApiController
    {
        #region 屬性

        IRoleService _roleService;

        #endregion

        #region 建構子

        public RoleAnApiController()
        {
            _roleService = new RoleService();
        }

        #endregion

        #region Action

        /// <summary>
        /// 取得角色資料
        /// </summary>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public IEnumerable<RoleVO> RoleAddEditDelete()
        {
            // Thread.Sleep(1000);
            var roleData = _roleService.GetRoleData();
            return roleData;
        }

        /// <summary>
        /// 新增Role
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public RoleVO AddRole(RoleVO roleVO)
        {
            if (!ModelState.IsValid)
                roleVO.Message = "請填寫必填欄位";
            else
            {
                var result = _roleService.AddRole(roleVO);

                if (!string.IsNullOrEmpty(result))
                    roleVO.Message = result;
            }
            return roleVO;
        }

        /// <summary>
        /// 刪除Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public string DeleteRole(RoleVO roleVO)
        {
            var result = _roleService.DeleteRole(roleVO.RoleID.ToString());

            return result;
        }

        /// <summary>
        /// 編輯Role
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public string EditRole(RoleVO roleVO)
        {
            var result = _roleService.EditRole(roleVO);
           
            return result;
        }

        /// <summary>
        /// 透過角色ID取得勾選的使用者資料
        /// 編輯角色與使用者的關係
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public IEnumerable<UserCheckVO> GetUserByRole(RoleVO roleVO)
        {
            var userCheckData = _roleService.GetUserCheckByRole(roleVO.RoleID.ToString());
            return userCheckData;
        }

        /// <summary>
        /// 儲存RoleUser設定的變更
        /// </summary>
        /// <param name="userCheckVO"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public string SaveRoleUserSetting(IEnumerable<UserCheckVO> userCheckVO)
        {
            string result = string.Empty;
            //處理有關選時的行為
            result = _roleService.SaveRoleUserSetting(userCheckVO);
            return result;
        }

        /// <summary>
        /// 儲存RoleUser設定的變更，未選取的時候
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public string NoSelectSaveRoleUserSetting(RoleVO roleVO)
        {
            string result = string.Empty;

            //處理清空所有check時的行為
            result = _roleService.ClearRoleUserByRoleID(roleVO.RoleID.ToString());

            return result;
        }

        #endregion
    }
}
