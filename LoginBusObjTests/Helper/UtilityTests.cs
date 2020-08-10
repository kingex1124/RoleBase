using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginBusObj.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginDTO.DTO;
using LoginVO.VO;

namespace LoginBusObj.Helper.Tests
{
    [TestClass()]
    public class UtilityTests
    {
        [TestMethod()]
        public void MigrationIEnumerableTest()
        {
            #region arrange

            List<RoleDTO> reRoleDTOList = new List<RoleDTO>()
            {
                new RoleDTO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleDTO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleDTO(){ RoleID = 3 , RoleName = "B" , Description = "B1"},
            };

            List<RoleVO> reRoleVOList = new List<RoleVO>()
            {
                new RoleVO(){ RoleID = 1 , RoleName = "Admin" , Description = "最高權限"},
                new RoleVO(){ RoleID = 2 , RoleName = "A" , Description = "A1"},
                new RoleVO(){ RoleID = 3 , RoleName = "B" , Description = "B1"},
            };

            #endregion

            #region act

            var result = Utility.MigrationIEnumerable<RoleDTO, RoleVO>(reRoleDTOList).ToList();

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
    }
}