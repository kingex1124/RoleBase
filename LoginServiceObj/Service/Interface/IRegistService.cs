using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServiceObj.Service.Interface
{
    public interface IRegistService
    {
        Account RegistValid(Account account);
        Account Regist(Account account);
    }
}
