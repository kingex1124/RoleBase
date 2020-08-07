using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginServerBO.EfBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginServerBO.EfRepository.Interface;
using Rhino.Mocks;
using LoginVO.VO;
using LoginDTO.DTO;

namespace LoginServerBO.EfBO.Tests
{
    [TestClass()]
    public class FunctionEfBOTests
    {
        #region 屬性

        IFunctionEfRepository _functionEfRepo = MockRepository.GenerateStub<IFunctionEfRepository>();
        IRoleFunctionEfRepository _roleFunctionEfRepo = MockRepository.GenerateStub<IRoleFunctionEfRepository>();
      
        FunctionEfBO _target;

        #endregion

        #region 建構子

        public FunctionEfBOTests()
        {
            _target = new FunctionEfBO(_functionEfRepo, _roleFunctionEfRepo);
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
                new FunctionVO(){ FunctionID = 1 , Url="Role/RoleManagement" , Description = "瀏覽角色管理畫面" },
                new FunctionVO(){ FunctionID = 2 , Url="Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" },
                new FunctionVO(){ FunctionID = 3 , Url="Role/EditRole" , Description = "編輯角色" }
            };

            List<FunctionDTO> reFunctionDTO = new List<FunctionDTO>()
            {
                new FunctionDTO(){ FunctionID = 1 , Url="Role/RoleManagement" , Description = "瀏覽角色管理畫面" },
                new FunctionDTO(){ FunctionID = 2 , Url="Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" },
                new FunctionDTO(){ FunctionID = 3 , Url="Role/EditRole" , Description = "編輯角色" }
            };

            _functionEfRepo.Stub(o => o.GetFunctionData()).Return(reFunctionDTO);

            #endregion

            #region act

            var result = _target.GetFunctionData().ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(result[i].FunctionID, reFunctionVO[i].FunctionID);
                Assert.AreEqual(result[i].Url, reFunctionVO[i].Url);
                Assert.AreEqual(result[i].Description, reFunctionVO[i].Description);
            }

