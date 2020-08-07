using LoginDTO.DTO;
using LoginDTO.EFModel;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.EfRepository.Interface
{
    public interface IRoleEfRepository : IRepository<Role>
    {
        IEnumerable<RoleDTO> GetRoleDataByAccountName(string userID);

        IEnumerable<RoleDTO> GetRoleData();

        int AddRole(RoleVO roleVO);

        int DeleteRole(string id);

        int EditRole(RoleVO roleVO);

    }
}
