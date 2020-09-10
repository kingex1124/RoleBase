using Login.DAL;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.BO
{
    public class RegistBO : IRegistBO
    {
        #region 屬性

        private IUserRepository _userRepo;

        #endregion

        #region 建構子

        public RegistBO()
        {
            _userRepo = new UserRepository();
        }

        public RegistBO(IUserRepository userRep)
        {
            _userRepo = userRep;
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
            if (_userRepo.FindAccountName(account.AccountName).Any())
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
            string key = ConfigurationManager.AppSettings["EncryptKey"];

            account.Password = AESEncryptHelper.AESEncryptBase64(account.Password, key);

            if (_userRepo.UserInsert(account) > 0)
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
