using Login.Service;
using Login.VO;
using RoleBase.CurrentStatus;
using RoleBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleBase.Controllers
{
    public class BaseController : Controller
    {
        public ILoginService _loginServiceBase;
        public ISecurityService _securityServiceBase;

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

        public BaseController()
        {
            _loginServiceBase = RouteConfig.Container.Resolve<ILoginService, LoginService>();
            _securityServiceBase = RouteConfig.Container.Resolve<ISecurityService, SecurityService>();
        }


        public BaseController(ILoginService loginService, ISecurityService securityService)
        {
            _loginServiceBase = loginService;
            _securityServiceBase = securityService;
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
            securityLevel.SecurityRole = _loginServiceBase.GetRoleDataByUserID(CurrentHttpContext.Session["UserID"].ToString()).ToList();

            securityLevel.SecurityUrl.AddRange(_securityServiceBase.GetSecurityRoleFunction(securityLevel.UserData.UserId.ToString()));

            CurrentSecurityLevel = securityLevel;
        }
    }
}