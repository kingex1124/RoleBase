using Login.BO;
using Login.DTO;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Service
{
    public class LoginService : ILoginService
    {
        #region 屬性

        ILoginBO _loginBO;
        
        #endregion

        #region 建構子

        public LoginService()
        {
             _loginBO = new LoginBO();
            //_loginBO = new LoginEfBO();
        }

        public LoginService(ILoginBO loginBO)
        {
            _loginBO = loginBO;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 驗證登入帳號密碼
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        public AccountInfoData AccountValid(AccountInfoData accountInfoData)
        {
            return _loginBO.AccountValid(accountInfoData);
        }

        /// <summary>
        /// 透過帳號名稱取得帳號資料
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        public UserDTO GetUserDataByAccountName(AccountInfoData accountInfoData)
        {
            return _loginBO.GetUserDataByAccountName(accountInfoData);
        }

        /// <summary>
        /// 透過UserID取得角色資料
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRoleDataByUserID(string userID)
        {
            return _loginBO.GetRoleDataByUserID(userID);
        }

        #endregion
    }
}
