using Login.DTO;
using Login.DTO.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public interface IRoleFunctionEfRepository : IRepository<RoleFunction>
    {
        IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string userID);

        int DeleteRoleFunctionByFunctionID(string functionID);

        IEnumerable<FunctionCheckDTO> GetFunctionCheckByRole(string id);

        int DeleteRoleFunctionByRoleID(string roleID);

        int InsertRoleFunction(RoleFunctionDTO roleFunctionDTO);
    }
}
