using LoginDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServiceObj.Service.Interface
{
    public interface ISecurityService
    {
        IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId);
    }
}
