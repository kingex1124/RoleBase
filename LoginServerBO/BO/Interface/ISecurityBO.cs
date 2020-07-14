using LoginDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO.Interface
{
    public interface ISecurityBO
    {
        IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId);
    }
}
