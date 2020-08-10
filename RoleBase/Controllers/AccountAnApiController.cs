using LoginDTO.DTO;
using LoginServiceObj.Service;
using LoginServiceObj.Service.Interface;
using LoginVO.VO;
using RoleBase.CurrentStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RoleBase.Controllers
{
    public class AccountAnApiController : ApiController
    {
        #region 屬性

        IRegistService _registService;
        ILoginService _loginService;
        ISecurityService _securityService;

        #endregion

        #region 建構子

        public AccountAnApiController()
        {
            _registService = new RegistService();
            _loginService = new LoginService();
            _securityService = new SecurityService();
        }

        #endregion

        #region Action

        /// <summary>
        /// 用API解決跨域問題
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public Account RegistAccount(Account account)
        {
            HttpResponseMessage response = Request.CreateResponse();

            if (!ModelState.IsValid)
                account.Message = "請填寫必填欄位";
            else
            {
                account = _registService.RegistValid(account);
                if (string.IsNullOrWhiteSpace(account.Message))
                    _registService.Regist(account);
            }
            return account;
        }

        #endregion
    }
}
