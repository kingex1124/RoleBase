﻿using Login.DTO;
using Login.Service;
using Login.VO;
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
    public class AccountController : Controller
    {
        #region 屬性

        public IRegistService _registService;
        public ILoginService _loginService;
        public ISecurityService _securityService;

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

        public AccountController()
        {
            _registService = RouteConfig.Container.Resolve<IRegistService, RegistService>();
            _loginService = RouteConfig.Container.Resolve<ILoginService, LoginService>();
            _securityService = RouteConfig.Container.Resolve<ISecurityService, SecurityService>();
            //_registService = new RegistService();
            //_loginService = new LoginService();
            //_securityService = new SecurityService();
        }

        public AccountController(IRegistService registService, ILoginService loginService, ISecurityService securityService)
        {
            _registService = registService;
            _loginService = loginService;
            _securityService = securityService;
        }

        #endregion

        #region Action

        /// <summary>
        /// 註冊頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Regist()
        {
            return View("Regist");
        }

        /// <summary>
        /// 註冊帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegistAccount(Account account)
        {
            ExecuteResult result = new ExecuteResult();

            // 前端欄位驗證
            if (!ModelState.IsValid)
            {
                CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                result.IsSuccessed = false;
                result.Message = "請填寫必填欄位";
            }
            else
            {
                result = _registService.RegistValid(account);
                if (!result.IsSuccessed)
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                else
                {
                    result = _registService.Regist(account);
                    if (result.IsSuccessed)
                        CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    else
                        CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 登入頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View("Login");
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(AccountInfoData accountInfoData)
        {
            ExecuteResult result = new ExecuteResult();

            if (!ModelState.IsValid)
            {
                CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                accountInfoData.Message = "請填寫必填欄位";
                return View(accountInfoData);
            }
            else
            {
                result = _loginService.AccountValid(accountInfoData);
                if (!result.IsSuccessed)
                {
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    accountInfoData.Message = result.Message;
                    return View(accountInfoData);
                }
                else
                {
                    UserDTO user = _loginService.GetUserDataByAccountName(accountInfoData);
                    SecurityLevel securityLevel = new SecurityLevel();
                    AccountInfoData userInfoData = new AccountInfoData()
                    {
                        UserId = user.UserID,
                        AccountName = accountInfoData.AccountName
                    };

                    securityLevel.UserData = userInfoData;
                    securityLevel.SecurityRole = _loginService.GetRoleDataByUserID(user.UserID.ToString()).ToList();
             
                    securityLevel.SecurityUrl.AddRange(_securityService.GetSecurityRoleFunction(securityLevel.UserData.UserId.ToString()));


                    CurrentSecurityLevel = securityLevel;
                    CurrentHttpContext.Session["UserName"] = user.UserName;
                    CurrentHttpContext.Session["UserID"] = user.UserID;
                    CurrentHttpContext.Session["AccountName"] = user.AccountName;

                    // UnitTest用
                    //if (HttpContext == null)
                    //{
                    //    CurrentHttpContext.Session[AccountInfoData.LoginInfo] = securityLevel;
                    //    CurrentHttpContext.Session["UserName"] = user.UserName;
                    //}
                    //else
                    //{
                    //    SessionConnectionPool.SetCurrentUserInfo(securityLevel);
                    //    Session["UserName"] = user.UserName;
                    //}

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            CurrentHttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// 取得授權資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAuthList()
        {
            var authData = CurrentHttpContext.Session[AccountInfoData.LoginInfo];
            return Json(authData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}