using LoginBusObj.BO.Interface;
using LoginDAL.EfRepository;
using LoginDAL.EfRepository.Interface;
using LoginDTO.DTO;
using LoginDTO.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBusObj.EfBO
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
