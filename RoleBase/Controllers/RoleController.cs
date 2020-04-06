using LoginServerBO.Service;
using LoginVO.VO;
using RoleBase.ActionFilters;
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
        RoleService roleService = new RoleService();
        // GET: Role

        /// <summary>
        /// Role管理畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult RoleManagement()
        {
            return View();
        }

        /// <summary>
        /// Role新增、修改、刪除畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult RoleAddEditDelete()
        {
            var roleData = roleService.GetRoleData();
            return View(roleData);
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
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                roleVO.Message = "請填寫必填欄位";
            }
            else
            {
                var result = roleService.AddRole(roleVO);

                if (!string.IsNullOrEmpty(result))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    roleVO.Message = result;
                }
                else
                    Response.StatusCode = (int)HttpStatusCode.OK;
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
            var result = roleService.DeleteRole(id);

            if (!string.IsNullOrEmpty(result))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                Response.StatusCode = (int)HttpStatusCode.OK;
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
            var result = roleService.EditRole(roleVO);

            if (!string.IsNullOrEmpty(result))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 轉到編輯角色使用者關聯的畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult RoleUserEdit()
        {
            var roleData = roleService.GetRoleData();
            return View(roleData);
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
            var userCheckData = roleService.GetUserCheckByRole(id);
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
                //處理有關選時的行為
                result = roleService.SaveRoleUserSetting(userCheckVO);

                if (!string.IsNullOrEmpty(result))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                    Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                //處理清空所有check時的行為
                result = roleService.ClearRoleUserByRoleID(roleID);

                if (!string.IsNullOrEmpty(result))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                    Response.StatusCode = (int)HttpStatusCode.OK;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}