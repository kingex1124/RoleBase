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
    public interface IUserEfRepository : IRepository<User>
    {
        IEnumerable<UserDTO> FindAccountName(string accountName);

        UserDTO FindAccountData(string accountName);

        int UserInsert(Account account);
    }
}
