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
    public class FunctionController : Controller
    {
        FunctionService functionService = new FunctionService();
        RoleService roleService = new RoleService();

        // GET: Function
        /// <summary>
        /// Function管理介面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult FunctionManagement()
        {
            return View();
        }

        /// <summary>
        /// Function新增、修改、刪除畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult FunctionAddEditDelete()
        {
            var functionData = functionService.GetFunctionData();
            return View(functionData);
        }

        /// <summary>
        /// 新增Function
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult AddFunction(FunctionVO functionVO)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                functionVO.Message = "請填寫必填欄位";
            }
            else
            {
                var result = functionService.AddFunction(functionVO);

                if (!string.IsNullOrEmpty(result))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    functionVO.Message = result;
                }
                else
                    Response.StatusCode = (int)HttpStatusCode.OK;
            }
            return Json(functionVO, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 刪除Function
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult DeleteFunction(string id)
        {
            var result = functionService.DeleteFunction(id);

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
        /// 編輯Function
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult EditFunction(FunctionVO functionVO)
        {
            var result = functionService.EditFunction(functionVO);

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
        /// 轉到編輯角色功能關聯的畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult RoleFunctionEdit()
        {
            var roleData = roleService.GetRoleData();
            return View(roleData);
        }

        /// <summary>
        /// 透過角色ID取得勾選的功能資料
        /// 編輯角色與功能的關係
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult GetFunctionByRole(string id)
        {
            var functionCheckData = functionService.GetFunctionCheckByRole(id);
            return Json(functionCheckData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 儲存RoleFunction設定的變更
        /// </summary>
        /// <param name="functionCheckVO"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult SaveRoleFunctionSetting(IEnumerable<FunctionCheckVO> functionCheckVO, string roleID = null)
        {
            string result = string.Empty;

            if (roleID == null)
            {
                //處理有關選時的行為
                result = functionService.SaveRoleFunctionSetting(functionCheckVO);

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
                result = functionService.ClearRoleFunctionByRoleID(roleID);

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