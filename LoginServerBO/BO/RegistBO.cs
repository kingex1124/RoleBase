using LoginServerBO.BO.Interface;
using LoginServerBO.Repository;
using LoginServerBO.Repository.Interface;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO
{
    public class RegistBO : IRegistBO
    {
        #region 屬性

        private IUserRepository _userRep;

        #endregion

        #region 建構子

        public RegistBO()
        {       
            _userRep = new UserRepository();
        }

        public RegistBO(IUserRepository userRep)
        {
            _userRep = userRep;
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
            // 驗證帳號
            if (_userRep.FindAccountName(account.AccountName).Any())
            {
                account.Message = "帳號名稱已被使用";
                return account;
            }

            //驗證密碼與密碼確認
            if (account.Password != account.PasswordConfirm)
            {
                account.Message = "密碼確認與密碼輸入不相同";
                return account;
            }

            return account;
        }

        /// <summary>
        ///  註冊帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Account Regist(Account account)
        {
            if (_userRep.UserInsert(account) > 0)
                return account;
            else
            {
                account.Message = "註冊失敗";
            }
            return account;
        }


        #endregion
    }
}
