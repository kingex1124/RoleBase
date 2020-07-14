using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO.Interface
{
    public interface IRegistBO
    {
        IEnumerable<UserDTO> FindAccountName(string accountName);
        int UserInsert(Account account);
    }
}
