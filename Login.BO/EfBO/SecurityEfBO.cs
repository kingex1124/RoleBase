using Login.DAL;
using Login.DTO;
using Login.DTO.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.BO
{
    public class SecurityEfBO : ISecurityBO
    {
        #region 屬性

        IRoleFunctionEfRepository _roleFunctionEfRepo;

        #endregion

        #region 建構子

        public SecurityEfBO()
        {
            _roleFunctionEfRepo = new RoleFunctionEfRepository(new RoleBaseEntities());
        }

        public SecurityEfBO(IRoleFunctionEfRepository roleFunctionEfRepo)
        {
            _roleFunctionEfRepo = roleFunctionEfRepo;
        }

        #endregion

        #region 方法

        public IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId)
        {
            return _roleFunctionEfRepo.GetSecurityRoleFunction(roleId);
        }

        #endregion
    }
}
