using LoginDTO.DTO;
using LoginServerBO.Service;
using LoginServerBO.Service.Interface;
using LoginVO.VO;
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
            _registService = RouteConfig.Container.Resolve<IRegistService>();
            _loginService = RouteConfig.Container.Resolve<ILoginService>();
            _securityService = RouteConfig.Container.Resolve<ISecurityService>();
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

        public ActionResult Regist()
        {
            return View("Regist");
        }

        [HttpPost]
        public ActionResult RegistAccount(Account account)
        {

            if (!ModelState.IsValid)
            {
                CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                account.Message = "請填寫必填欄位";
            }
            else
            {
                account = _registService.RegistValid(account);
                if (!string.IsNullOrWhiteSpace(account.Message))
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                else
                {
                    _registService.Regist(account);
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                }
            }
            return Json(account, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(AccountInfoData accountInfoData)
        {
            if (!ModelState.IsValid)
            {
                CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                accountInfoData.Message = "請填寫必填欄位";
                return View(accountInfoData);
            }
            else
            {
                accountInfoData = _loginService.AccountValid(accountInfoData);
                if (!string.IsNullOrWhiteSpace(accountInfoData.Message))
                {
                    CurrentHttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
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

                    foreach (var item in securityLevel.SecurityRole)
                        securityLevel.SecurityUrl.AddRange(_securityService.GetSecurityRoleFunction(item.RoleID.ToString()));


                    CurrentSecurityLevel = securityLevel;
                    CurrentHttpContext.Session["UserName"] = user.UserName;

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

        public ActionResult Logout()
        {
            CurrentHttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        #endregion

    }
}