using Login.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.BO
{
    public interface ISecurityBO
    {
        IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string userID);
    }
}
