using Login.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using Rhino.Mocks;
using Login.VO;
using System.Data.SqlClient;
using Login.DTO;
using Rhino.Mocks.Constraints;

namespace Login.DAL.Tests
{
    [TestClass()]
    public class FunctionRepositoryTests
    {
        #region 屬性

        IDataAccess _dataAccess = MockRepository.GenerateStub<IDataAccess>();

        FunctionRepository _target;

        #endregion

        #region 建構子

        public FunctionRepositoryTests()
        {
            _target = new FunctionRepository(_dataAccess);
        }

        #endregion

        #region 測試方法

        #region GetFunctionData

        /// <summary>
        /// 取得Function資料
        /// </summary>
        [TestMethod()]
        public void GetFunctionDataTest()
        {
            #region arrange

            List<FunctionDTO> reFunctionDTO = new List<FunctionDTO>()
            {
                new FunctionDTO(){ FunctionID = 1 , Url="Role/RoleManagement" , Title = "角色管理" , Description = "瀏覽角色管理畫面" , IsMenu = true , Parent = 0 , ParentName = "No" },
                new FunctionDTO(){ FunctionID = 2 , Url="Role/RoleAddEditDelete" , Title = "編輯角色" , Description = "角色新增修改刪除畫面" , IsMenu = true , Parent = 1 , ParentName = "角色管理" },
                new FunctionDTO(){ FunctionID = 3 , Url="Role/EditRole" , Title = "編輯" , Description = "編輯角色" , IsMenu = false , Parent = -1 , ParentName = "Not Menu"}
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
                             Key = "Url",
                             Value = ""
                        }
                   }
            };

            _dataAccess.Stub(o => o.QueryDataTable<FunctionDTO>(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reFunctionDTO);

            #endregion

            #region act

            var result = _target.GetFunctionData(pageDataVO).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].FunctionID, reFunctionDTO[i].FunctionID);
                Assert.AreEqual(result[i].Url, reFunctionDTO[i].Url);
                Assert.AreEqual(result[i].Title, reFunctionDTO[i].Title);
                Assert.AreEqual(result[i].Description, reFunctionDTO[i].Description);
                Assert.AreEqual(result[i].IsMenu, reFunctionDTO[i].IsMenu);
                Assert.AreEqual(result[i].Parent, reFunctionDTO[i].Parent);
                Assert.AreEqual(result[i].ParentName, reFunctionDTO[i].ParentName);
            }

            #endregion
        }

        #endregion


        #region GetFunctionCount

        /// <summary>
        /// 取得功能資料總筆數
        /// </summary>
        [TestMethod()]
        public void GetFunctionCountTest()
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
                             Key = "Url",
                             Value = ""
                        }
                   }
            };

            _dataAccess.Stub(o => o.ExecuteScalar(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.GetFunctionCount(pageDataVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #region GetParentKeyValueTest

        /// <summary>
        /// 取得作為上層的keyValue資料
        /// </summary>
        [TestMethod()]
        public void GetParentKeyValueTest()
        {
            #region arrange

            List<KeyValuePairDTO> reKeyValuePairDTO = new List<KeyValuePairDTO>()
            {
                new KeyValuePairDTO(){ Key = 1 , Value = "角色管理"},
                new KeyValuePairDTO(){ Key = 2 , Value = "編輯角色"},
                new KeyValuePairDTO(){ Key = 3 , Value = "新增"},
            };

            _dataAccess.Stub(o => o.QueryDataTable<KeyValuePairDTO>(Arg<string>.Is.Anything)).Return(reKeyValuePairDTO);

            #endregion

            #region act

            var result = _target.GetParentKeyValue().ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].Key, reKeyValuePairDTO[i].Key);
                Assert.AreEqual(result[i].Value, reKeyValuePairDTO[i].Value);
            }

            #endregion
        }

        #endregion

        #region AddFunction

        /// <summary>
        /// 新增功能
        /// </summary>
        [TestMethod()]
        public void AddFunctionTest()
        {
            #region arrange

            FunctionVO functionVO = new FunctionVO() { Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.AddFunction(functionVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #region DeleteFunction

        /// <summary>
        /// 刪除功能
        /// </summary>
        [TestMethod()]
        public void DeleteFunctionTest()
        {
            #region arrange

            string id = "1";

            SqlConnection conn = new SqlConnection();

            SqlTransaction tran = null;

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy, Arg<object[]>.Is.Anything)).Return(reNumber);


            #endregion

            #region act

            var result = _target.DeleteFunction(id, ref conn, ref tran);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #region EditFunction

        /// <summary>
        /// 編輯功能
        /// </summary>
        [TestMethod()]
        public void EditFunctionTest()
        {
            #region arrange

            FunctionVO functionVO = new FunctionVO() { FunctionID = 1, Url = "Role/RoleManagement", Title = "角色管理", Description = "瀏覽角色管理畫面", IsMenu = true, Parent = 0 };

            int reNumber = 1;

            _dataAccess.Stub(o => o.ExcuteSQL(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.EditFunction(functionVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reNumber);

            #endregion
        }

        #endregion

        #region GetMenuDataTest

        /// <summary>
        /// 取得Menu資料
        /// </summary>
        [TestMethod()]
        public void GetMenuDataTest()
        {
            #region arrang

            string userID = "1";

            List<FunctionMenuDTO> reFunctionMenuDTO = new List<FunctionMenuDTO>()
            {
                new FunctionMenuDTO(){ FunctionID = 1 , Url = "Role/RoleManagement" , Parent = 0 , Title = "角色管理"},
                new FunctionMenuDTO(){ FunctionID = 2 , Url = "Role/RoleAddEditDelete" , Parent = 1 , Title = "編輯角色"},
                new FunctionMenuDTO(){ FunctionID = 8 , Url = "Role/RoleUserEdit" , Parent = 1 , Title = "編輯角色使用者"}
            };

            _dataAccess.Stub(o => o.QueryDataTable<FunctionMenuDTO>(Arg<string>.Is.Anything, Arg<object[]>.Is.Anything)).Return(reFunctionMenuDTO);

            #endregion

            #region act

            var result = _target.GetMenuData(userID).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].FunctionID, reFunctionMenuDTO[i].FunctionID);
                Assert.AreEqual(result[i].Url, reFunctionMenuDTO[i].Url);
                Assert.AreEqual(result[i].Parent, reFunctionMenuDTO[i].Parent);
                Assert.AreEqual(result[i].Title, reFunctionMenuDTO[i].Title);
            }

            #endregion
        }

        #endregion

        #endregion
    }
}