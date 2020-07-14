using LoginDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO.Interface
{
    public interface ILoginBO
    {
        IEnumerable<UserDTO> FindAccountName(string accountName);
        UserDTO FindAccountData(string accountName);
        IEnumerable<RoleDTO> GetRoleDataByAccountName(string userID);

    }
}
