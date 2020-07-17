using LoginDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Repository.Interface
{
    public interface IRoleRepository
    {
        IEnumerable<RoleDTO> GetRoleDataByAccountName(string userID);
    }
}
