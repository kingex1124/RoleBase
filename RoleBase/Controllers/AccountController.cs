using LoginDTO.DTO;
using LoginServerBO.Service;
using LoginServerBO.Service.Interface;
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
        #region 屬性

        IRegistService _registService;
        ILoginService _loginService; 
        ISecurityService _securityService;

        #endregion

        #region 建構子

        public AccountController()
        {
            _registService = new RegistService();
            _loginService = new LoginService();
            _securityService = new SecurityService();
        }

        #endregion

        #region Action

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
                account = _registService.RegistValid(account);
                if (!string.IsNullOrWhiteSpace(account.Message))
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                else
                {
                    _registService.Regist(account);
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
                accountInfoData = _loginService.AccountValid(accountInfoData);
                if (!string.IsNullOrWhiteSpace(accountInfoData.Message))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return View(accountInfoData);
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

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        #endregion

    }
}