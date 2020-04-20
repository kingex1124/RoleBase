using LoginDTO.DTO;
using LoginServerBO.Service;
using LoginVO.VO;
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
        RegistService registService = new RegistService();
        LoginService loginService = new LoginService();
        SecurityService securityService = new SecurityService();
        // GET: AccountAn

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        [AllowCrossSite]
        [HttpPost]
        public ActionResult Login(AccountInfoData accountInfoData)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                accountInfoData.Message = "請填寫必填欄位";
                return Json(accountInfoData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                accountInfoData = loginService.AccountValid(accountInfoData);
                if (!string.IsNullOrWhiteSpace(accountInfoData.Message))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(accountInfoData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    UserDTO user = loginService.GetUserDataByAccountName(accountInfoData);
                    SecurityLevel securityLevel = new SecurityLevel();
                    AccountInfoData userInfoData = new AccountInfoData()
                    {
                        UserId = user.UserID,
                        AccountName = accountInfoData.AccountName,
                        UserName = accountInfoData.UserName
                    };
                    securityLevel.UserData = userInfoData;
                    securityLevel.SecurityRole = loginService.GetRoleDataByUserID(user.UserID.ToString()).ToList();

                    foreach (var item in securityLevel.SecurityRole)
                        securityLevel.SecurityUrl.AddRange(securityService.GetSecurityRoleFunction(item.RoleID.ToString()));

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
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                account.Message = "請填寫必填欄位";
            }
            else
            {
                account = registService.RegistValid(account);
                if (!string.IsNullOrWhiteSpace(account.Message))
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                else
                {
                    registService.Regist(account);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                }
            }
            return Json(account, JsonRequestBehavior.AllowGet);
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
    }
}