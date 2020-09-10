using Login.DTO;
using Login.Service;
using Login.VO;
using Newtonsoft.Json;
using RoleBase.ActionFilters;
using RoleBase.CurrentStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace RoleBase.Controllers
{
    public class AccountAnController : Controller
    {
        #region 屬性

        IRegistService _registService;
        ILoginService _loginService;
        ISecurityService _securityService;

        #endregion

        #region 建構子

        public AccountAnController()
        {
            _registService = new RegistService();
            _loginService = new LoginService();
            _securityService = new SecurityService();
        }

        #endregion

        #region Action

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        [AllowCrossSite]
        [HttpPost]
        public ActionResult Login(AccountInfoData accountInfoData)
        {
            ExecuteResult result = new ExecuteResult();

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                result.IsSuccessed = false;
                result.Message = "請填寫必填欄位";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = _loginService.AccountValid(accountInfoData);
                if (!result.IsSuccessed)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    UserDTO user = _loginService.GetUserDataByAccountName(accountInfoData);
                    SecurityLevel securityLevel = new SecurityLevel();
                    AccountInfoData userInfoData = new AccountInfoData()
                    {
                        UserId = user.UserID,
                        AccountName = accountInfoData.AccountName,
                        UserName = accountInfoData.UserName
                    };
                    securityLevel.UserData = userInfoData;
                    securityLevel.SecurityRole = _loginService.GetRoleDataByUserID(user.UserID.ToString()).ToList();

                    foreach (var item in securityLevel.SecurityRole)
                        securityLevel.SecurityUrl.AddRange(_securityService.GetSecurityRoleFunction(item.RoleID.ToString()));

                    SessionConnectionPool.SetCurrentUserInfo(securityLevel);

                    Session["UserName"] = user.UserName;
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(securityLevel, JsonRequestBehavior.AllowGet);
                    //return Redirect("http://localhost:4200/");
                }
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [AllowCrossSite]
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(" ", JsonRequestBehavior.AllowGet);
        }

        [AllowCrossSite]
        [HttpPost]
        public ActionResult RegistAccount(Account account)
        {
            ExecuteResult result = new ExecuteResult();

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                account.Message = "請填寫必填欄位";
            }
            else
            {
                result = _registService.RegistValid(account);
                if (!result.IsSuccessed)
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                else
                {
                    _registService.Regist(account);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 測試用方法
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [AllowCrossSite]
        //[Route("api/[controller]/[action]")]
        //[EnableCors(origins: "*",//設定允許哪些來源網址，允許存取此web API
        // headers: "*", methods: "*")]
        [HttpPost]
        public ActionResult Test(Account account)
        {
            return Json(account, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}