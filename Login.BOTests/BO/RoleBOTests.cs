﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using KevanFramework.DataAccessDAL.SQLDAL.Model;
using System.Data.SqlClient;
using Rhino.Mocks.Constraints;
using Login.DAL;
using Login.DTO;
using Login.VO;

namespace Login.BO.Tests
{
    [TestClass()]
    public class RoleBOTests
    {
        #region 屬性

        IRoleRepository _roleRepo = MockRepository.GenerateStub<IRoleRepository>();
        IRoleUserRepository _roleUserRepo = MockRepository.GenerateStub<IRoleUserRepository>();
        IRoleFunctionRepository _roleFunctionRepo = MockRepository.GenerateStub<IRoleFunctionRepository>();
        ISQLTransactionHelper _sqlConnectionHelper = MockRepository.GenerateStub<ISQLTransactionHelper>();
        RoleBO _target;

        #endregion

        #region 建構子

        public RoleBOTests()
        {
            _target = new RoleBO(_roleRepo, _roleUserRepo, _roleFunctionRepo, _sqlConnectionHelper);
        }

        #endregion

        #region 測試方法

        #region GetRoleData

        /// <summary>
        /// 取得Role資料
        /// </summary>
        [TestMethod()]
        public void GetRoleDataTest()
        {
            #region arrange

            List<RoleDTO> reRoleDTOList = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleDTO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleDTO(){ RoleID = 3 , RoleName = "B" , Description = "B1"}
            };

            List<RoleVO> reRoleVOList = new List<RoleVO>()
            {
                new RoleVO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleVO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleVO(){ RoleID = 3 , RoleName = "B" , Description = "B1"}
            };

            int renumber = 3;

            PageDataVO pageDataVO = new PageDataVO()
            {
                PageNumber = 1,
                WhereCondition = new List<KeyValueVO>()
                   {
                        new KeyValueVO()
                        {
                             Key = "RoleName",
                             Value = ""
                        }
                   }
            };

            _roleRepo.Stub(o => o.GetRoleCount(pageDataVO)).Return(renumber);

            _roleRepo.Stub(o => o.GetRoleData(pageDataVO)).Return(reRoleDTOList);

            #endregion

            #region act

            var result = _target.GetRoleData(pageDataVO).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(result[i].RoleID, reRoleVOList[i].RoleID);
                Assert.AreEqual(result[i].RoleName, reRoleVOList[i].RoleName);
                Assert.AreEqual(result[i].Description, reRoleVOList[i].Description);
            }

