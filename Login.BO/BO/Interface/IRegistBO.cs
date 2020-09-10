using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.BO
{
    public interface IRegistBO
    {
        ExecuteResult RegistValid(Account account);

        ExecuteResult Regist(Account account);
    }
}
