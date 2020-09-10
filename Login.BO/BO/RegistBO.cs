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
        public ExecuteResult RegistValid(Account account)
        {
            ExecuteResult result = new ExecuteResult();

            try
            {
                // 驗證帳號
                result.IsSuccessed = !_userRepo.FindAccountName(account.AccountName).Any();

                if (!result.IsSuccessed)
                {
                    result.Message = "帳號名稱已被使用";
                    return result;
                }

                //驗證密碼與密碼確認
                if (account.Password != account.PasswordConfirm)
                {
                    result.IsSuccessed = false;
                    result.Message = "密碼確認與密碼輸入不相同";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccessed = false;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        ///  註冊帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public ExecuteResult Regist(Account account)
        {
            ExecuteResult result = new ExecuteResult();

            try
            {
                string key = ConfigurationManager.AppSettings["EncryptKey"];

                account.Password = AESEncryptHelper.AESEncryptBase64(account.Password, key);

                result.IsSuccessed = _userRepo.UserInsert(account) > 0;

                if (!result.IsSuccessed)
                    result.Message = "註冊失敗";
            }
            catch (Exception ex)
            {
                result.IsSuccessed = false;
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion
    }
}
