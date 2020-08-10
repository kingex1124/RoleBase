using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginServiceObj.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginBusObj.BO.Interface;
using Rhino.Mocks;
using LoginVO.VO;

namespace LoginServiceObj.Service.Tests
{
    [TestClass()]
    public class RoleServiceTests
    {
        #region 屬性

        IRoleBO _roleBO = MockRepository.GenerateStub<IRoleBO>();
        RoleService _target;

        #endregion

        #region 建構子

        public RoleServiceTests()
        {
            _target = new RoleService(_roleBO);
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

            List<RoleVO> reRoleVOList = new List<RoleVO>()
            {
                new RoleVO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleVO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleVO(){ RoleID = 3 , RoleName = "B" , Description = "B1"},
            };

            _roleBO.Stub(o => o.GetRoleData()).Return(reRoleVOList);

            #endregion

            #region act

            var result = _target.GetRoleData().ToList();

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
        /// 測試成功新增
        /// </summary>
        [TestMethod()]
        public void AddRoleTest()
        {
            #region arrange (成功新增)

            RoleVO roleVO = new RoleVO() { RoleName = "Admin", Description = "最高權限" };

            string reMessage = "";

            _roleBO.Stub(o => o.AddRole(Arg<RoleVO>.Is.Anything)).Return(reMessage);

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

            string reMessage = "新增失敗。";

            _roleBO.Stub(o => o.AddRole(Arg<RoleVO>.Is.Anything)).Return(reMessage);

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

            string reMessage = "";

            _roleBO.Stub(o => o.DeleteRole(Arg<string>.Is.Anything)).Return(reMessage);

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

            string reMessage = "刪除失敗。";

            _roleBO.Stub(o => o.DeleteRole(Arg<string>.Is.Anything)).Return(reMessage);

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

            string reMessage = "";

            _roleBO.Stub(o => o.EditRole(Arg<RoleVO>.Is.Anything)).Return(reMessage);

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

            string reMessage = "編輯失敗。";

            _roleBO.Stub(o => o.EditRole(Arg<RoleVO>.Is.Anything)).Return(reMessage);

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

            List<UserCheckVO> reUserCheckVOList = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1 , UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckVO(){ RoleID = 1 , UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckVO(){ RoleID = 1 , UserID = 3 , UserName = "B" , AccountName = "B1" , Check = false }
            };

            _roleBO.Stub(o => o.GetUserCheckByRole(Arg<string>.Is.Anything)).Return(reUserCheckVOList);

            #endregion

            #region act

            var result = _target.GetUserCheckByRole(roleID).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].RoleID, reUserCheckVOList[i].RoleID);
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
        /// 測試編輯成功
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest()
        {
            #region arrange (編輯成功)

            List<UserCheckVO> userCheckVOList = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1, UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 3 , UserName = "B" , AccountName = "B2" , Check = false }
            };

            string reMessage = "";

            _roleBO.Stub(o => o.SaveRoleUserSetting(Arg<IEnumerable<UserCheckVO>>.Is.Anything)).Return(reMessage);

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
        /// 測試編輯失敗
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest1()
        {
            #region arrange (刪除失敗)

            List<UserCheckVO> userCheckVOList = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1, UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 3 , UserName = "B" , AccountName = "B2" , Check = false }
            };

            string reMessage = "刪除失敗。";

            _roleBO.Stub(o => o.SaveRoleUserSetting(Arg<IEnumerable<UserCheckVO>>.Is.Anything)).Return(reMessage);

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
        /// 測試編輯失敗
        /// </summary>
        [TestMethod()]
        public void SaveRoleUserSettingTest2()
        {
            #region arrange (設定失敗)

            List<UserCheckVO> userCheckVOList = new List<UserCheckVO>()
            {
                new UserCheckVO(){ RoleID = 1, UserID = 1 , UserName = "kevan" , AccountName = "kevan" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 2 , UserName = "A" , AccountName = "A1" , Check = true },
                new UserCheckVO(){ RoleID = 1, UserID = 3 , UserName = "B" , AccountName = "B2" , Check = false }
            };

            string reMessage = "設定失敗。";

            _roleBO.Stub(o => o.SaveRoleUserSetting(Arg<IEnumerable<UserCheckVO>>.Is.Anything)).Return(reMessage);

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

            string reMessage = "";

            _roleBO.Stub(o => o.ClearRoleUserByRoleID(Arg<string>.Is.Anything)).Return(reMessage);

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

            string reMessage = "刪除失敗。";

            _roleBO.Stub(o => o.ClearRoleUserByRoleID(Arg<string>.Is.Anything)).Return(reMessage);

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