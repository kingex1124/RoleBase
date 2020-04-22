using LoginServerBO.Service;
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
        RoleService roleService = new RoleService();

        /// <summary>
        /// 取得角色資料
        /// </summary>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public IEnumerable<RoleVO> RoleAddEditDelete()
        {
            // Thread.Sleep(1000);
            var roleData = roleService.GetRoleData();
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
                var result = roleService.AddRole(roleVO);

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
            var result = roleService.DeleteRole(roleVO.RoleID.ToString());

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
            var result = roleService.EditRole(roleVO);
           
            return result;
        }
    }
}
