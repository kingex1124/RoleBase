using LoginDTO.DTO;
using LoginDTO.EFModel;
using LoginServerBO.BO.Interface;
using LoginServerBO.EfRepository;
using LoginServerBO.EfRepository.Interface;
using LoginServerBO.Repository;
using LoginServerBO.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO
{
    public class SecurityBO : ISecurityBO
    {
        #region 屬性

        IRoleFunctionRepository _roleFunctionRepo;

        IRoleFunctionEfRepository _roleFunctionEfRepo;

        #endregion

        #region 建構子

        public SecurityBO()
        {
            _roleFunctionRepo = new RoleFunctionRepository();

            _roleFunctionEfRepo = new RoleFunctionEfRepository(new RoleBaseEntities());
        }

        public SecurityBO(IRoleFunctionRepository roleFunctionRepo)
        {
            _roleFunctionRepo = roleFunctionRepo;
        }

        public SecurityBO(IRoleFunctionEfRepository roleFunctionEfRepo)
        {
            _roleFunctionEfRepo = roleFunctionEfRepo;
        }

        #endregion

        #region 方法

        public IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId)
        {
            return _roleFunctionRepo.GetSecurityRoleFunction(roleId);

            // Ef
            // return _roleFunctionEfRepo.GetSecurityRoleFunction(roleId);
        }

        #endregion
    }
}
