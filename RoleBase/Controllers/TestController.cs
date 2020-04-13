using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RoleBase.Controllers
{
    public class TestController : ApiController
    {
        /// <summary>
        /// Api測試用
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [EnableCors(
         origins: "*",//設定允許哪些來源網址，允許存取此web API
          headers: "*",
          methods: "*")]
        [HttpPost]
        public Account Test(Account account)
        {
            return account;
        }
    }
}
