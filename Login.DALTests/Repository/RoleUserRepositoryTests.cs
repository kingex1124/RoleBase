using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using System.Data.SqlClient;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Login.DTO;

namespace Login.DAL.Tests
{
    [TestClass()]
    public class RoleUserRepositoryTests
    {
        #region 屬性

        IDataAccess _dataAccess = MockRepository.GenerateStub<IDataAccess>();
        RoleUserRepository _target;

        #endregion

        #region 建構子

        public RoleUserRepositoryTests()
        {
            _target = new RoleUserRepository(_dataAccess);
        }

        #endregion

        #region 測試方法

        #region GetUserCheckByRole

        /// <summary>
        /// 取得角色設定使用者的資料
        /// </summary>
        [TestMethod()]
        public void GetUserCheckByRoleTest()
        {
            #region arrange

            string id = "1";

            List<UserCheckDTO> reUserCheckDTO = new List<UserCheckDTO>()
            {
                new UserCheckDTO(){ UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckDTO(){ UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckDTO(){ UserID = 3 , UserName = "B" , AccountName = "B1" , Check = false }
            };

            _dataAccess.Stub(o => o.QueryDataTable<UserCheckDTO>(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reUserCheckDTO);

            #endregion

            #region act

            var result = _target.GetUserCheckByRole(id).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].UserID, reUserCheckDTO[i].UserID);
                Assert.AreEqual(result[i].UserName, reUserCheckDTO[i].UserName);
                Assert.AreEqual(result[i].AccountName, reUserCheckDTO[i].AccountName);
                Assert.AreEqual(result[i].Check, reUserCheckDTO[i].Check);
            }

            #endregion
        }

        #endregion

        #region DeleteRoleUserByRoleID

        /// <summary>
        /// 透過角色ID清空RoleUer的資料
        /// </summary>
        [TestMethod()]
        public void DeleteRoleUserByRoleIDTest()
        {
            #region arrange

            string roleID = "1";

            SqlConnection conn = new SqlConnection();

            SqlTransaction tran = null;

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.DeleteRoleUserByRoleID(roleID, ref conn, ref tran);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #region InsertRoleUser

        /// <summary>
        /// 透過角色ID新增RoleUser的資料
        /// </summary>
        [TestMethod()]
        public void InsertRoleUserTest()
        {
            #region arrange

            RoleUserDTO roleUserDTO = new RoleUserDTO() { RoleID = 1, UserID = 1 };

            SqlConnection conn = new SqlConnection();

            SqlTransaction tran = null;

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.InsertRoleUser(roleUserDTO, ref conn, ref tran);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #endregion
    }
}