            #endregion
        }

        #endregion

        #region AddFunction

        /// <summary>
        /// 新增功能
        /// 測試新增成功
        /// </summary>
        [TestMethod()]
        public void AddFunctionTest()
        {
            #region arrange (新增成功)

            FunctionVO functionVO = new FunctionVO() { Url = "Role/RoleManagement", Description = "瀏覽角色管理畫面" };

            int reNumber = 1;

            string reMessage = "";

            _functionEfRepo.Stub(o => o.AddFunction(Arg<FunctionVO>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.AddFunction(functionVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 新增功能
        /// 測試失敗
        /// </summary>
        [TestMethod()]
        public void AddFunctionTest1()
        {
            #region arrange (新增失敗)

            FunctionVO functionVO = new FunctionVO() { Url = "Role/RoleManagement", Description = "瀏覽角色管理畫面" };

            int reNumber = -1;

            string reMessage = "新增失敗。";

            _functionEfRepo.Stub(o => o.AddFunction(Arg<FunctionVO>.Is.Anything)).Return(reNumber);

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

            string reMessage = string.Empty;

            int reDeleteRoleFunctionResult = 1;

            int reDeleteFunctionResult = 1;

            _roleFunctionEfRepo.Stub(o => o.DeleteRoleFunctionByFunctionID(Arg<string>.Is.Anything)).Return(reDeleteRoleFunctionResult);

            _functionEfRepo.Stub(o => o.DeleteFunction(Arg<string>.Is.Anything)).Return(reDeleteFunctionResult);

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

            int reDeleteRoleFunctionResult = -1;

            int reDeleteFunctionResult = 1;

            _roleFunctionEfRepo.Stub(o => o.DeleteRoleFunctionByFunctionID(Arg<string>.Is.Anything)).Return(reDeleteRoleFunctionResult);

            _functionEfRepo.Stub(o => o.DeleteFunction(Arg<string>.Is.Anything)).Return(reDeleteFunctionResult);

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

            FunctionVO functionVO = new FunctionVO() { FunctionID = 1, Url = "Role/RoleManagement", Description = "瀏覽角色管理畫面" };

            int reNumber = 1;

            string reMessage = string.Empty;

            _functionEfRepo.Stub(o => o.EditFunction(Arg<FunctionVO>.Is.Anything)).Return(reNumber);

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

            FunctionVO functionVO = new FunctionVO() { FunctionID = 1, Url = "Role/RoleManagement", Description = "瀏覽角色管理畫面" };

            int reNumber = -1;

            string reMessage = "編輯失敗";

            _functionEfRepo.Stub(o => o.EditFunction(Arg<FunctionVO>.Is.Anything)).Return(reNumber);

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
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 1 , Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 3 , Url = "Role/EditRole" , Description = "編輯角色" , Check = false }
            };

            List<FunctionCheckDTO> reFunctionCheckDTO = new List<FunctionCheckDTO>()
            {
                new FunctionCheckDTO(){ FunctionID = 1 , Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" , Check = true },
                new FunctionCheckDTO(){ FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" , Check = true },
                new FunctionCheckDTO(){ FunctionID = 3 , Url = "Role/EditRole" , Description = "編輯角色" , Check = false }
            };

            _roleFunctionEfRepo.Stub(o => o.GetFunctionCheckByRole(Arg<string>.Is.Anything)).Return(reFunctionCheckDTO);

            #endregion

            #region act

            var result = _target.GetFunctionCheckByRole(roleID).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].FunctionID, reFunctionCheckVO[i].FunctionID);
                Assert.AreEqual(result[i].Url, reFunctionCheckVO[i].Url);
                Assert.AreEqual(result[i].Description, reFunctionCheckVO[i].Description);
                Assert.AreEqual(result[i].Check, reFunctionCheckVO[i].Check);
            }

            #endregion
        }

        #endregion

        #region SaveRoleFunctionSetting

        /// <summary>
        /// 角色編輯功能
        /// 儲存勾選功能時的變更
        /// 測試儲存成功
        /// </summary>
        [TestMethod()]
        public void SaveRoleFunctionSettingTest()
        {
            #region arrange (儲存成功)

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>()
            {
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 1 , Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 3 , Url = "Role/EditRole" , Description = "編輯角色" , Check = false }
            };

            int reDeleteResult = 1;

            int reInsertResult = 3;

            string reMessage = string.Empty;

            _roleFunctionEfRepo.Stub(o => o.DeleteRoleFunctionByRoleID(Arg<string>.Is.Anything)).Return(reDeleteResult);

            _roleFunctionEfRepo.Stub(o => o.InsertRoleFunction(Arg<RoleFunctionDTO>.Is.Anything)).Return(reInsertResult);

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
        /// 測試儲存失敗
        /// </summary>
        [TestMethod()]
        public void SaveRoleFunctionSettingTest1()
        {
            #region arrange (儲存失敗)

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>()
            {
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 1 , Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 3 , Url = "Role/EditRole" , Description = "編輯角色" , Check = false }
            };

            int reDeleteResult = -1;

            int reInsertResult = 3;

            string reMessage = "刪除失敗。";

            _roleFunctionEfRepo.Stub(o => o.DeleteRoleFunctionByRoleID(Arg<string>.Is.Anything)).Return(reDeleteResult);

            _roleFunctionEfRepo.Stub(o => o.InsertRoleFunction(Arg<RoleFunctionDTO>.Is.Anything)).Return(reInsertResult);

            #endregion

            #region act

            var result = _target.SaveRoleFunctionSetting(functionCheckVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region ClearRoleFunctionByRoleID

        /// <summary>
        /// 角色編輯功能
        /// 儲存清空功能的變更
        /// 測試清除成功
        /// </summary>
        [TestMethod()]
        public void ClearRoleFunctionByRoleIDTest()
        {
            #region arrange (清除成功)

            string roleID = "1";

            int reDeleteResult = 1;

            string reMessage = string.Empty;

            _roleFunctionEfRepo.Stub(o => o.DeleteRoleFunctionByRoleID(Arg<string>.Is.Anything)).Return(reDeleteResult);

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
        /// 測試清除失敗
        /// </summary>
        [TestMethod()]
        public void ClearRoleFunctionByRoleIDTest1()
        {
            #region arrange (清除失敗)

            string roleID = "1";

            int reDeleteResult = -1;

            string reMessage = "刪除失敗。";

            _roleFunctionEfRepo.Stub(o => o.DeleteRoleFunctionByRoleID(Arg<string>.Is.Anything)).Return(reDeleteResult);

            #endregion

            #region act

            var result = _target.ClearRoleFunctionByRoleID(roleID);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #endregion
    }
}