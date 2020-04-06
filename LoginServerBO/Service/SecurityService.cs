using LoginDTO.DTO;
using LoginServerBO.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Service
{
    public class SecurityService
    {
        SecurityBO securityBO;
        public SecurityService()
        {
            securityBO = new SecurityBO();
        }

        public IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId)
        {
            return securityBO.GetSecurityRoleFunction(roleId);
        }
    }
}
