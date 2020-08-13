using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.BO
{
    public interface IFunctionBO
    {
        IEnumerable<FunctionVO> GetFunctionData();
        IEnumerable<KeyValuePairVO> GetParentKeyValue();
        string AddFunction(FunctionVO functionVO);
        string DeleteFunction(string id);
        string EditFunction(FunctionVO functionVO);
        IEnumerable<FunctionCheckVO> GetFunctionCheckByRole(string roleID);
        string SaveRoleFunctionSetting(IEnumerable<FunctionCheckVO> functionCheckVO);
        string ClearRoleFunctionByRoleID(string roleID);
        List<FunctionMenuNode> GetFunctionToNode(string userID);
        void SetNode(List<FunctionMenuVO> vo, FunctionMenuNode node);
    }
}
