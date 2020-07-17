using LoginDTO.DTO;
using LoginServerBO.Service.Interface;
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
using System.Web.Routing;

namespace RoleBase.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        #region 屬性

        IRegistService _registService = MockRepository.GenerateStub<IRegistService>();
        ILoginService _loginService = MockRepository.GenerateStub<ILoginService>();
        ISecurityService _securityService = MockRepository.GenerateStub<ISecurityService>();
        AccountController _target;

        #endregion

        #region 建構子

        public AccountControllerTests()
        {
            _target = new AccountController(_registService, _loginService, _securityService);     
        }

        #endregion

        #region 測試方法

        #region Regist

        /// <summary>
        /// 測試進入註冊畫面
        /// </summary>
        [TestMethod()]
        public void RegistTest()
        {
            // act
            var result = _target.Regist() as ViewResult;

            // assert
            // 驗證 Action
            Assert.IsTrue(!string.IsNullOrEmpty(result.ViewName) && result.ViewName == "Regist");
        }

        #endregion

        #region RegistAccount

        /// <summary>
        /// 測試正常註冊
        /// </summary>
        [TestMethod()]
        public void RegistAccountTest()
        {
            #region arrange (註冊成功)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 傳入參數
            Account account = new Account() 
            {
                UserName = "Kevan",
                Password = "1qaz@WSX",
                PasswordConfirm = "1qaz@WSX",
                AccountName = "Kevan",
                Email = "kevan@gmail.com"
            };

            // 回傳參數
            Account reData = new Account()
            {
                UserName = "Kevan",
                Password = "1qaz@WSX",
                PasswordConfirm = "1qaz@WSX",
                AccountName = "Kevan",
                Email = "kevan@gmail.com"
            };

            // 驗證資料
            _registService.Stub(o => o.RegistValid(Arg<Account>.Is.Anything)).Return(reData);
            // 註冊資料
            _registService.Stub(o => o.Regist(Arg<Account>.Is.Anything)).Return(reData);
            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act

            var resultData = _target.RegistAccount(account);

            var result = (Account)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            // 測試回應狀態
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 200);

            // 測試註冊結果
            Assert.AreEqual(result.UserName, "Kevan");
            Assert.AreEqual(result.Password, "1qaz@WSX");
            Assert.AreEqual(result.PasswordConfirm, "1qaz@WSX");
            Assert.AreEqual(result.AccountName, "Kevan");
            Assert.AreEqual(result.Email, "kevan@gmail.com");

            #endregion
        }

        /// <summary>
        /// 測試密碼確認與密碼輸入不相同的情況
        /// </summary>
        [TestMethod()]
        public void RegistAccountTest1()
        {
            #region arrange (欄位驗證失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 傳入參數
            Account account = new Account()
            {
                UserName = "Kevan",
                Password = "1qaz@WSX",
                PasswordConfirm = "111111",
                AccountName = "Kevan",
                Email = "kevan@gmail.com"
            };

            // 回傳參數
            Account reData = new Account()
            {
                UserName = "Kevan",
                Password = "1qaz@WSX",
                PasswordConfirm = "111111",
                AccountName = "Kevan",
                Email = "kevan@gmail.com",
                Message = "密碼確認與密碼輸入不相同"
            };

            // 驗證資料
            _registService.Stub(o => o.RegistValid(Arg<Account>.Is.Anything)).Return(reData);

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act

            var resultData = _target.RegistAccount(account);

            var result = (Account)(((JsonResult)resultData).Data);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, "密碼確認與密碼輸入不相同");
            Assert.AreEqual(_target.CurrentHttpContext.Response.StatusCode, 400);

            #endregion
        }

        #endregion

        #region Login

        /// <summary>
        /// 測試進入登入畫面
        /// </summary>
        [TestMethod()]
        public void LoginTest()
        {
            // act
            var result = _target.Login() as ViewResult;

            // assert
            Assert.IsTrue(!string.IsNullOrEmpty(result.ViewName) && result.ViewName == "Login");
        }

        /// <summary>
        /// 測試登入成功
        /// </summary>
        [TestMethod()]
        public void LoginTest1()
        {
            #region arrange (登入成功)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 輸入參數
            AccountInfoData accountInfoData = new AccountInfoData()
            {
                AccountName = "kevan",
                Password = "1qaz@WSX"
            };

            // 輸出參數
            AccountInfoData reData = new AccountInfoData()
            {
                AccountName = "kevan",
                Password = "1qaz@WSX"
            };

            // 透過帳號名稱所取得的帳號資訊
            UserDTO reUserDTO = new UserDTO()
            {
                UserID = 1,
                AccountName = "kevan",
                Password = "1qaz@WSX",
                UserName = "kevan",
                Email = "kevan@gmail.com.tw"
            };

            // 透過ID所取得腳色資料包
            List<RoleDTO> reRoleDTOList = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1 , RoleName = "Admin", Description = "最高權限"},
                new RoleDTO(){ RoleID = 2 , RoleName = "A", Description = "A1"},
                new RoleDTO(){ RoleID = 3 , RoleName = "B", Description = "B1"}
            };

            // 透過ID取得該使用者所有的權限資料包
            List<SecurityRoleFunctionDTO> reSRFRole1 = new List<SecurityRoleFunctionDTO>()
            {
                new SecurityRoleFunctionDTO(){  RoleName = "Admin" , Description = "首頁" , Url ="Home/Index"},
                new SecurityRoleFunctionDTO(){  RoleName = "Admin" , Description = "瀏覽角色管理畫面" , Url ="Role/RoleManagement"},
                new SecurityRoleFunctionDTO(){  RoleName = "Admin" , Description = "角色新增修改刪除畫面" , Url ="Role/RoleAddEditDelete"},
                new SecurityRoleFunctionDTO(){  RoleName = "Admin" , Description = "編輯角色" , Url ="Role/EditRole"},
                new SecurityRoleFunctionDTO(){  RoleName = "Admin" , Description = "編輯角色使用者畫面" , Url ="Role/RoleUserEdit"}
            };

            List<SecurityRoleFunctionDTO> reSRFRole2 = new List<SecurityRoleFunctionDTO>()
            {
                new SecurityRoleFunctionDTO(){  RoleName = "A" , Description = "首頁" , Url ="Home/Index"},
                new SecurityRoleFunctionDTO(){  RoleName = "A" , Description = "瀏覽角色管理畫面" , Url ="Role/RoleManagement"},
                new SecurityRoleFunctionDTO(){  RoleName = "A" , Description = "角色新增修改刪除畫面" , Url ="Role/RoleAddEditDelete"},
                new SecurityRoleFunctionDTO(){  RoleName = "A" , Description = "編輯角色" , Url ="Role/EditRole"},
                new SecurityRoleFunctionDTO(){  RoleName = "A" , Description = "編輯角色使用者畫面" , Url ="Role/RoleUserEdit"}
            };

            List<SecurityRoleFunctionDTO> reSRFRole3 = new List<SecurityRoleFunctionDTO>()
            {
                new SecurityRoleFunctionDTO(){  RoleName = "B" , Description = "首頁" , Url ="Home/Index"},
                new SecurityRoleFunctionDTO(){  RoleName = "B" , Description = "瀏覽角色管理畫面" , Url ="Role/RoleManagement"},
                new SecurityRoleFunctionDTO(){  RoleName = "B" , Description = "角色新增修改刪除畫面" , Url ="Role/RoleAddEditDelete"},
                new SecurityRoleFunctionDTO(){  RoleName = "B" , Description = "編輯角色" , Url ="Role/EditRole"},
                new SecurityRoleFunctionDTO(){  RoleName = "B" , Description = "編輯角色使用者畫面" , Url ="Role/RoleUserEdit"}
            };

            List<SecurityRoleFunctionDTO> reSRF = new List<SecurityRoleFunctionDTO>();
            reSRF.AddRange(reSRFRole1);
            reSRF.AddRange(reSRFRole2);
            reSRF.AddRange(reSRFRole3);

            // 驗證使用者帳號密碼
            _loginService.Stub(o => o.AccountValid(Arg<AccountInfoData>.Is.Anything)).Return(reData);

            // 取得帳號資料
            _loginService.Stub(o => o.GetUserDataByAccountName(Arg<AccountInfoData>.Is.Anything)).Return(reUserDTO);

            // 取得腳色資料包
            _loginService.Stub(o => o.GetRoleDataByUserID(Arg<string>.Is.Anything)).Return(reRoleDTOList);

            // 取得功能權限
            _securityService.Stub(o => o.GetSecurityRoleFunction("1")).Return(reSRFRole1);
            _securityService.Stub(o => o.GetSecurityRoleFunction("2")).Return(reSRFRole2);
            _securityService.Stub(o => o.GetSecurityRoleFunction("3")).Return(reSRFRole3);

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act
            
            var result = _target.Login(accountInfoData) as RedirectToRouteResult;

            #endregion

            #region assert
         
            // 驗證 Action
            Assert.IsTrue(string.IsNullOrEmpty(result.RouteValues["action"].ToString()) || result.RouteValues["action"].ToString() == "Index");
           
            // 驗證 Controller
            Assert.IsTrue(string.IsNullOrEmpty(result.RouteValues["controller"].ToString()) || result.RouteValues["controller"].ToString() == "Home");

            // 取得 Session 並驗證
            var sessionInfo = _target.CurrentHttpContext.Session["LoginInfo"] as SecurityLevel;

            // 驗證權限資料
            for (int i = 0; i < sessionInfo.SecurityRole.Count; i++)
            {
                Assert.AreEqual(sessionInfo.SecurityRole[i].RoleID, reRoleDTOList[i].RoleID);
                Assert.AreEqual(sessionInfo.SecurityRole[i].RoleName, reRoleDTOList[i].RoleName);
                Assert.AreEqual(sessionInfo.SecurityRole[i].Description, reRoleDTOList[i].Description);
            }

            for (int i = 0; i < sessionInfo.SecurityUrl.Count; i++)
            {
                Assert.AreEqual(sessionInfo.SecurityUrl[i].RoleName, reSRF[i].RoleName);
                Assert.AreEqual(sessionInfo.SecurityUrl[i].Url, reSRF[i].Url);
                Assert.AreEqual(sessionInfo.SecurityUrl[i].Description, reSRF[i].Description);
            }
           
            Assert.AreEqual(sessionInfo.UserData.UserId , 1);
            Assert.AreEqual(sessionInfo.UserData.AccountName, "kevan");

            Assert.AreEqual(_target.CurrentHttpContext.Session["UserName"], "kevan");

            #endregion
        }

        /// <summary>
        /// 測試登入帳號不存在
        /// </summary>
        [TestMethod()]
        public void LoginTest2()
        {
            #region arrange (登入失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 輸入參數 登入資訊
            AccountInfoData accountInfoData = new AccountInfoData()
            {
                AccountName = "Jon",
                Password = "1qaz@WSX"
            };

            // 輸出參數 驗證後結果
            AccountInfoData reData = new AccountInfoData()
            {
                AccountName = "Jon",
                Password = "1qaz@WSX",
                Message = "該帳號不存在。"
            };
      
            // 驗證帳號密碼
            _loginService.Stub(o => o.AccountValid(Arg<AccountInfoData>.Is.Anything)).Return(reData);
     
            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act

            var result = _target.Login(accountInfoData) as ViewResult;

            #endregion

            #region assert

            // 驗證資料
            Assert.AreEqual(((AccountInfoData)result.Model).AccountName, reData.AccountName);
            Assert.AreEqual(((AccountInfoData)result.Model).Password, reData.Password);
            Assert.AreEqual(((AccountInfoData)result.Model).Message, reData.Message);

            #endregion
        }

        /// <summary>
        /// 測試登入密碼失敗
        /// </summary>
        [TestMethod()]
        public void LoginTest3()
        {
            #region arrange (登入失敗)

            // httpContext物件設定
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();
            httpContext.Response.StatusCode = 200;

            // 輸入參數
            AccountInfoData accountInfoData = new AccountInfoData()
            {
                AccountName = "kevan",
                Password = "111111"
            };

            // 輸出參數
            AccountInfoData reData = new AccountInfoData()
            {
                AccountName = "kevan",
                Password = "111111",
                Message = "密碼輸入錯誤。"
            };

            _loginService.Stub(o => o.AccountValid(Arg<AccountInfoData>.Is.Anything)).Return(reData);

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            #endregion

            #region act
            
            var result = _target.Login(accountInfoData) as ViewResult;

            #endregion

            #region assert

            // 驗證資料
            Assert.AreEqual(((AccountInfoData)result.Model).AccountName, reData.AccountName);
            Assert.AreEqual(((AccountInfoData)result.Model).Password, reData.Password);
            Assert.AreEqual(((AccountInfoData)result.Model).Message, reData.Message);

            #endregion
        }

        #endregion

        #region Logout

        [TestMethod()]
        public void LogoutTest()
        {
            // arrange
            var httpContext = FakeHttpContextManager.CreateHttpContextBase();

            // 設定httpContext
            _target.CurrentHttpContext = httpContext;

            // act
            var result = _target.Logout() as RedirectToRouteResult;

            // assert
            Assert.IsTrue(!string.IsNullOrEmpty(result.RouteValues["action"].ToString()) && result.RouteValues["action"].ToString() == "Login");

            Assert.IsTrue(!string.IsNullOrEmpty(result.RouteValues["controller"].ToString()) && result.RouteValues["controller"].ToString() == "Account");
        }

        #endregion

        #endregion

    }
}