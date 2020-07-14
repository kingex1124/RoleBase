using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Service.Interface
{
    public interface ILoginService
    {
        AccountInfoData AccountValid(AccountInfoData accountInfoData);
        UserDTO GetUserDataByAccountName(AccountInfoData accountInfoData);
        IEnumerable<RoleDTO> GetRoleDataByUserID(string userID);
    }
}
