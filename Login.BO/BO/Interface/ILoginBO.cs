using Login.DTO;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.BO
{
    public interface ILoginBO
    {
        ExecuteResult AccountValid(AccountInfoData accountInfoData);

        UserDTO GetUserDataByAccountName(AccountInfoData accountInfoData);

        IEnumerable<RoleDTO> GetRoleDataByUserID(string userID);
    }
}
