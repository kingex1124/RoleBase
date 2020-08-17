using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Login.DTO;

namespace Login.DAL.Tests
{
    [TestClass()]
    public class RoleFunctionRepositoryTests
    {
        #region 屬性

        IDataAccess _dataAccess = MockRepository.GenerateStub<IDataAccess>();

        RoleFunctionRepository _target;

        #endregion

        #region 建構子

        public RoleFunctionRepositoryTests()
        {
            _target = new RoleFunctionRepository(_dataAccess);
        }

        #endregion

        #region 測試方法

        #region GetSecurityRoleFunction

        /// <summary>
        /// 取得該角色ID所具備的權限功能
        /// </summary>
        [TestMethod()]
        public void GetSecurityRoleFunctionTest()
        {
            #region arrange

            string userID = "1";

            List<SecurityRoleFunctionDTO> reSRFDTO = new List<SecurityRoleFunctionDTO>()
            {
                new SecurityRoleFunctionDTO(){  Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" },
                new SecurityRoleFunctionDTO(){  Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" },
                new SecurityRoleFunctionDTO(){  Url = "Role/EditRole" , Description = "編輯角色" },
            };

            _dataAccess.Stub(o => o.QueryDataTable<SecurityRoleFunctionDTO>(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reSRFDTO);

            #endregion

            #region act

            var result = _target.GetSecurityRoleFunction(userID).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].Url, reSRFDTO[i].Url);
                Assert.AreEqual(result[i].Description, reSRFDTO[i].Description);
            }

            #endregion
        }

        #endregion

        #region DeleteRoleFunctionByFunctionID

        /// <summary>
        /// 透過FunctionID刪除RoleFunction資料
        /// </summary>
        [TestMethod()]
        public void DeleteRoleFunctionByFunctionIDTest()
        {
            #region arrange

            string functionId = "1";

            SqlConnection conn = new SqlConnection();

            SqlTransaction tran = null;

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.DeleteRoleFunctionByFunctionID(functionId, ref conn, ref tran);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

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

            string id = "1";

            List<FunctionCheckDTO> reFunctionCheckDTO = new List<FunctionCheckDTO>()
            {
                new FunctionCheckDTO(){ FunctionID = 1 , Url = "Role/RoleManagement" , Title = "角色管理", Description = "瀏覽角色管理畫面" , IsMenu = true, ParentName = "No", Check = true },
                new FunctionCheckDTO(){ FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Title = "編輯角色", Description = "角色新增修改刪除畫面" , IsMenu = true, ParentName = "角色管理" , Check = true },
                new FunctionCheckDTO(){ FunctionID = 3 , Url = "Role/EditRole", Title = "編輯" , Description = "編輯角色" , IsMenu = false, ParentName = "Not Menu" , Check = false }
            };

            _dataAccess.Stub(o => o.QueryDataTable<FunctionCheckDTO>(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reFunctionCheckDTO);

            #endregion

            #region act

            var result = _target.GetFunctionCheckByRole(id).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].FunctionID, reFunctionCheckDTO[i].FunctionID);
                Assert.AreEqual(result[i].Url, reFunctionCheckDTO[i].Url);
                Assert.AreEqual(result[i].Title, reFunctionCheckDTO[i].Title);
                Assert.AreEqual(result[i].Description, reFunctionCheckDTO[i].Description);
                Assert.AreEqual(result[i].IsMenu, reFunctionCheckDTO[i].IsMenu);
                Assert.AreEqual(result[i].ParentName, reFunctionCheckDTO[i].ParentName);
                Assert.AreEqual(result[i].Check, reFunctionCheckDTO[i].Check);
            }

            #endregion
        }

        #endregion

        #region DeleteRoleFunctionByRoleID

        /// <summary>
        /// 透過角色ID清空RoleFunciton的資料
        /// </summary>
        [TestMethod()]
        public void DeleteRoleFunctionByRoleIDTest()
        {
            #region arrange

            string roleID = "1";

            SqlConnection conn = new SqlConnection();

            SqlTransaction tran = null;

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.DeleteRoleFunctionByRoleID(roleID, ref conn, ref tran);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #region InsertRoleFunction

        /// <summary>
        /// 透過角色ID新增RoleFunction資料
        /// </summary>
        [TestMethod()]
        public void InsertRoleFunctionTest()
        {
            #region arrange

            RoleFunctionDTO roleFunctionDTO = new RoleFunctionDTO() { RoleID = 1, FunctionID = 1 };

            SqlConnection conn = new SqlConnection();

            SqlTransaction tran = null;

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy, Arg<object[]>.Is.Anything)).Return(reNumber);


            #endregion

            #region act

            var result = _target.InsertRoleFunction(roleFunctionDTO, ref conn, ref tran);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #endregion
    }
}