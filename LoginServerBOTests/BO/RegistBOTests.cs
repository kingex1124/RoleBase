using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginServerBO.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginServerBO.Repository.Interface;
using Rhino.Mocks;
using LoginVO.VO;
using LoginDTO.DTO;

namespace LoginServerBO.BO.Tests
{
    [TestClass()]
    public class RegistBOTests
    {
        #region 屬性

        IUserRepository _userRep = MockRepository.GenerateStub<IUserRepository>();
        RegistBO _target;

        #endregion

        #region 建構子

        public RegistBOTests()
        {
            _target = new RegistBO(_userRep);
        }

        #endregion

        #region 測試方法

        #region RegistValid

        /// <summary>
        /// 驗證帳號密碼
        /// 驗證帳號不存在 密碼正確 成功
        /// </summary>
        [TestMethod()]
        public void RegistValidTest()
        {
            #region arrange (驗證帳號不存在 密碼正確 成功)

            // 傳入的參數
            Account account = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com" };

            // 回傳的參數
            List<UserDTO> reUserDTOList = new List<UserDTO>() { };

            string reMessage = account.Message;

            _userRep.Stub(o => o.FindAccountName(Arg<string>.Is.Anything)).Return(reUserDTOList);

            #endregion

            #region act

            var result = _target.RegistValid(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reMessage);

            #endregion
        }

        /// <summary>
        /// 驗證帳號密碼
        /// 驗證帳號存在 失敗
        /// </summary>
        [TestMethod()]
        public void RegistValidTest1()
        {
            #region arrange (驗證帳號存在 失敗)

            // 傳入的參數
            Account account = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com" };

            // 回傳的參數
            List<UserDTO> reUserDTOList = new List<UserDTO>()
            {
                new UserDTO() { UserID = 1, AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", Email = "kevan@gmail.com" }
            };

            string reMessage = "帳號名稱已被使用";

            _userRep.Stub(o => o.FindAccountName(Arg<string>.Is.Anything)).Return(reUserDTOList);

            #endregion

            #region act

            var result = _target.RegistValid(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reMessage);

            #endregion
        }

        /// <summary>
        /// 驗證帳號密碼
        /// 驗證帳號不存在 密碼錯誤 失敗
        /// </summary>
        [TestMethod()]
        public void RegistValidTest2()
        {
            #region arrange (驗證帳號不存在 密碼錯誤 失敗)

            // 傳入的參數
            Account account = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "xxxxxx", Email = "kevan@gmail.com" };

            // 回傳的參數
            List<UserDTO> reUserDTOList = new List<UserDTO>()
            {

            };

            string reMessage = "密碼確認與密碼輸入不相同";

            _userRep.Stub(o => o.FindAccountName(Arg<string>.Is.Anything)).Return(reUserDTOList);

            #endregion

            #region act

            var result = _target.RegistValid(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reMessage);

            #endregion
        }

        #endregion

        #region Regist

        /// <summary>
        /// 註冊帳號 
        /// 註冊成功
        /// </summary>
        [TestMethod()]
        public void RegistTest()
        {
            #region arrange (註冊成功)

            // 傳入的參數
            Account account = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com" };

            int reNumber = 1;

            string reMessage = account.Message;

            _userRep.Stub(o => o.UserInsert(Arg<Account>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.Regist(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reMessage);

            #endregion
        }

        /// <summary>
        /// 註冊帳號
        /// 註冊失敗
        /// </summary>
        [TestMethod()]
        public void RegistTest1()
        {
            #region arrange (註冊失敗)

            // 傳入的參數
            Account account = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com" };

            int reNumber = -1;

            string reMessage = "註冊失敗";

            _userRep.Stub(o => o.UserInsert(Arg<Account>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.Regist(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reMessage);

            #endregion
        }

        #endregion

        #endregion
    }
}