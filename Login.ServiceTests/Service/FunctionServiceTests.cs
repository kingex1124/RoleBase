using Login.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.BO;
using Rhino.Mocks;
using Login.VO;

namespace Login.Service.Tests
{
    [TestClass()]
    public class FunctionServiceTests
    {
        #region 屬性

        IFunctionBO _functionBO = MockRepository.GenerateStub<IFunctionBO>();

        FunctionService _target;

        #endregion

        #region 建構子

        public FunctionServiceTests()
        {
            _target = new FunctionService(_functionBO);
        }

        #endregion

        #region 測試方法

        #region GetFunctionData

        /// <summary>
        /// 取得Function資料
        /// </summary>
        [TestMethod()]
        public void GetFunctionDataTest()
        {
            #region arrange

            List<FunctionVO> reFunctionVO = new List<FunctionVO>()
            {
                new FunctionVO(){ FunctionID = 1 , Url="Role/RoleManagement" , Title = "角色管理" , Description = "瀏覽角色管理畫面" , IsMenu = true , Parent = 0 , ParentName = "No"  },
                new FunctionVO(){ FunctionID = 2 , Url="Role/RoleAddEditDelete" , Title = "編輯角色" , Description = "角色新增修改刪除畫面" , IsMenu = true , Parent = 1 , ParentName = "角色管理"  },
                new FunctionVO(){ FunctionID = 3 , Url="Role/EditRole" , Title = "編輯" , Description = "編輯角色" , IsMenu = false , Parent = -1 , ParentName = "Not Menu" }
            };

            PageDataVO pageDataVO = new PageDataVO()
            {
                PageNumber = 1,
                PageSize = 5,
                WhereCondition = new List<KeyValueVO>()
                   {
                        new KeyValueVO()
                        {
                             Key = "Url",
                             Value = ""
                        }
                   }
            };

            _functionBO.Stub(o => o.GetFunctionData(pageDataVO)).Return(reFunctionVO);

            #endregion

            #region act

            var result = _target.GetFunctionData(pageDataVO).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(result[i].FunctionID, reFunctionVO[i].FunctionID);
                Assert.AreEqual(result[i].Url, reFunctionVO[i].Url);
                Assert.AreEqual(result[i].Title, reFunctionVO[i].Title);
                Assert.AreEqual(result[i].Description, reFunctionVO[i].Description);
                Assert.AreEqual(result[i].IsMenu, reFunctionVO[i].IsMenu);
                Assert.AreEqual(result[i].Parent, reFunctionVO[i].Parent);
                Assert.AreEqual(result[i].ParentName, reFunctionVO[i].ParentName);
            }

            #endregion
        }

        #endregion

        #region GetParentKeyValueTest

        /// <summary>
        /// 取得作為上層的keyValue資料
        /// </summary>
        [TestMethod()]
        public void GetParentKeyValueTest()
        {
            #region arrang

            List<KeyValuePairVO> reKeyValuePairVO = new List<KeyValuePairVO>()
            {
                new KeyValuePairVO(){ Key = 1 , Value = "角色管理"},
                new KeyValuePairVO(){ Key = 2 , Value = "編輯角色"},
                new KeyValuePairVO(){ Key = 3 , Value = "新增"},
            };

            _functionBO.Stub(o => o.GetParentKeyValue()).Return(reKeyValuePairVO);

            #endregion

            #region act

            var result = _target.GetParentKeyValue().ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(result[i].Key, reKeyValuePairVO[i].Key);
                Assert.AreEqual(result[i].Value, reKeyValuePairVO[i].Value);
            }

