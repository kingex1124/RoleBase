using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using Login.BO;
using Login.DTO;

namespace Login.Service.Tests
{
    [TestClass()]
    public class SecurityServiceTests
    {
        #region 屬性

        ISecurityBO _securityBO = MockRepository.GenerateStub<ISecurityBO>();

        SecurityService _target;

        #endregion

        #region 建構子

        public SecurityServiceTests()
        {
            _target = new SecurityService(_securityBO);
        }

        #endregion

        #region 測試方法

        #region GetSecurityRoleFunction

        [TestMethod()]
        public void GetSecurityRoleFunctionTest()
        {
            #region arrange

            // 傳入參數
            string roleId = "1";

            List<SecurityRoleFunctionDTO> reSRFDTOList = new List<SecurityRoleFunctionDTO>()
            {
                new SecurityRoleFunctionDTO(){ RoleName = "Admin" ,  Url = "Role/RoleManagement" , Description = "瀏覽角色管理畫面" },
                new SecurityRoleFunctionDTO(){ RoleName = "Admin" ,  Url = "Role/RoleAddEditDelete" , Description = "角色新增修改刪除畫面" },
                new SecurityRoleFunctionDTO(){ RoleName = "Admin" ,  Url = "Role/EditRole" , Description = "編輯角色" }
            };

            _securityBO.Stub(o => o.GetSecurityRoleFunction(Arg<string>.Is.Anything)).Return(reSRFDTOList);

            #endregion

            #region act

            var result = _target.GetSecurityRoleFunction(roleId).ToList();

            #endregion

            #region assert

            for (int i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(result[i].RoleName, reSRFDTOList[i].RoleName);
                Assert.AreEqual(result[i].Url, reSRFDTOList[i].Url);
                Assert.AreEqual(result[i].Description, reSRFDTOList[i].Description);
            }

            #endregion
        }

        #endregion

        #endregion
    }
}