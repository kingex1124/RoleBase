using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginServerBO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KevanFramework.DataAccessDAL.Interface;
using Rhino.Mocks;
using LoginDTO.DTO;
using LoginVO.VO;

namespace LoginServerBO.Repository.Tests
{
    [TestClass()]
    public class UserRepositoryTests
    {
        #region 屬性

        private IDataAccess _dataAccess = MockRepository.GenerateStub<IDataAccess>();
        UserRepository _target;

        #endregion

        #region 建構子

        public UserRepositoryTests()
        {
            _target = new UserRepository(_dataAccess);
        }

        #endregion

        #region 測試方法

        #region FindAccountName

        /// <summary>
        /// 查找帳號
        /// 驗證帳號是否重複
        /// </summary>
        [TestMethod()]
        public void FindAccountNameTest()
        {
            #region arrange

            string accountName = "kevan";

            List<UserDTO> userDTOList = new List<UserDTO>()
            {
                new UserDTO(){ UserID = 1 , AccountName = "kevan" , UserName = "kevan" , Password = "1qaz@WSX"}
            };

            _dataAccess.Stub(o => o.QueryDataTable<UserDTO>(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(userDTOList);

            #endregion

            #region act

            var result = _target.FindAccountName(accountName).ToList();

            #endregion

            #region assert

            for (int i = 0; i < userDTOList.Count; i++)
            {
                Assert.AreEqual(result[i].UserID, userDTOList[i].UserID);
                Assert.AreEqual(result[i].AccountName, userDTOList[i].AccountName);
                Assert.AreEqual(result[i].UserName, userDTOList[i].UserName);
                Assert.AreEqual(result[i].Password, userDTOList[i].Password);
            }

            #endregion
        }

        #endregion

        #region FindAccountData

        /// <summary>
        /// 取得該帳號資料
        /// </summary>
        [TestMethod()]
        public void FindAccountDataTest()
        {
            #region arrange

            string accountName = "kevan";

            List<UserDTO> userDTOList = new List<UserDTO>()
            {
                new UserDTO(){ UserID = 1 , AccountName = "kevan" , UserName = "kevan" , Password = "1qaz@WSX"}
            };

            _dataAccess.Stub(o => o.QueryDataTable<UserDTO>(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(userDTOList);

            #endregion

            #region act

            var result = _target.FindAccountData(accountName);

            #endregion

            #region assert

            Assert.AreEqual(result.UserID, userDTOList[0].UserID);
            Assert.AreEqual(result.AccountName, userDTOList[0].AccountName);
            Assert.AreEqual(result.UserName, userDTOList[0].UserName);

            #endregion
        }

        #endregion

        #region UserInsert

        /// <summary>
        /// 新增帳號
        /// </summary>
        [TestMethod()]
        public void UserInsertTest()
        {
            #region arrange

            Account account = new Account() { UserName = "kevan", AccountName = "kevan", Password = "1qaz@WSX", PasswordConfirm = "1qaz@WSX", Email = "kevan@gmail.com" };

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.UserInsert(account);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #endregion
    }
}