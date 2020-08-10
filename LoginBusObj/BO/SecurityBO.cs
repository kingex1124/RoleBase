using LoginBusObj.BO.Interface;
using LoginDAL.Repository;
using LoginDAL.Repository.Interface;
using LoginDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBusObj.BO
{
    public class SecurityBO : ISecurityBO
    {
        #region 屬性

        IRoleFunctionRepository _roleFunctionRepo;

        #endregion

        #region 建構子

        public SecurityBO()
        {
            _roleFunctionRepo = new RoleFunctionRepository();
        }

        public SecurityBO(IRoleFunctionRepository roleFunctionRepo)
        {
            _roleFunctionRepo = roleFunctionRepo;
        }

        #endregion

        #region 方法

        public IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId)
        {
            return _roleFunctionRepo.GetSecurityRoleFunction(roleId);
        }

        #endregion
    }
}
