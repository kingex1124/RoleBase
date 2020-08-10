using LoginServiceObj.Service.Interface;
using LoginVO.VO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Rhino.Mocks;
using RoleBase.Controllers;
using RoleBase.CurrentStatus;
using RoleBaseTests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RoleBase.Controllers.Tests
{
    [TestClass()]
    public class RoleControllerTests
    {
        #region 屬性

        IRoleService _roleService = MockRepository.GenerateStub<IRoleService>();

        RoleController _target;

        #endregion

        #region 建構子

        public RoleControllerTests()
        {
            _target = new RoleController(_roleService);
        }

        #endregion

        #region 測試方法

        #region RoleManagement

        /// <summary>
        /// 進入腳色管理畫面
        /// </summary>
        [TestMethod()]
        public void RoleManagementTest()
        {
            // act
            var result = _target.RoleManagement() as ViewResult;

            // assert
            // 驗證 Action
            Assert.IsTrue(!string.IsNullOrEmpty(result.ViewName) && result.ViewName == "RoleManagement");
        }

        #endregion

        #region RoleAddEditDelete

        /// <summary>
        /// Role新增、修改、刪除畫面
        /// </summary>
        [TestMethod()]
        public void RoleAddEditDeleteTest()
        {
            #region arrange

            List<RoleVO> reRoleVO = new List<RoleVO>()
            {
                new RoleVO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleVO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleVO(){ RoleID = 3 , RoleName = "B" , Description = "B1"}
            };

            _roleService.Stub(o => o.GetRoleData()).Return(reRoleVO);

            #endregion

            #region act
            
            var result = _target.RoleAddEditDelete() as ViewResult;

            #endregion

            #region assert

            // 驗證資料
            for (int i = 0; i < ((List<RoleVO>)result.Model).Count; i++)
            {
                Assert.AreEqual(((List<RoleVO>)result.Model)[i].RoleID, reRoleVO[i].RoleID);
                Assert.AreEqual(((List<RoleVO>)result.Model)[i].RoleName, reRoleVO[i].RoleName);
                Assert.AreEqual(((List<RoleVO>)result.Model)[i].Description, reRoleVO[i].Description);
            }

            #endregion
        }

        #endregion

        #region AddRole

        /// <summary>
        /// 新增Role 
        /// 成功新增
        /// </summary>
        [TestMethod()]
        public void AddRoleTest()
        {
            #region arrange (成功新增)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 傳入新增的腳色
            RoleVO roleVO = new RoleVO() { RoleName = "Admin", Description = "最高權限" };

            // 回傳新增後的腳色
            string reMessage = string.Empty;

            _roleService.Stub(o => o.AddRole(Arg<RoleVO>.Is.Anything)).Return(reMessage);

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act

            var resultData = _target.AddRole(roleVO);

            var result = (RoleVO)(((JsonResult)resultData).Data);

            #endregion

            #region assert
            
            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            // 測試回傳結果
            Assert.AreEqual(result.RoleName, "Admin");
            Assert.AreEqual(result.Description, "最高權限");
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));

            #endregion
        }

        /// <summary>
        /// 測試新增腳色失敗
        /// </summary>
        [TestMethod()]
        public void AddRoleTest1()
        {
            #region arrange (新增失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 傳入新增的腳色
            RoleVO roleVO = new RoleVO() { RoleName = "Admin", Description = "最高權限" };

            // 回傳新增後的腳色
            string reMessage = "新增失敗。";

            _roleService.Stub(o => o.AddRole(Arg<RoleVO>.Is.Anything)).Return(reMessage);

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act

            var resultData = _target.AddRole(roleVO);

            var result = (RoleVO)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            // 測試回傳結果
            Assert.AreEqual(result.RoleName, "Admin");
            Assert.AreEqual(result.Description, "最高權限");
            Assert.AreEqual(result.Message, "新增失敗。");

            #endregion
        }

        #endregion

        #region DeleteRole

        /// <summary>
        /// 測試刪除腳色資料
        /// </summary>
        [TestMethod()]
        public void DeleteRoleTest()
        {
            #region arrange (成功刪除腳色)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            string id = "1";

            string reMessage = string.Empty;

            _roleService.Stub(o => o.DeleteRole(Arg<string>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.DeleteRole(id);

            var result = (string)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 測試刪除腳色失敗
        /// </summary>
        [TestMethod()]
        public void DeleteRoleTest1()
        {
            #region arrange (刪除腳色失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            string id = "1";

            string reMessage = "刪除失敗。";

            _roleService.Stub(o => o.DeleteRole(Arg<string>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.DeleteRole(id);

            var result = (string)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region EditRole

        /// <summary>
        /// 成功編輯腳色
        /// </summary>
        [TestMethod()]
        public void EditRoleTest()
        {
            #region arrange (編輯成功)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            RoleVO roleVO = new RoleVO() { RoleID = 1, RoleName = "Admin", Description = "最高權限" };

            string reMessage = string.Empty;

            _roleService.Stub(o => o.EditRole(Arg<RoleVO>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.EditRole(roleVO);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 測試編輯失敗
        /// </summary>
        [TestMethod()]
        public void EditRoleTest1()
        {
            #region arrange (編輯失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            RoleVO roleVO = new RoleVO() { RoleID = 1, RoleName = "Admin", Description = "最高權限" };

            string reMessage = "編輯失敗。";

            _roleService.Stub(o => o.EditRole(Arg<RoleVO>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.EditRole(roleVO);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region RoleUserEdit

        /// <summary>
        /// 轉到編輯角色使用者關聯的畫面
        /// </summary>
        [TestMethod()]
        public void RoleUserEditTest()
        {
            #region arrange

            List<RoleVO> reRoleVO = new List<RoleVO>()
            {
                new RoleVO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleVO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleVO(){ RoleID = 3 , RoleName = "B" , Description = "B1"}
            };

            _roleService.Stub(o => o.GetRoleData()).Return(reRoleVO);

            #endregion

            #region act

            var result = _target.RoleUserEdit() as ViewResult;

            #endregion

            #region assert

            // 驗證資料
            for (int i = 0; i < ((List<RoleVO>)result.Model).Count; i++)
            {
                Assert.AreEqual(((List<RoleVO>)result.Model)[i].RoleID, reRoleVO[i].RoleID);
                Assert.AreEqual(((List<RoleVO>)result.Model)[i].RoleName, reRoleVO[i].RoleName);
                Assert.AreEqual(((List<RoleVO>)result.Model)[i].Description, reRoleVO[i].Description);
            }
           

            #endregion
        }

        #endregion

        #region GetUserByRole

        /// <summary>
        /// 透過角色ID取得勾選的使用者資料
        /// 編輯角色與使用者的關係
        /// </summary>
        [TestMethod()]
        public void GetUserByRoleTest()
        {
            #region arrange

            string roleID = "1";

            List<UserCheckVO> reUserCheckVO = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1 , UserID = 1 , Check = true , UserName = "kevan" , AccountName = "kevan"},
                new UserCheckVO(){ RoleID = 1 , UserID = 2 , Check = true , UserName = "A" , AccountName = "A"},
                new UserCheckVO(){ RoleID = 1 , UserID = 3 , Check = false , UserName = "B" , AccountName = "B"}
            };

            _roleService.Stub(o => o.GetUserCheckByRole(Arg<string>.Is.Anything)).Return(reUserCheckVO);

            #endregion

            #region act

            var resultData = _target.GetUserByRole(roleID);

            var result = (List<UserCheckVO>)((JsonResult)resultData).Data;

            #endregion

            #region assert
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].RoleID, reUserCheckVO[i].RoleID);
                Assert.AreEqual(result[i].UserID, reUserCheckVO[i].UserID);
                Assert.AreEqual(result[i].Check, reUserCheckVO[i].Check);
                Assert.AreEqual(result[i].UserName, reUserCheckVO[i].UserName);
                Assert.AreEqual(result[i].AccountName, reUserCheckVO[i].AccountName);
            }

            #endregion
        }

        #endregion

        #region SaveRoleUserSetting

        /// <summary>
        /// 測試正常儲存RoleUser設定的變更
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest()
        {
            #region arrange (處理有關聯時的行為 成功)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            List<UserCheckVO> userCheckVO = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1 , UserID = 1 , Check = true , AccountName = "kevan" , UserName = "kevan"}
            };

            string roleID = null;

            string reMessage = string.Empty;

            _roleService.Stub(o => o.SaveRoleUserSetting(Arg<List<UserCheckVO>>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.SaveRoleUserSetting(userCheckVO, roleID);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 測試儲存時
        /// 處理有關聯時的行為 失敗
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest1()
        {
            #region arrange (處理有關聯時的行為 失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            List<UserCheckVO> userCheckVO = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1 , UserID = 1 , Check = true , AccountName = "kevan" , UserName = "kevan"}
            };

            string roleID = null;

            string reMessage = "刪除失敗。";

            _roleService.Stub(o => o.SaveRoleUserSetting(Arg<List<UserCheckVO>>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.SaveRoleUserSetting(userCheckVO, roleID);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 測試儲存時
        /// 處理清空所有check時的行為 成功
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest2()
        {
            #region arrange (處理清空所有check時的行為 成功)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            List<UserCheckVO> userCheckVO = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1 , UserID = 1 , Check = true , AccountName = "kevan" , UserName = "kevan"}
            };

            string roleID = "1";

            string reMessage = string.Empty;

            _roleService.Stub(o => o.ClearRoleUserByRoleID(Arg<string>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.SaveRoleUserSetting(userCheckVO, roleID);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 測試儲存時
        /// 處理清空所有check時的行為 失敗
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest3()
        {
            #region arrange (處理清空所有check時的行為 失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            List<UserCheckVO> userCheckVO = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1 , UserID = 1 , Check = true , AccountName = "kevan" , UserName = "kevan"}
            };

            string roleID = "1";

            string reMessage = "刪除失敗。";

            _roleService.Stub(o => o.ClearRoleUserByRoleID(Arg<string>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.SaveRoleUserSetting(userCheckVO, roleID);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #endregion
    }
}