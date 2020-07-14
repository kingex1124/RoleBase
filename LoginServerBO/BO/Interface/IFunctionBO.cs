using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO.Interface
{
    public interface IFunctionBO
    {
        IEnumerable<FunctionDTO> GetFunctionData();
        int AddFunction(FunctionVO functionVO);
        int DeleteFunction(string id, ref SqlConnection conn, ref SqlTransaction tran);
        int DeleteRoleFunctionByFunctionID(string functionID, ref SqlConnection conn, ref SqlTransaction tran);
        int EditFunction(FunctionVO functionVO);
        IEnumerable<FunctionCheckDTO> GetFunctionCheckByRole(string id);
        int DeleteRoleFunctionByRoleID(string roleID, ref SqlConnection conn, ref SqlTransaction tran);
        int InsertRoleFunction(RoleFunctionDTO roleFunctionDTO, ref SqlConnection conn, ref SqlTransaction tran);
    }
}
