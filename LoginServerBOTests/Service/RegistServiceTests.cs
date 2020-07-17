﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginServerBO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginServerBO.BO.Interface;
using Rhino.Mocks;
using LoginVO.VO;
using LoginDTO.DTO;
using System.IO;

namespace LoginServerBO.Service.Tests
{
    [TestClass()]
    public class RegistServiceTests
    {
        #region 屬性

        IRegistBO _registBO = MockRepository.GenerateStub<IRegistBO>();
        RegistService _target;

        #endregion

        #region 建構子

        public RegistServiceTests()
        {
            _target = new RegistService(_registBO);
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
            Account reAccount = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com" };

            string reMessage = account.Message;

            _registBO.Stub(o => o.RegistValid(Arg<Account>.Is.Anything)).Return(reAccount);

            #endregion

            #region act

            var result = _target.RegistValid(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reAccount.Message);

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
            Account reAccount = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com" , Message = "帳號名稱已被使用" };

            _registBO.Stub(o => o.RegistValid(Arg<Account>.Is.Anything)).Return(reAccount);

            #endregion

            #region act

            var result = _target.RegistValid(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reAccount.Message);

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
            Account reAccount = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com", Message = "密碼確認與密碼輸入不相同" };

            _registBO.Stub(o => o.RegistValid(Arg<Account>.Is.Anything)).Return(reAccount);

            #endregion

            #region act

            var result = _target.RegistValid(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reAccount.Message);

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

            Account reAccount = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com" };
          
            _registBO.Stub(o => o.Regist(Arg<Account>.Is.Anything)).Return(reAccount);

            #endregion

            #region act

            var result = _target.Regist(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reAccount.Message);

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

            Account reAccount = new Account() { AccountName = "kevan", UserName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com" , Message = "註冊失敗" };

            _registBO.Stub(o => o.Regist(Arg<Account>.Is.Anything)).Return(reAccount);

            #endregion

            #region act

            var result = _target.Regist(account);

            #endregion

            #region assert

            Assert.AreEqual(result.Message, reAccount.Message);

            #endregion
        }

        #endregion

        #endregion

    }
}