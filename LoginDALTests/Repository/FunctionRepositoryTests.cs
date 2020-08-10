﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginDAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using LoginVO.VO;
using Rhino.Mocks;
using System.Data.SqlClient;
using LoginDTO.DTO;
using Rhino.Mocks.Constraints;

namespace LoginDAL.Repository.Tests
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
                new FunctionDTO(){ FunctionID = 1 , Url="Role/RoleManagement" , Description = "瀏覽角色管理畫面" },
                new FunctionDTO(){ FunctionID = 2 , Url="Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" },
                new FunctionDTO(){ FunctionID = 3 , Url="Role/EditRole" , Description = "編輯角色" }
            };

            _dataAccess.Stub(o => o.QueryDataTable<FunctionDTO>(Arg<string>.Is.Anything)).Return(reFunctionDTO);

            #endregion

            #region act

            var result = _target.GetFunctionData().ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].FunctionID, reFunctionDTO[i].FunctionID);
                Assert.AreEqual(result[i].Url, reFunctionDTO[i].Url);
                Assert.AreEqual(result[i].Description, reFunctionDTO[i].Description);
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

            FunctionVO functionVO = new FunctionVO() { Url = "Role/RoleManagement", Description = "瀏覽角色管理畫面" };

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

            FunctionVO functionVO = new FunctionVO() { FunctionID = 1, Url = "Role/RoleManagement", Description = "瀏覽角色管理畫面" };

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

        #endregion
    }
}