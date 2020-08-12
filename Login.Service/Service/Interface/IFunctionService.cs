using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Service
{
    public interface IFunctionService
    {
        IEnumerable<FunctionVO> GetFunctionData();
        string AddFunction(FunctionVO functionVO);
        string DeleteFunction(string id);
        string EditFunction(FunctionVO functionVO);
        IEnumerable<FunctionCheckVO> GetFunctionCheckByRole(string roleID);
        string SaveRoleFunctionSetting(IEnumerable<FunctionCheckVO> functionCheckVO);
        string ClearRoleFunctionByRoleID(string roleID);
        IEnumerable<KeyValuePairVO> GetParentKeyValue();
    }
}
