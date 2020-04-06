using LoginDTO.DTO;
using LoginServerBO.Service;
using LoginVO.VO;
using RoleBase.CurrentStatus;
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
        RegistService registService = new RegistService();
        LoginService loginService = new LoginService();
        SecurityService securityService = new SecurityService();
        // GET: Account
        public ActionResult Regist()
        {
            return View();
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountInfoData accountInfoData)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                accountInfoData.Message = "請填寫必填欄位";
                return View(accountInfoData);
            }
            else
            {
                accountInfoData = loginService.AccountValid(accountInfoData);
                if (!string.IsNullOrWhiteSpace(accountInfoData.Message))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return View(accountInfoData);
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

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}