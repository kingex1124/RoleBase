using LoginDTO.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Repository.Interface
{
    public interface IRoleFunctionRepository
    {
        IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId);

        int DeleteRoleFunctionByFunctionID(string functionID, ref SqlConnection conn, ref SqlTransaction tran);

        IEnumerable<FunctionCheckDTO> GetFunctionCheckByRole(string id);

        int DeleteRoleFunctionByRoleID(string roleID, ref SqlConnection conn, ref SqlTransaction tran);

        int InsertRoleFunction(RoleFunctionDTO roleFunctionDTO, ref SqlConnection conn, ref SqlTransaction tran);
    }
}
