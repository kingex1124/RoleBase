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
        IEnumerable<FunctionVO> GetFunctionData(PageDataVO pageDataVO);
        IEnumerable<KeyValuePairVO> GetParentKeyValue();
        string AddFunction(FunctionVO functionVO);
        string DeleteFunction(string id);
        string EditFunction(FunctionVO functionVO);
        IEnumerable<FunctionCheckVO> GetFunctionCheckByRole(string roleID, PageDataVO pageDataVO);
        string SaveRoleFunctionSetting(IEnumerable<FunctionCheckVO> functionCheckVO);
        string ClearRoleFunctionByRoleID(string roleID);
        List<FunctionMenuNode> GetFunctionToNode(string userID);
        void SetNode(List<FunctionMenuVO> vo, FunctionMenuNode node);
    }
}
