using Login.Service;
using Login.VO;
using RoleBase.ActionFilters;
using RoleBase.CurrentStatus;
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

        ILoginService _loginService;
        ISecurityService _securityService;

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

        private SecurityLevel _currentUserInfo;

        public SecurityLevel CurrentSecurityLevel
        {
            get
            {
                if (_currentUserInfo != null)
                    return _currentUserInfo;

                return SessionConnectionPool.GetCurrentUserInfo;
            }
            set
            {
                if (HttpContext != null)
                    SessionConnectionPool.SetCurrentUserInfo(value);
                else
                    SessionConnectionPool.SetCurrentUserInfo(CurrentHttpContext, value);
                _currentUserInfo = value;
            }
        }

        #endregion

        #region 建構子

        public FunctionController()
        {
            _functionService = RouteConfig.Container.Resolve<IFunctionService, FunctionService>();
            _roleService = RouteConfig.Container.Resolve<IRoleService, RoleService>();
            _loginService = RouteConfig.Container.Resolve<ILoginService, LoginService>();
            _securityService = RouteConfig.Container.Resolve<ISecurityService, SecurityService>();
        }

        public FunctionController(IFunctionService functionService, IRoleService roleService, ILoginService loginService, ISecurityService securityService)
        {
            _functionService = functionService;
            _roleService = roleService;
            _loginService = loginService;
            _securityService = securityService;
        }

        #endregion

        #region Action

        /// <summary>
        /// 取得選單資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFunctionMenu(string userID)
        {
            var data = _functionService.GetFunctionNode(userID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

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
            // var functionData = _functionService.GetFunctionData();
            return View("FunctionAddEditDelete");
        }

        /// <summary>
        /// 查詢功能
        /// </summary>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult QueryFunction(PageDataVO pageDataVO)
        {
            FunctionTableResultVO result = new FunctionTableResultVO();
            result.FunctionData = _functionService.GetFunctionData(pageDataVO);
            result.TableMaxPage = pageDataVO.AllPageNumber;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得作為上層的keyValue資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FunctionGetParentData()
        {
            var parentData = _functionService.GetParentKeyValue();

            return Json(parentData, JsonRequestBehavior.AllowGet);
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
            SessionReflash();

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
           //var roleData = _roleService.GetRoleData();
            return View("RoleFunctionEdit");
        }

        /// <summary>
        /// 查詢腳色資料
        /// </summary>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult QueryRoleFunctionEditRole(PageDataVO pageDataVO)
        {
            RoleTableResultVO result = new RoleTableResultVO();
            result.RoleData = _roleService.GetRoleData(pageDataVO);
            result.TableMaxPage = pageDataVO.AllPageNumber;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 透過角色ID取得勾選的功能資料
        /// 編輯角色與功能的關係
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserSession]
        [HttpPost]
        public ActionResult GetFunctionByRole(string id, PageDataVO pageDataVO)
        {
            var functionCheckData = _functionService.GetFunctionCheckByRole(id, pageDataVO);
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
            SessionReflash();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 刷新權限Sesstion
        /// </summary>
        public void SessionReflash()
        {
            SecurityLevel securityLevel = new SecurityLevel();
            AccountInfoData userInfoData = new AccountInfoData()
            {
                UserId = Convert.ToInt32(CurrentHttpContext.Session["UserID"]),
                AccountName = CurrentHttpContext.Session["AccountName"].ToString()
            };

            securityLevel.UserData = userInfoData;
            securityLevel.SecurityRole = _loginService.GetRoleDataByUserID(CurrentHttpContext.Session["UserID"].ToString()).ToList();

            securityLevel.SecurityUrl.AddRange(_securityService.GetSecurityRoleFunction(securityLevel.UserData.UserId.ToString()));

            CurrentSecurityLevel = securityLevel;
        }

        #endregion
    }
}