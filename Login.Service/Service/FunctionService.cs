using Login.BO;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Service
{
    public class FunctionService : IFunctionService
    {
        #region 屬性

        IFunctionBO _functionBO;

        #endregion

        #region 建構子

        public FunctionService()
        {
            _functionBO = new FunctionBO();
        }

        public FunctionService(IFunctionBO functionBO)
        {
            _functionBO = functionBO;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 取得Function資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionVO> GetFunctionData(PageDataVO pageDataVO)
        {
            return _functionBO.GetFunctionData(pageDataVO);
        }

        /// <summary>
        /// 取得作為上層的keyValue資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePairVO> GetParentKeyValue()
        {
            return _functionBO.GetParentKeyValue();
        }

        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public string AddFunction(FunctionVO functionVO)
        {
            return _functionBO.AddFunction(functionVO);
        }

        /// <summary>
        /// 刪除功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteFunction(string id)
        {
            return _functionBO.DeleteFunction(id);
        }

        /// <summary>
        /// 編輯功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public string EditFunction(FunctionVO functionVO)
        {
            return _functionBO.EditFunction(functionVO);
        }

        /// <summary>
        /// 取得角色設定功能的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IEnumerable<FunctionCheckVO> GetFunctionCheckByRole(string roleID)
        {
            return _functionBO.GetFunctionCheckByRole(roleID);
        }

        /// <summary>
        /// 角色編輯功能
        /// 儲存勾選功能時的變更
        /// </summary>
        /// <param name="functionCheckVO"></param>
        /// <returns></returns>
        public string SaveRoleFunctionSetting(IEnumerable<FunctionCheckVO> functionCheckVO)
        {
            return _functionBO.SaveRoleFunctionSetting(functionCheckVO);
        }

        /// <summary>
        /// 角色編輯功能
        /// 儲存清空功能的變更
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public string ClearRoleFunctionByRoleID(string roleID)
        {
            return _functionBO.ClearRoleFunctionByRoleID(roleID);
        }

        /// <summary>
        /// 取得MenuNode
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<FunctionMenuNode> GetFunctionNode(string userID)
        {
            return _functionBO.GetFunctionToNode(userID);
        }

        #endregion
    }
}
