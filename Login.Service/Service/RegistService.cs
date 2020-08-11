using Login.BO;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Service
{
    public class RegistService : IRegistService
    {
        #region 屬性

        IRegistBO _registBO;

        #endregion

        #region 建構子

        public RegistService()
        {
            _registBO = new RegistBO();
        }

        public RegistService(IRegistBO registBO)
        {
            _registBO = registBO;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 驗證帳號資料
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Account RegistValid(Account account)
        {
            return _registBO.RegistValid(account);
        }

        /// <summary>
        ///  註冊帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Account Regist(Account account)
        {
            return _registBO.Regist(account);
        }

        #endregion
    }
}
