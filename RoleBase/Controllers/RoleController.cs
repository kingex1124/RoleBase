using Login.Service;
using Login.VO;
using RoleBase.ActionFilters;
using RoleBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RoleBase.Controllers
{
    public class RoleController : Controller
    {
        #region 屬性

        IRoleService _roleService;

        private HttpContextBase _currentHttpContext;

        public HttpContextBase CurrentHttpContext
        {
            get
            {
                if (_currentHttpContext != null)
                    return _currentHttpContext;

                return HttpContextFactory.GetHttpContext();
            }
            set { _currentHttpContext = value; }
        }
        #endregion

        #region 建構子

        public RoleController()
        {
            _roleService = RouteConfig.Container.Resolve<IRoleService>();
        }

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion

        #region Action

        /// <summary>
        /// Role管理畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult RoleManagement()
        {
            return View("RoleManagement");
        }

        /// <summary>
        /// Role新增、修改、刪除畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult RoleAddEditDelete()
        {
            //var roleData = _roleService.GetRoleData();
            return View("RoleAddEditDelete");
        }

        /// <summary>
        /// 查詢腳色資料
        /// </summary>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult QueryRole(PageDataVO pageDataVO)
        {
            var roleData = _roleService.GetRoleData();
            return Json(roleData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增Role
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        [HttpPost]
        [UserSession]
        public ActionResult AddRole(RoleVO roleVO)
        {
            if (!ModelState.IsValid)
            {
                CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                roleVO.Message = "請填寫必填欄位";
            }
            else
            {
                var result = _roleService.AddRole(roleVO);

                if (!string.IsNullOrEmpty(result))
                {
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    roleVO.Message = result;
                }
                else
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            return Json(roleVO, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 刪除Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult DeleteRole(string id)
        {
            var result = _roleService.DeleteRole(id);

            if (!string.IsNullOrEmpty(result))
            {
                CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 編輯Role
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        [HttpPost]
        [UserSession]
        public ActionResult EditRole(RoleVO roleVO)
        {
            var result = _roleService.EditRole(roleVO);

            if (!string.IsNullOrEmpty(result))
            {
                CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 轉到編輯角色使用者關聯的畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult RoleUserEdit()
        {
            //var roleData = _roleService.GetRoleData();
            return View("RoleUserEdit");
        }

        /// <summary>
        /// 查詢角色資料
        /// </summary>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult QueryRoleUserEditRole()
        {
            var roleData = _roleService.GetRoleData();
            return Json(roleData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 透過角色ID取得勾選的使用者資料
        /// 編輯角色與使用者的關係
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult GetUserByRole(string id)
        {
            var userCheckData = _roleService.GetUserCheckByRole(id);
            return Json(userCheckData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 儲存RoleUser設定的變更
        /// </summary>
        /// <param name="userCheckVO"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult SaveRoleUserSetting(IEnumerable<UserCheckVO> userCheckVO, string roleID = null)
        {
            string result = string.Empty;

            if (roleID == null)
            {
                //處理有關聯時的行為
                result = _roleService.SaveRoleUserSetting(userCheckVO);

                if (!string.IsNullOrEmpty(result))
                {
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                //處理清空所有check時的行為
                result = _roleService.ClearRoleUserByRoleID(roleID);

                if (!string.IsNullOrEmpty(result))
                {
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}