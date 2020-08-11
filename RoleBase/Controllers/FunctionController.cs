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
    public class FunctionController : Controller
    {
        #region 屬性

        IFunctionService _functionService;
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

        public FunctionController()
        {
            _functionService = RouteConfig.Container.Resolve<IFunctionService>();
            _roleService = RouteConfig.Container.Resolve<IRoleService>(); 
        }

        public FunctionController(IFunctionService functionService, IRoleService roleService)
        {
            _functionService = functionService;
            _roleService = roleService;
        }

        #endregion

        #region Action

        /// <summary>
        /// Function管理介面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult FunctionManagement()
        {
            return View("FunctionManagement");
        }

        /// <summary>
        /// Function新增、修改、刪除畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult FunctionAddEditDelete()
        {
            var functionData = _functionService.GetFunctionData();
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
                var result = _functionService.AddFunction(functionVO);

                if (!string.IsNullOrEmpty(result))
                {
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    functionVO.Message = result;
                }
                else
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
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
            var result = _functionService.DeleteFunction(id);

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
        /// 編輯Function
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult EditFunction(FunctionVO functionVO)
        {
            var result = _functionService.EditFunction(functionVO);

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
        /// 轉到編輯角色功能關聯的畫面
        /// </summary>
        /// <returns></returns>
        [UserSession]
        public ActionResult RoleFunctionEdit()
        {
            var roleData = _roleService.GetRoleData();
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
            var functionCheckData = _functionService.GetFunctionCheckByRole(id);
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
                result = _functionService.SaveRoleFunctionSetting(functionCheckVO);

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
                result = _functionService.ClearRoleFunctionByRoleID(roleID);

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