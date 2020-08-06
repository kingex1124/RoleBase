using LoginDTO.DTO;
using LoginDTO.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.EfRepository.Interface
{
    public interface IRoleUserEfRepository : IRepository<RoleUser>
    {
        IEnumerable<UserCheckDTO> GetUserCheckByRole(string id);

        int DeleteRoleUserByRoleID(string roleID);

        int InsertRoleUser(RoleUserDTO roleUserDTO);
    }
}