            #endregion
        }

        #endregion

        #region AddRole

        /// <summary>
        /// 新增角色
        /// 測試成功
        /// </summary>
        [TestMethod()]
        public void AddRoleTest()
        {
            #region arrange (測試成功)

            RoleVO roleVO = new RoleVO() { RoleName = "Admin", Description = "最高權限" };

            int reNumber = 1;

            string reMessage = "";

            _roleRepo.Stub(o => o.AddRole(Arg<RoleVO>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.AddRole(roleVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 新增角色
        /// 測試新增失敗
        /// </summary>
        [TestMethod()]
        public void AddRoleTest1()
        {
            #region arrange (新增失敗)

            RoleVO roleVO = new RoleVO() { RoleName = "Admin", Description = "最高權限" };

            int reNumber = -1;

            string reMessage = "新增失敗。";

            _roleRepo.Stub(o => o.AddRole(Arg<RoleVO>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.AddRole(roleVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region DeleteRole

        /// <summary>
        /// 刪除角色
        /// 測試刪除成功
        /// </summary>
        [TestMethod()]
        public void DeleteRoleTest()
        {
            #region arrange (刪除成功)

            string id = "1";

            int reDeleteRoleUserResult = 1;

            int reDeleteRoleFunctionResult = 1;

            int reDeleteRoleResult = 1;

            _sqlConnectionHelper.Stub(o => o.BeginTransaction()).Return(new SQLConnTran() { SqlConn = new SqlConnection(), SqlTrans = null });

            _roleUserRepo.Stub(o => o.DeleteRoleUserByRoleID(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteRoleUserResult);

            _roleFunctionRepo.Stub(o => o.DeleteRoleFunctionByRoleID(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteRoleFunctionResult);

            _roleRepo.Stub(o => o.DeleteRole(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteRoleResult);

            string reMessage = "";

            #endregion

            #region act

            var result = _target.DeleteRole(id);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 刪除角色
        /// 測試刪除失敗
        /// </summary>
        [TestMethod()]
        public void DeleteRoleTest1()
        {
            #region arrange (刪除失敗)

            string id = "1";

            int reDeleteRoleUserResult = -1;

            int reDeleteRoleFunctionResult = 1;

            int reDeleteRoleResult = 1;

            _sqlConnectionHelper.Stub(o => o.BeginTransaction()).Return(new SQLConnTran() { SqlConn = new SqlConnection(), SqlTrans = null });

            _roleUserRepo.Stub(o => o.DeleteRoleUserByRoleID(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteRoleUserResult);

            _roleFunctionRepo.Stub(o => o.DeleteRoleFunctionByRoleID(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteRoleFunctionResult);

            _roleRepo.Stub(o => o.DeleteRole(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteRoleResult);

            string reMessage = "刪除失敗。";

            #endregion

            #region act

            var result = _target.DeleteRole(id);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region EditRole

        /// <summary>
        /// 編輯角色
        /// 測試編輯成功
        /// </summary>
        [TestMethod()]
        public void EditRoleTest()
        {
            #region arrange (編輯成功)

            RoleVO roleVO = new RoleVO() { RoleID = 1, RoleName = "Admin", Description = "最高權限" };

            int reNumber = 1;

            string reMessage = "";

            _roleRepo.Stub(o => o.EditRole(Arg<RoleVO>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.EditRole(roleVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 編輯角色
        /// 測試編輯失敗
        /// </summary>
        [TestMethod()]
        public void EditRoleTest1()
        {
            #region arrange (編輯失敗)

            RoleVO roleVO = new RoleVO() { RoleID = 1, RoleName = "Admin", Description = "最高權限" };

            int reNumber = -1;

            string reMessage = "編輯失敗。";

            _roleRepo.Stub(o => o.EditRole(Arg<RoleVO>.Is.Anything)).Return(reNumber);

            #endregion

            #region act

            var result = _target.EditRole(roleVO);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region GetUserCheckByRole

        /// <summary>
        /// 取得角色設定使用者的資料
        /// </summary>
        [TestMethod()]
        public void GetUserCheckByRoleTest()
        {
            #region arrange

            string roleID = "1";

            List<UserCheckDTO> reUserCheckDTOList = new List<UserCheckDTO>()
            {
                new UserCheckDTO(){ UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckDTO(){ UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckDTO(){ UserID = 3 , UserName = "B" , AccountName = "B1" , Check = false }
            };

            List<UserCheckVO> reUserCheckVOList = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1 , UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckVO(){ RoleID = 1 , UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckVO(){ RoleID = 1 , UserID = 3 , UserName = "B" , AccountName = "B1" , Check = false }
            };

            PageDataVO pageDataVO = new PageDataVO() { OrderByColumn = "UserID", OrderByType = "ASC" };

            _roleUserRepo.Stub(o => o.GetUserCheckByRole(Arg<string>.Is.Anything, Arg<PageDataVO>.Is.Anything)).Return(reUserCheckDTOList);

            #endregion

            #region act

            var result = _target.GetUserCheckByRole(roleID, pageDataVO).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].RoleID, 0);
                Assert.AreEqual(result[i].UserID, reUserCheckVOList[i].UserID);
                Assert.AreEqual(result[i].UserName, reUserCheckVOList[i].UserName);
                Assert.AreEqual(result[i].AccountName, reUserCheckVOList[i].AccountName);
                Assert.AreEqual(result[i].Check, reUserCheckVOList[i].Check);
            }

            #endregion
        }

        #endregion

        #region SaveRoleUserSetting

        /// <summary>
        /// 角色編輯使用者
        /// 儲存勾選使用者時的變更
        /// 測試成功
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest()
        {
            #region arrange (測試成功)

            List<UserCheckVO> userCheckVOList = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1, UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 3 , UserName = "B" , AccountName = "B2" , Check = false }
            };

            int reDeleteResult = 1;

            int reInsertResult = 1;

            string reMessage = "";

            _sqlConnectionHelper.Stub(o => o.BeginTransaction()).Return(new SQLConnTran() { SqlConn = new SqlConnection(), SqlTrans = null });

            _roleUserRepo.Stub(o => o.DeleteRoleUserByRoleID(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteResult);

            _roleUserRepo.Stub(o => o.InsertRoleUser(Arg<RoleUserDTO>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reInsertResult);

            #endregion

            #region act

            var result = _target.SaveRoleUserSetting(userCheckVOList);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 角色編輯使用者
        /// 儲存勾選使用者時的變更
        /// 測試失敗
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest1()
        {
            #region arrange (測試失敗)

            List<UserCheckVO> userCheckVOList = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1, UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 3 , UserName = "B" , AccountName = "B2" , Check = false }
            };

            int reDeleteResult = -1;

            int reInsertResult = -1;

            string reMessage = "刪除失敗。";

            _sqlConnectionHelper.Stub(o => o.BeginTransaction()).Return(new SQLConnTran() { SqlConn = new SqlConnection(), SqlTrans = null });

            _roleUserRepo.Stub(o => o.DeleteRoleUserByRoleID(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteResult);

            _roleUserRepo.Stub(o => o.InsertRoleUser(Arg<RoleUserDTO>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reInsertResult);

            #endregion

            #region act

            var result = _target.SaveRoleUserSetting(userCheckVOList);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 角色編輯使用者
        /// 儲存勾選使用者時的變更
        /// 測試失敗
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest2()
        {
            #region arrange (測試失敗)

            List<UserCheckVO> userCheckVOList = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1, UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 3 , UserName = "B" , AccountName = "B2" , Check = false }
            };

            int reDeleteResult = 1;

            int reInsertResult = -1;

            string reMessage = "設定失敗。";

            _sqlConnectionHelper.Stub(o => o.BeginTransaction()).Return(new SQLConnTran() { SqlConn = new SqlConnection(), SqlTrans = null });

            _roleUserRepo.Stub(o => o.DeleteRoleUserByRoleID(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteResult);

            _roleUserRepo.Stub(o => o.InsertRoleUser(Arg<RoleUserDTO>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reInsertResult);

            #endregion

            #region act

            var result = _target.SaveRoleUserSetting(userCheckVOList);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #region ClearRoleUserByRoleID

        /// <summary>
        /// 角色編輯使用者
        /// 儲存清空使用者時的變更
        /// 測試成功
        /// </summary>
        [TestMethod()]
        public void ClearRoleUserByRoleIDTest()
        {
            #region arrange (測試成功)

            string roleID = "1";

            int reDeleteResult = 1;

            string reMessage = "";

            _sqlConnectionHelper.Stub(o => o.BeginTransaction()).Return(new SQLConnTran() { SqlConn = new SqlConnection(), SqlTrans = null });

            _roleUserRepo.Stub(o => o.DeleteRoleUserByRoleID(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteResult);

            #endregion

            #region act

            var result = _target.ClearRoleUserByRoleID(roleID);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        /// <summary>
        /// 角色編輯使用者
        /// 儲存清空使用者時的變更
        /// 測試失敗
        /// </summary>
        [TestMethod()]
        public void ClearRoleUserByRoleIDTest1()
        {
            #region arrange (測試失敗)

            string roleID = "1";

            int reDeleteResult = -1;

            string reMessage = "刪除失敗。";

            _sqlConnectionHelper.Stub(o => o.BeginTransaction()).Return(new SQLConnTran() { SqlConn = new SqlConnection(), SqlTrans = null });

            _roleUserRepo.Stub(o => o.DeleteRoleUserByRoleID(Arg<string>.Is.Anything, ref Arg<SqlConnection>.Ref(Is.Anything(), null).Dummy, ref Arg<SqlTransaction>.Ref(Is.Anything(), null).Dummy)).Return(reDeleteResult);

            #endregion

            #region act

            var result = _target.ClearRoleUserByRoleID(roleID);

            #endregion

            #region assert

            Assert.AreEqual(result, reMessage);

            #endregion
        }

        #endregion

        #endregion
    }
}