using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using Login.DAL;
using Login.VO;
using Login.DTO;

namespace Login.BO.Tests
{
    [TestClass()]
    public class LoginEfBOTests
    {
        #region 屬性

        IUserEfRepository _userEfRepo = MockRepository.GenerateStub<IUserEfRepository>();

        IRoleEfRepository _roleEfRepo = MockRepository.GenerateStub<IRoleEfRepository>();

        LoginEfBO _target;

        #endregion

        #region 建構子

        public LoginEfBOTests()
        {
            _target = new LoginEfBO(_userEfRepo, _roleEfRepo);
        }

        #endregion

        #region 測試方法

        #region AccountValid

        /// <summary>
        /// 驗證登入帳號密碼
        /// 驗證成功
        /// </summary>
        [TestMethod()]
        public void AccountValidTest()
        {
            #region arrange (驗證成功)

            // 輸入的帳號
            AccountInfoData accountID = new AccountInfoData() { UserId = 1, AccountName = "kevan", Password = "1qaz@WSX", UserName = "kevan" };

            List<UserDTO> reUserDTOList = new List<UserDTO>()
            {
                new UserDTO(){ UserID = 1, AccountName = "kevan" , Password = "1qaz@WSX" , UserName = "kevan" , Email = "kevan@gmail.com"}
            };

            UserDTO reUserDTO = new UserDTO() { UserID = 1, AccountName = "kevan", Password = "1qaz@WSX", UserName = "kevan", Email = "kevan@gmail.com" };

            string reMessage = accountID.Message;

            _userEfRepo.Stub(o => o.FindAccountName(Arg<string>.Is.Anything)).Return(reUserDTOList);

            _userEfRepo.Stub(o => o.FindAccountData(Arg<string>.Is.Anything)).Return(reUserDTO);

            #endregion

            #region act

            var result = _target.AccountValid(accountID);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reMessage);

            #endregion
        }

        /// <summary>
        /// 驗證登入帳號密碼
        /// 驗證帳號失敗
        /// </summary>
        [TestMethod()]
        public void AccountValidTest1()
        {
            #region arrange (驗證帳號失敗)

            // 輸入的帳號
            AccountInfoData accountID = new AccountInfoData() { UserId = 1, AccountName = "kevan", Password = "1qaz@WSX", UserName = "kevan" };

            List<UserDTO> reUserDTOList = new List<UserDTO>() { };

            string reMessage = "該帳號不存在。";

            _userEfRepo.Stub(o => o.FindAccountName(Arg<string>.Is.Anything)).Return(reUserDTOList);

            #endregion

            #region act

            var result = _target.AccountValid(accountID);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reMessage);

            #endregion
        }

        /// <summary>
        /// 驗證登入帳號密碼
        /// 驗證密碼失敗
        /// </summary>
        [TestMethod()]
        public void AccountValidTest2()
        {
            #region arrange (驗證密碼失敗)

            // 輸入的帳號
            AccountInfoData accountID = new AccountInfoData() { UserId = 1, AccountName = "kevan", Password = "XXXXXX", UserName = "kevan" };

            List<UserDTO> reUserDTOList = new List<UserDTO>()
            {
                new UserDTO(){ UserID = 1, AccountName = "kevan" , Password = "1qaz@WSX" , UserName = "kevan" , Email = "kevan@gmail.com"}
            };

            UserDTO reUserDTO = new UserDTO() { UserID = 1, AccountName = "kevan", Password = "1qaz@WSX", UserName = "kevan", Email = "kevan@gmail.com" };

            string reMessage = "密碼輸入錯誤。";

            _userEfRepo.Stub(o => o.FindAccountName(Arg<string>.Is.Anything)).Return(reUserDTOList);

            _userEfRepo.Stub(o => o.FindAccountData(Arg<string>.Is.Anything)).Return(reUserDTO);

            #endregion

            #region act

            var result = _target.AccountValid(accountID);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reMessage);

            #endregion
        }

        #endregion

        #region GetUserDataByAccountName

        /// <summary>
        /// 透過帳號名稱取得帳號資料
        /// </summary>
        [TestMethod()]
        public void GetUserDataByAccountNameTest()
        {
            #region arrange

            AccountInfoData accountID = new AccountInfoData() { UserId = 1, AccountName = "kevan", Password = "1qaz@WSX", UserName = "kevan" };

            UserDTO reUserDTO = new UserDTO() { UserID = 1, AccountName = "kevan", Password = "1qaz@WSX", UserName = "kevan", Email = "kevan@gmail.com" };

            _userEfRepo.Stub(o => o.FindAccountData(Arg<string>.Is.Anything)).Return(reUserDTO);

            #endregion

            #region act

            var result = _target.GetUserDataByAccountName(accountID);

            #endregion

            #region assert

            Assert.AreEqual(result.UserID, reUserDTO.UserID);
            Assert.AreEqual(result.AccountName, reUserDTO.AccountName);
            Assert.AreEqual(result.Password, reUserDTO.Password);
            Assert.AreEqual(result.UserName, reUserDTO.UserName);
            Assert.AreEqual(result.Email, reUserDTO.Email);

            #endregion
        }

        #endregion

        #region GetRoleDataByUserID

        [TestMethod()]
        public void GetRoleDataByUserIDTest()
        {
            #region arrange

            // 傳入的參數
            string userID = "1";

            List<RoleDTO> reRoleDTOList = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleDTO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleDTO(){ RoleID = 3 , RoleName = "B" , Description = "B1"}
            };

            _roleEfRepo.Stub(o => o.GetRoleDataByAccountName(Arg<string>.Is.Anything)).Return(reRoleDTOList);

            #endregion

            #region act

            var result = _target.GetRoleDataByUserID(userID).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].RoleID, reRoleDTOList[i].RoleID);
                Assert.AreEqual(result[i].RoleName, reRoleDTOList[i].RoleName);
                Assert.AreEqual(result[i].Description, reRoleDTOList[i].Description);
            }

            #endregion
        }

        #endregion

        #endregion
    }
}