            #endregion
        }

        #endregion

        #region AddFunction

        /// <summary>
        /// 新增功能
        /// 新增成功
        /// </summary>
        [TestMethod()]
        public void AddFunctionTest()
        {
            #region arrange (新增成功)

            FunctionVO functionVO = new FunctionVO() { Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            string reMessage = "";

            _functionBO.Stub(o => o.AddFunction(Arg<FunctionVO>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.AddFunction(functionVO);

            #endregion

            #region assert

            Assert.IsTrue(string.IsNullOrEmpty(result));

            #endregion
        }

        /// <summary>
        /// 新增功能
        /// 新增失敗
        /// </summary>
        [TestMethod()]
        public void AddFunctionTest1()
        {
            #region arrange (新增失敗)

            FunctionVO functionVO = new FunctionVO() { Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            string reMessage = "新增失敗。";

            _functionBO.Stub(o => o.AddFunction(Arg<FunctionVO>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.AddFunction(functionVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region DeleteFunction

        /// <summary>
        /// 刪除功能
        /// 測試刪除成功
        /// </summary>
        [TestMethod()]
        public void DeleteFunctionTest()
        {
            #region arrange (刪除成功)

            string id = "1";

            string reMessage = "";

            _functionBO.Stub(o => o.DeleteFunction(Arg<string>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.DeleteFunction(id);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 刪除功能
        /// 測試刪除失敗
        /// </summary>
        [TestMethod()]
        public void DeleteFunctionTest1()
        {
            #region arrange (刪除失敗)

            string id = "1";

            string reMessage = "刪除失敗。";

            _functionBO.Stub(o => o.DeleteFunction(Arg<string>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.DeleteFunction(id);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region EditFunction

        /// <summary>
        /// 編輯功能
        /// 測試編輯成功
        /// </summary>
        [TestMethod()]
        public void EditFunctionTest()
        {
            #region arrange (編輯成功)

            FunctionVO functionVO = new FunctionVO() { FunctionID = 1, Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            string reMessage = string.Empty;

            _functionBO.Stub(o => o.EditFunction(Arg<FunctionVO>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.EditFunction(functionVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 編輯功能
        /// 測試編輯失敗
        /// </summary>
        [TestMethod()]
        public void EditFunctionTest1()
        {
            #region arrange (編輯失敗)

            FunctionVO functionVO = new FunctionVO() { FunctionID = 1, Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            string reMessage = "編輯失敗";

            _functionBO.Stub(o => o.EditFunction(Arg<FunctionVO>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.EditFunction(functionVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region GetFunctionCheckByRole

        /// <summary>
        /// 取得角色設定功能的資料
        /// </summary>
        [TestMethod()]
        public void GetFunctionCheckByRoleTest()
        {
            #region arrange

            string roleID = "1";

            List<FunctionCheckVO> reFunctionCheckVO = new List<FunctionCheckVO>()
            {
                new FunctionCheckVO(){ FunctionID = 1 , Url = "Role/RoleManagement" , Title = "角色管理", Description = "瀏覽角色管理畫面" , IsMenu = true, ParentName = "No", Check = true },
                new FunctionCheckVO(){ FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Title = "編輯角色", Description = "角色新增修改刪除畫面" , IsMenu = true, ParentName = "角色管理" , Check = true },
                new FunctionCheckVO(){ FunctionID = 3 , Url = "Role/EditRole", Title = "編輯" , Description = "編輯角色" , IsMenu = false, ParentName = "Not Menu" , Check = false }
            };

            PageDataVO pageDataVO = new PageDataVO() { OrderByColumn = "UserID", OrderByType = "ASC" };

            _functionBO.Stub(o => o.GetFunctionCheckByRole(Arg<string>.Is.Anything, Arg<PageDataVO>.Is.Anything)).Return(reFunctionCheckVO);

            #endregion

            #region act

            var result = _target.GetFunctionCheckByRole(roleID, pageDataVO).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].RoleID, reFunctionCheckVO[i].RoleID);
                Assert.AreEqual(result[i].FunctionID, reFunctionCheckVO[i].FunctionID);
                Assert.AreEqual(result[i].Url, reFunctionCheckVO[i].Url);
                Assert.AreEqual(result[i].Title, reFunctionCheckVO[i].Title);
                Assert.AreEqual(result[i].Description, reFunctionCheckVO[i].Description);
                Assert.AreEqual(result[i].IsMenu, reFunctionCheckVO[i].IsMenu);
                Assert.AreEqual(result[i].ParentName, reFunctionCheckVO[i].ParentName);
                Assert.AreEqual(result[i].Check, reFunctionCheckVO[i].Check);
            }

            #endregion
        }

        #endregion

        #region SaveRoleFunctionSetting

        /// <summary>
        /// 角色編輯功能
        /// 儲存勾選功能時的變更
        /// 測試成功
        /// </summary>
        [TestMethod()]
        public void SaveRoleFunctionSettingTest()
        {
            #region arrange (測試成功)

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>()
            {
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 1 , Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 3 , Url = "Role/EditRole" , Description = "編輯角色" , Check = false }
            };

            string reMessage = string.Empty;

            _functionBO.Stub(o => o.SaveRoleFunctionSetting(Arg<List<FunctionCheckVO>>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.SaveRoleFunctionSetting(functionCheckVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 角色編輯功能
        /// 儲存勾選功能時的變更
        /// 測試失敗
        /// </summary>
        [TestMethod()]
        public void SaveRoleFunctionSettingTest1()
        {
            #region arrange (測試失敗)

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>()
            {
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 1 , Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 3 , Url = "Role/EditRole" , Description = "編輯角色" , Check = false }
            };

            string reMessage = "刪除失敗。";

            _functionBO.Stub(o => o.SaveRoleFunctionSetting(Arg<List<FunctionCheckVO>>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.SaveRoleFunctionSetting(functionCheckVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region ClearRoleFunctionByRole

        /// <summary>
        /// 角色編輯功能
        /// 儲存清空功能的變更
        /// 測試成功
        /// </summary>
        [TestMethod()]
        public void ClearRoleFunctionByRoleIDTest()
        {
            #region arrange (測試成功)

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>() { };

            string roleID = "1";

            string reMessage = string.Empty;

            _functionBO.Stub(o => o.ClearRoleFunctionByRoleID(Arg<string>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.ClearRoleFunctionByRoleID(roleID);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 角色編輯功能
        /// 儲存清空功能的變更
        /// 測試失敗
        /// </summary>
        [TestMethod()]
        public void ClearRoleFunctionByRoleIDTest1()
        {
            #region arrange (測試失敗)

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>() { };

            string roleID = "1";

            string reMessage = "刪除失敗。";

            _functionBO.Stub(o => o.ClearRoleFunctionByRoleID(Arg<string>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var result = _target.ClearRoleFunctionByRoleID(roleID);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region GetFunctionNodeTest

        /// <summary>
        /// 取得MenuNode
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [TestMethod()]
        public void GetFunctionNodeTest()
        {
            #region arrange

            string userID = "1";

            List<FunctionMenuNode> reFunctionMenuNodeList = new List<FunctionMenuNode>()
            {
                new FunctionMenuNode(new FunctionMenuVO(){ FunctionID = 1 , Url = "Role/RoleManagement" , Parent = 0 , Title = "角色管理" })
                {
                     Next = new List<FunctionMenuNode>()
                     {
                         new FunctionMenuNode(new FunctionMenuVO(){ FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Parent = 1 , Title = "編輯角色"}),
                         new FunctionMenuNode(new FunctionMenuVO(){  FunctionID = 8 , Url = "Role/RoleUserEdit" , Parent = 1 , Title = "編輯角色使用者"})
                     }
                }
            };

            _functionBO.Stub(o => o.GetFunctionToNode(Arg<string>.Is.Anything)).Return(reFunctionMenuNodeList);

            #endregion

            #region act

            var result = _target.GetFunctionNode(userID).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].Val.FunctionID, reFunctionMenuNodeList[i].Val.FunctionID);
                Assert.AreEqual(result[i].Val.Url, reFunctionMenuNodeList[i].Val.Url);
                Assert.AreEqual(result[i].Val.Parent, reFunctionMenuNodeList[i].Val.Parent);
                Assert.AreEqual(result[i].Val.Title, reFunctionMenuNodeList[i].Val.Title);
            }

            for (int i = 0; i < result[0].Next.Count; i++)
            {
                Assert.AreEqual(result[0].Next[i].Val.FunctionID, reFunctionMenuNodeList[0].Next[i].Val.FunctionID);
                Assert.AreEqual(result[0].Next[i].Val.Url, reFunctionMenuNodeList[0].Next[i].Val.Url);
                Assert.AreEqual(result[0].Next[i].Val.Parent, reFunctionMenuNodeList[0].Next[i].Val.Parent);
                Assert.AreEqual(result[0].Next[i].Val.Title, reFunctionMenuNodeList[0].Next[i].Val.Title);
            }

            #endregion
        }

        #endregion

        #endregion
    }
}