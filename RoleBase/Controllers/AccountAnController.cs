using LoginDTO.DTO;
using LoginServerBO.Service;
using LoginVO.VO;
using Newtonsoft.Json;
using RoleBase.CurrentStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RoleBase.Controllers
{
    public class AccountAnController : Controller
    {
        RegistService registService = new RegistService();
        LoginService loginService = new LoginService();
        SecurityService securityService = new SecurityService();
        // GET: AccountAn
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

        public ActionResult Logout()
        {
            Session.Clear();
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(" ", JsonRequestBehavior.AllowGet);
        }

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
    }
}