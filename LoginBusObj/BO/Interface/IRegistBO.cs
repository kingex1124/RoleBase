using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBusObj.BO.Interface
{
    public interface IRegistBO
    {
        Account RegistValid(Account account);

        Account Regist(Account account);
    }
}
