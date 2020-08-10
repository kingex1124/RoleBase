using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDAL.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<UserDTO> FindAccountName(string accountName);

        UserDTO FindAccountData(string accountName);

        int UserInsert(Account account);
    }
}
