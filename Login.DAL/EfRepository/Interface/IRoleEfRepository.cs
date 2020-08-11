using Login.DTO;
using Login.DTO.EFModel;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
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
