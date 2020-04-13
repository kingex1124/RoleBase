using LoginDTO.DTO;
using LoginServerBO.Service;
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
        RegistService registService = new RegistService();
        LoginService loginService = new LoginService();
        SecurityService securityService = new SecurityService();

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
                account = registService.RegistValid(account);
                if (string.IsNullOrWhiteSpace(account.Message))
                    registService.Regist(account);
            }
            return account;
        }
    }
}
