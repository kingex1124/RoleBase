using LoginDTO.DTO;
using LoginServerBO.BO;
using LoginServerBO.Service;
using LoginServerBO.Service.Interface;
using LoginVO.VO;
using RoleBase.CurrentStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleBase.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserSessionAttribute : ActionFilterAttribute
    {
        #region 屬性

        ISecurityService _securityService;

        #endregion

        #region 建構子

        public UserSessionAttribute()
        {
            _securityService = new SecurityService();
        }

        #endregion

        #region 方法

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session[AccountInfoData.LoginInfo] != null || session.IsNewSession ? !session.IsNewSession : false)
            {
                List<SecurityRoleFunctionDTO> securityRoleFunctions = ((SecurityLevel)session[AccountInfoData.LoginInfo]).SecurityUrl;

                //foreach (RoleDTO roleArr in SessionConnectionPool.CurrentUserInfo.SecurityRole)
                //{
                //    var securityRF = securityService.GetSecurityRoleFunction(roleArr.RoleID.ToString());
                //        securityRoleFunctions.AddRange(securityRF);
                //}

                bool Check = true;
                if (securityRoleFunctions.Count == 0)
                    Check = false;
                else
                {
                    foreach (SecurityRoleFunctionDTO item in securityRoleFunctions)
                    {
                        if (string.Concat(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, "/", filterContext.ActionDescriptor.ActionName) == item.Url)
                        {
                            Check = true;
                            return;
                        }
                        else
                            Check = false;
                    }
                }
                if (Check)
                    OnActionExecuting(filterContext);
                else if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    UrlHelper url = new UrlHelper(filterContext.RequestContext);
                    filterContext.Result = new RedirectResult(url.Content("~/Home/NoCompetence"));
                }
                else
                {
                    string loginUrl = (new UrlHelper(filterContext.RequestContext)).Content("~/Home/NoCompetence");
                    filterContext.Result = new ContentResult()
                    {
                        Content = loginUrl
                    };
                }
            }
            else if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                UrlHelper url = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new RedirectResult(url.Content("~/Account/Login"));
            }
            else
            {
                string loginUrl = (new UrlHelper(filterContext.RequestContext)).Content("~/Account/Login");
                filterContext.Result = new ContentResult()
                {
                    Content = loginUrl
                };
            }
        }

        #endregion

    }
}