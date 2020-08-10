using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBusObj.BO.Interface
{
    public interface ILoginBO
    {
        AccountInfoData AccountValid(AccountInfoData accountInfoData);

        UserDTO GetUserDataByAccountName(AccountInfoData accountInfoData);

        IEnumerable<RoleDTO> GetRoleDataByUserID(string userID);
    }
}
