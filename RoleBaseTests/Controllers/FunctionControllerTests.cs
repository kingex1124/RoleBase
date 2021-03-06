﻿using Login.DTO;
using Login.Service;
using Login.VO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using RoleBase.Controllers;
using RoleBaseTests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RoleBase.Controllers.Tests
{
    [TestClass()]
    public class FunctionControllerTests
    {
        #region 屬性

        IFunctionService _functionService = MockRepository.GenerateStub<IFunctionService>();
        IRoleService _roleService = MockRepository.GenerateStub<IRoleService>();
        ILoginService _loginService = MockRepository.GenerateStub<ILoginService>();
        ISecurityService _securityService = MockRepository.GenerateStub<ISecurityService>();

        FunctionController _target;

        #endregion

        #region 建構子

        public FunctionControllerTests()
        {
            _target = new FunctionController(_functionService, _roleService, _loginService, _securityService);
        }

        #endregion

        #region 測試方法

        #region FunctionManagement

        /// <summary>
        /// Function管理介面
        /// </summary>
        [TestMethod()]
        public void FunctionManagementTest()
        {
            #region act

            var result = _target.FunctionManagement() as ViewResult;

            #endregion

            #region assert

            Assert.IsTrue(!string.IsNullOrEmpty(result.ViewName) && result.ViewName == "FunctionManagement");

            #endregion
        }

        #endregion

        #region FunctionAddEditDelete

        /// <summary>
        /// Function新增、修改、刪除畫面
        /// </summary>
        [TestMethod()]
        public void FunctionAddEditDeleteTest()
        {
            // act
            var result = _target.FunctionAddEditDelete() as ViewResult;

            // assert
            // 驗證 Action
            Assert.IsTrue(!string.IsNullOrEmpty(result.ViewName) && result.ViewName == "FunctionAddEditDelete");
        }

        #endregion

        #region QueryFunction

        /// <summary>
        /// 查詢功能
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public void QueryFunctionTest()
        {
            #region arrange

            List<FunctionVO> reFunctionList = new List<FunctionVO>()
            {
                new FunctionVO(){ FunctionID = 1 , Url="Role/RoleManagement" , Title = "角色管理" , Description = "瀏覽角色管理畫面" , IsMenu = true , Parent = 0 , ParentName = "No" },
                new FunctionVO(){ FunctionID = 2 , Url="Role/RoleAddEditDelete" , Title = "編輯角色" , Description = "角色新增修改刪除畫面" , IsMenu = true , Parent = 1 , ParentName = "角色管理" },
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

            _functionService.Stub(o => o.GetFunctionData(pageDataVO)).Return(reFunctionList);

            #endregion

            #region act

            var resultData = _target.QueryFunction(pageDataVO);

            var result = (FunctionTableResultVO)((JsonResult)resultData).Data;

            var roleData = result.FunctionData.ToList();

            #endregion

            #region assert

            for (int i = 0; i < roleData.Count; i++)
            {
                Assert.AreEqual(roleData[i].FunctionID, reFunctionList[i].FunctionID);
                Assert.AreEqual(roleData[i].Url, reFunctionList[i].Url);
                Assert.AreEqual(roleData[i].Title, reFunctionList[i].Title);
                Assert.AreEqual(roleData[i].Description, reFunctionList[i].Description);
                Assert.AreEqual(roleData[i].IsMenu, reFunctionList[i].IsMenu);
                Assert.AreEqual(roleData[i].Parent, reFunctionList[i].Parent);
                Assert.AreEqual(roleData[i].ParentName, reFunctionList[i].ParentName);
            }

            #endregion
        }

        #endregion

        #region FunctionGetParentData

        /// <summary>
        /// 取得作為上層的keyValue資料
        /// </summary>
        [TestMethod()]
        public void FunctionGetParentDataTest()
        {
            #region arrange

            List<KeyValuePairVO> reKeyValuePairVO = new List<KeyValuePairVO>()
            {
                new KeyValuePairVO(){ Key = 1 , Value = "角色管理"},
                new KeyValuePairVO(){ Key = 2 , Value = "編輯角色"},
                new KeyValuePairVO(){ Key = 3 , Value = "新增"},
            };

            _functionService.Stub(o => o.GetParentKeyValue()).Return(reKeyValuePairVO);

            #endregion

            #region act

            var resultData = _target.FunctionGetParentData();

            var result = (List<KeyValuePairVO>)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].Key, reKeyValuePairVO[i].Key);
                Assert.AreEqual(result[i].Value, reKeyValuePairVO[i].Value);
            }

            #endregion
        }

        #endregion

        #region AddFunction

        /// <summary>
        /// 新增Function
        /// 測試新增成功
        /// </summary>
        [TestMethod()]
        public void AddFunctionTest()
        {
            #region arrange (新增成功)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            FunctionVO functionVO = new FunctionVO() { Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            string reMessage = "";

            _functionService.Stub(o => o.AddFunction(Arg<FunctionVO>.Is.Anything)).Return(reMessage);

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act

            var resultData = _target.AddFunction(functionVO);

            var result = (FunctionVO)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            // 測試回傳結果
            Assert.AreEqual(result.Url, functionVO.Url);
            Assert.AreEqual(result.Title, functionVO.Title);
            Assert.AreEqual(result.Description, functionVO.Description);
            Assert.AreEqual(result.IsMenu, functionVO.IsMenu);
            Assert.AreEqual(result.Parent, functionVO.Parent);
            Assert.AreEqual(result.ParentName, functionVO.ParentName);
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));

            #endregion
        }

        /// <summary>
        /// 新增Function
        /// 測試新增失敗
        /// </summary>
        [TestMethod()]
        public void AddFunctionTest1()
        {
            #region arrange (新增失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            FunctionVO functionVO = new FunctionVO() { Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            string reMessage = "新增失敗。";

            _functionService.Stub(o => o.AddFunction(Arg<FunctionVO>.Is.Anything)).Return(reMessage);

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act

            var resultData = _target.AddFunction(functionVO);

            var result = (FunctionVO)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            // 測試回傳結果
            Assert.AreEqual(result.Url, functionVO.Url);
            Assert.AreEqual(result.Title, functionVO.Title);
            Assert.AreEqual(result.Description, functionVO.Description);
            Assert.AreEqual(result.IsMenu, functionVO.IsMenu);
            Assert.AreEqual(result.Parent, functionVO.Parent);
            Assert.AreEqual(result.ParentName, functionVO.ParentName);
            Assert.AreEqual(result.Message, reMessage);

            #endregion
        }

        #endregion

        #region DeleteFunction

        /// <summary>
        /// 刪除Function
        /// 測試刪除成功
        /// </summary>
        [TestMethod()]
        public void DeleteFunctionTest()
        {
            #region arrange (刪除成功)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            string id = "1";

            string reMessage = "";

            _functionService.Stub(o => o.DeleteFunction(Arg<string>.Is.Anything)).Return(reMessage);

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act

            var resultData = _target.DeleteFunction(id);

            var result = (string)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 刪除Function
        /// 測試刪除失敗
        /// </summary>
        [TestMethod()]
        public void DeleteFunctionTest1()
        {
            #region arrange (刪除失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            string id = "1";

            string reMessage = "刪除失敗。";

            _functionService.Stub(o => o.DeleteFunction(Arg<string>.Is.Anything)).Return(reMessage);

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act

            var resultData = _target.DeleteFunction(id);

            var result = (string)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region EditFunction

        /// <summary>
        /// 編輯Function
        /// 測試編輯成功
        /// </summary>
        [TestMethod()]
        public void EditFunctionTest()
        {
            #region arrange (編輯成功)

            List<RoleDTO> reRoleDTO = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1, RoleName = "Admin" , Description = "最高權限" },
                new RoleDTO(){ RoleID = 2, RoleName = "A" , Description = "A1" },
                new RoleDTO(){ RoleID = 3, RoleName = "B" , Description = "B1" },
            };

            List<SecurityRoleFunctionDTO> reSecurityRoleFunctionDTO = new List<SecurityRoleFunctionDTO>()
            {
                new SecurityRoleFunctionDTO(){ Url ="Role/RoleManagement" ,Description = "瀏覽角色管理畫面"},
                new SecurityRoleFunctionDTO(){ Url ="Role/RoleAddEditDelete" ,Description = "角色新增修改刪除畫面"},
                new SecurityRoleFunctionDTO(){ Url ="Role/EditRole" ,Description = "編輯角色"}
            };

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            httpContext.Session["UserID"] = 1;

            httpContext.Session["AccountName"] = "kevan";

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            FunctionVO functionVO = new FunctionVO() { FunctionID = 1, Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            string reMessage = string.Empty;

            _loginService.Stub(o => o.GetRoleDataByUserID(Arg<string>.Is.Anything)).Return(reRoleDTO);

            _securityService.Stub(o => o.GetSecurityRoleFunction(Arg<string>.Is.Anything)).Return(reSecurityRoleFunctionDTO);

            _functionService.Stub(o => o.EditFunction(Arg<FunctionVO>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.EditFunction(functionVO);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 編輯Function
        /// 測試編輯失敗
        /// </summary>
        [TestMethod()]
        public void EditFunctionTest1()
        {
            #region arrange (編輯成功)

            List<RoleDTO> reRoleDTO = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1, RoleName = "Admin" , Description = "最高權限" },
                new RoleDTO(){ RoleID = 2, RoleName = "A" , Description = "A1" },
                new RoleDTO(){ RoleID = 3, RoleName = "B" , Description = "B1" },
            };

            List<SecurityRoleFunctionDTO> reSecurityRoleFunctionDTO = new List<SecurityRoleFunctionDTO>()
            {
                new SecurityRoleFunctionDTO(){ Url ="Role/RoleManagement" ,Description = "瀏覽角色管理畫面"},
                new SecurityRoleFunctionDTO(){ Url ="Role/RoleAddEditDelete" ,Description = "角色新增修改刪除畫面"},
                new SecurityRoleFunctionDTO(){ Url ="Role/EditRole" ,Description = "編輯角色"}
            };

            // httpContext物件失敗
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            httpContext.Session["UserID"] = 1;

            httpContext.Session["AccountName"] = "kevan";

            FunctionVO functionVO = new FunctionVO() { FunctionID = 1, Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            string reMessage = "編輯失敗";

            _loginService.Stub(o => o.GetRoleDataByUserID(Arg<string>.Is.Anything)).Return(reRoleDTO);

            _securityService.Stub(o => o.GetSecurityRoleFunction(Arg<string>.Is.Anything)).Return(reSecurityRoleFunctionDTO);

            _functionService.Stub(o => o.EditFunction(Arg<FunctionVO>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.EditFunction(functionVO);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region RoleFunctionEdit

        /// <summary>
        /// 轉到編輯角色功能關聯的畫面
        /// </summary>
        [TestMethod()]
        public void RoleFunctionEditTest()
        {
            // act
            var result = _target.RoleFunctionEdit() as ViewResult;

            // assert
            // 驗證 Action
            Assert.IsTrue(!string.IsNullOrEmpty(result.ViewName) && result.ViewName == "RoleFunctionEdit");
        }

        #endregion

        #region QueryRoleFunctionEditRole

        /// <summary>
        /// 查詢腳色資料
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public void QueryRoleFunctionEditRoleTest()
        {
            #region arrange

            List<RoleVO> reRoleVOList = new List<RoleVO>()
            {
                new RoleVO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleVO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleVO(){ RoleID = 3 , RoleName = "B" , Description = "B1"}
            };

            PageDataVO pageDataVO = new PageDataVO()
            {
                PageNumber = 1,
                WhereCondition = new List<KeyValueVO>()
                   {
                        new KeyValueVO()
                        {
                             Key = "RoleName",
                             Value = ""
                        }
                   }
            };

            _roleService.Stub(o => o.GetRoleData(pageDataVO)).Return(reRoleVOList);

            #endregion

            #region act

            var resultData = _target.QueryRoleFunctionEditRole(pageDataVO);

            var result = (RoleTableResultVO)((JsonResult)resultData).Data;

            var roleData = result.RoleData.ToList();

            #endregion

            #region assert

            // 驗證資料
            for (int i = 0; i < roleData.Count; i++)
            {
                Assert.AreEqual(roleData[i].RoleID, reRoleVOList[i].RoleID);
                Assert.AreEqual(roleData[i].RoleName, reRoleVOList[i].RoleName);
                Assert.AreEqual(roleData[i].Description, reRoleVOList[i].Description);
            }

            #endregion
        }

        #endregion

        #region GetFunctionByRole

        /// <summary>
        /// 透過角色ID取得勾選的功能資料
        /// 編輯角色與功能的關係
        /// </summary>
        [TestMethod()]
        public void GetFunctionByRoleTest()
        {
            #region arrange

            string id = "1";

            List<FunctionCheckVO> reFunctionCheckVO = new List<FunctionCheckVO>()
            {
                new FunctionCheckVO(){ FunctionID = 1 , Url = "Role/RoleManagement" , Title = "角色管理", Description = "瀏覽角色管理畫面" , IsMenu = true, ParentName = "No", Check = true },
                new FunctionCheckVO(){ FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Title = "編輯角色", Description = "角色新增修改刪除畫面" , IsMenu = true, ParentName = "角色管理" , Check = true },
                new FunctionCheckVO(){ FunctionID = 3 , Url = "Role/EditRole", Title = "編輯" , Description = "編輯角色" , IsMenu = false, ParentName = "Not Menu" , Check = false }
            };

            PageDataVO pageDataVO = new PageDataVO() { OrderByColumn = "UserID", OrderByType = "ASC" };

            _functionService.Stub(o => o.GetFunctionCheckByRole(Arg<string>.Is.Anything, Arg<PageDataVO>.Is.Anything)).Return(reFunctionCheckVO);

            #endregion

            #region act

            var resultData = _target.GetFunctionByRole(id, pageDataVO);

            var result = (List<FunctionCheckVO>)((JsonResult)resultData).Data;

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
        /// 儲存RoleFunction設定的變更
        /// </summary>
        [TestMethod()]
        public void SaveRoleFunctionSettingTest()
        {
            #region arrange (處理有關選時的行為 成功)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>()
            {
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 1 , Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 3 , Url = "Role/EditRole" , Description = "編輯角色" , Check = false }
            };

            string roleID = null;

            string reMessage = string.Empty;

            _functionService.Stub(o => o.SaveRoleFunctionSetting(Arg<List<FunctionCheckVO>>.Is.Anything)).Return(reMessage);

            SessionReflashSetting();

            #endregion

            #region act

            var resultData = _target.SaveRoleFunctionSetting(functionCheckVO, roleID);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 儲存RoleFunction設定的變更
        /// </summary>
        [TestMethod()]
        public void SaveRoleFunctionSettingTest1()
        {
            #region arrange (處理有關選時的行為 失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>()
            {
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 1 , Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" , Check = true },
                new FunctionCheckVO(){ RoleID = 1 , FunctionID = 3 , Url = "Role/EditRole" , Description = "編輯角色" , Check = false }
            };

            string roleID = null;

            string reMessage = "刪除失敗。";

            _functionService.Stub(o => o.SaveRoleFunctionSetting(Arg<List<FunctionCheckVO>>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.SaveRoleFunctionSetting(functionCheckVO, roleID);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 儲存RoleFunction設定的變更
        /// </summary>
        [TestMethod()]
        public void SaveRoleFunctionSettingTest2()
        {
            #region arrange (處理清空所有check時的行為 成功)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>() { };

            string roleID = "1";

            string reMessage = string.Empty;

            _functionService.Stub(o => o.ClearRoleFunctionByRoleID(Arg<string>.Is.Anything)).Return(reMessage);

            SessionReflashSetting();

            #endregion

            #region act

            var resultData = _target.SaveRoleFunctionSetting(functionCheckVO, roleID);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 儲存RoleFunction設定的變更
        /// </summary>
        [TestMethod()]
        public void SaveRoleFunctionSettingTest3()
        {
            #region arrange (處理清空所有check時的行為 失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            List<FunctionCheckVO> functionCheckVO = new List<FunctionCheckVO>() { };

            string roleID = "1";

            string reMessage = "刪除失敗。";

            _functionService.Stub(o => o.ClearRoleFunctionByRoleID(Arg<string>.Is.Anything)).Return(reMessage);

            #endregion

            #region act

            var resultData = _target.SaveRoleFunctionSetting(functionCheckVO, roleID);

            var result = (string)((JsonResult)resultData).Data;

            #endregion

            #region assert

            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region GetFunctionMenuTest

        /// <summary>
        /// 取得選單資料
        /// </summary>
        [TestMethod()]
        public void GetFunctionMenuTest()
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

            _functionService.Stub(o => o.GetFunctionNode(Arg<string>.Is.Anything)).Return(reFunctionMenuNodeList);

            #endregion

            #region act

            var resultData = _target.GetFunctionMenu(userID);

            var result = (List<FunctionMenuNode>)((JsonResult)resultData).Data;

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

        #region SessionReflashTest

        [TestMethod()]
        public void SessionReflashTest()
        {
            #region arrange 

            List<RoleDTO> reRoleDTO = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleDTO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleDTO(){ RoleID = 3 , RoleName = "B" , Description = "B1"}
            };

            List<SecurityRoleFunctionDTO> reSRFRole = new List<SecurityRoleFunctionDTO>()
            {
                new SecurityRoleFunctionDTO(){  Description = "首頁" , Url ="Home/Index"},
                new SecurityRoleFunctionDTO(){  Description = "瀏覽角色管理畫面" , Url ="Role/RoleManagement"},
                new SecurityRoleFunctionDTO(){  Description = "角色新增修改刪除畫面" , Url ="Role/RoleAddEditDelete"},
                new SecurityRoleFunctionDTO(){  Description = "編輯角色" , Url ="Role/EditRole"},
                new SecurityRoleFunctionDTO(){  Description = "編輯角色使用者畫面" , Url ="Role/RoleUserEdit"}
            };

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Session["UserID"] = 1;
            httpContext.Session["AccountName"] = "kevan";

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            _loginService.Stub(o => o.GetRoleDataByUserID(Arg<string>.Is.Anything)).Return(reRoleDTO);

            _securityService.Stub(o => o.GetSecurityRoleFunction(Arg<string>.Is.Anything)).Return(reSRFRole);

            #endregion

            #region act

            _target.SessionReflash();

            #endregion

            #region assert

            for (int i = 0; i < _target.CurrentSecurityLevel.SecurityRole.Count; i++)
            {
                Assert.AreEqual(_target.CurrentSecurityLevel.SecurityRole[i].RoleID, reRoleDTO[i].RoleID);
                Assert.AreEqual(_target.CurrentSecurityLevel.SecurityRole[i].RoleName, reRoleDTO[i].RoleName);
                Assert.AreEqual(_target.CurrentSecurityLevel.SecurityRole[i].Description, reRoleDTO[i].Description);
            }

            for (int i = 0; i < _target.CurrentSecurityLevel.SecurityUrl.Count; i++)
            {
                Assert.AreEqual(_target.CurrentSecurityLevel.SecurityUrl[i].Url, reSRFRole[i].Url);
                Assert.AreEqual(_target.CurrentSecurityLevel.SecurityUrl[i].Description, reSRFRole[i].Description);
            }

            #endregion
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 測通刷新方法時所用的
        /// </summary>
        private void SessionReflashSetting()
        {
            #region arrange 

            List<RoleDTO> reRoleDTO = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleDTO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleDTO(){ RoleID = 3 , RoleName = "B" , Description = "B1"}
            };

            List<SecurityRoleFunctionDTO> reSRFRole = new List<SecurityRoleFunctionDTO>()
            {
                new SecurityRoleFunctionDTO(){  Description = "首頁" , Url ="Home/Index"},
                new SecurityRoleFunctionDTO(){  Description = "瀏覽角色管理畫面" , Url ="Role/RoleManagement"},
                new SecurityRoleFunctionDTO(){  Description = "角色新增修改刪除畫面" , Url ="Role/RoleAddEditDelete"},
                new SecurityRoleFunctionDTO(){  Description = "編輯角色" , Url ="Role/EditRole"},
                new SecurityRoleFunctionDTO(){  Description = "編輯角色使用者畫面" , Url ="Role/RoleUserEdit"}
            };

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Session["UserID"] = 1;
            httpContext.Session["AccountName"] = "kevan";

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            _loginService.Stub(o => o.GetRoleDataByUserID(Arg<string>.Is.Anything)).Return(reRoleDTO);

            _securityService.Stub(o => o.GetSecurityRoleFunction(Arg<string>.Is.Anything)).Return(reSRFRole);

            #endregion
        }


        #endregion
    }
}