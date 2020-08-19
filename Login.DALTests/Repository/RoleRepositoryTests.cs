using Login.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using Rhino.Mocks;
using System.Data.SqlClient;
using Rhino.Mocks.Constraints;
using Login.DTO;
using Login.VO;

namespace Login.DAL.Tests
{
    [TestClass()]
    public class RoleRepositoryTests
    {
        #region 屬性

        IDataAccess _dataAccess = MockRepository.GenerateStub<IDataAccess>();
        RoleRepository _target;

        #endregion

        #region 建構子

        public RoleRepositoryTests()
        {
            _target = new RoleRepository(_dataAccess);
        }

        #endregion

        #region 測試方法

        #region GetRoleDataByAccountName

        /// <summary>
        /// 透過帳號查找角色
        /// </summary>
        [TestMethod()]
        public void GetRoleDataByAccountNameTest()
        {
            #region arrange

            string userID = "1";

            List<RoleDTO> reRoleDTOList = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限" },
                new RoleDTO(){ RoleID = 2 , RoleName = "A" , Description = "A1" },
                new RoleDTO(){ RoleID = 3 , RoleName = "B" , Description = "B1" },
            };

            _dataAccess.Stub(o => o.QueryDataTable<RoleDTO>(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reRoleDTOList);

            #endregion

            #region act

            var result = _target.GetRoleDataByAccountName(userID).ToList();

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

        #region GetRoleData

        /// <summary>
        /// 取得Role資料
        /// </summary>
        [TestMethod()]
        public void GetRoleDataTest()
        {
            #region arrange

            List<RoleDTO> reRoleDTO = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1, RoleName = "Admin" , Description = "最高權限" },
                new RoleDTO(){ RoleID = 2, RoleName = "A" , Description = "A1" },
                new RoleDTO(){ RoleID = 3, RoleName = "B" , Description = "B1" },
            };

            PageDataVO pageDataVO = new PageDataVO()
            {
                PageSize = 3,
                DataCount = 3,
                PageNumber = 1,
                LowerBound = 0,
                UpperBound = 2,
                WhereCondition = new List<KeyValueVO>()
                   {
                        new KeyValueVO()
                        {
                             Key = "RoleName",
                             Value = ""
                        }
                   }
            };

            _dataAccess.Stub(o => o.QueryDataTable<RoleDTO>(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reRoleDTO);

            #endregion

            #region act

            var result = _target.GetRoleData(pageDataVO).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].RoleID, reRoleDTO[i].RoleID);
                Assert.AreEqual(result[i].RoleName, reRoleDTO[i].RoleName);
                Assert.AreEqual(result[i].Description, reRoleDTO[i].Description);
            }

            #endregion
        }

        #endregion

        #region GetRoleCount

        /// <summary>
        /// 取得資料總筆數
        /// </summary>
        [TestMethod()]
        public void GetRoleCountTest()
        {
            #region arrange

            int reNumber = 3;

            PageDataVO pageDataVO = new PageDataVO()
            {
                PageSize = 3,
                DataCount = 3,
                PageNumber = 1,
                LowerBound = 0,
                UpperBound = 2,
                WhereCondition = new List<KeyValueVO>()
                   {
                        new KeyValueVO()
                        {
                             Key = "RoleName",
                             Value = ""
                        }
                   }
            };

            _dataAccess.Stub(o => o.ExecuteScalar(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.GetRoleCount(pageDataVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #region AddRole

        /// <summary>
        /// 新增角色
        /// </summary>
        [TestMethod()]
        public void AddRoleTest()
        {
            #region arrange

            RoleVO roleVO = new RoleVO() { RoleName = "Admin", Description = "最高權限" };

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.AddRole(roleVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #region DeleteRole

        /// <summary>
        /// 刪除角色
        /// </summary>
        [TestMethod()]
        public void DeleteRoleTest()
        {
            #region arrange

            string id = "1";

            SqlConnection conn = new SqlConnection();

            SqlTransaction tran = null;

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.DeleteRole(id, ref conn, ref tran);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #region EditRole

        /// <summary>
        /// 編輯角色
        /// </summary>
        [TestMethod()]
        public void EditRoleTest()
        {
            #region arrange

            RoleVO roleVO = new RoleVO() { RoleName = "Admin", Description = "最高權限" };

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.EditRole(roleVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #endregion
    }
}