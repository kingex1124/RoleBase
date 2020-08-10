using LoginDTO.DTO;
using LoginDTO.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDAL.EfRepository.Interface
{
    public interface IRoleFunctionEfRepository : IRepository<RoleFunction>
    {
        IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId);

        int DeleteRoleFunctionByFunctionID(string functionID);

        IEnumerable<FunctionCheckDTO> GetFunctionCheckByRole(string id);

        int DeleteRoleFunctionByRoleID(string roleID);

        int InsertRoleFunction(RoleFunctionDTO roleFunctionDTO);
    }
}
