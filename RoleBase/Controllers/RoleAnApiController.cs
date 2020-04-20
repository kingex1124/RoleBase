using LoginServerBO.Service;
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
    public class RoleAnApiController : ApiController
    {
        RoleService roleService = new RoleService();

        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public IEnumerable<RoleVO> RoleAddEditDelete()
        {
            var roleData = roleService.GetRoleData();
            return roleData;
        }
    }
}
