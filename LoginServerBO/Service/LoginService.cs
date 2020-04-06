using LoginDTO.DTO;
using LoginServerBO.BO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Service
{
    public class LoginService
    {
        LoginBO loginBO;
        public LoginService()
        {
            loginBO = new LoginBO();
        }

        /// <summary>
        /// 驗證登入照號密碼
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        public AccountInfoData AccountValid(AccountInfoData accountInfoData)
        {
            //驗證帳號
            if(!loginBO.FindAccountName(accountInfoData.AccountName).Any())
            {
                accountInfoData.Message = "該帳號不存在。";
                return accountInfoData;
            }

            //驗證密碼
            if(loginBO.FindAccountData(accountInfoData.AccountName).Password != accountInfoData.Password)
            {
                accountInfoData.Message = "密碼輸入錯誤。";
                return accountInfoData;
            }

            return accountInfoData;
        }

        /// <summary>
        /// 透過帳號名稱取得帳號資料
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        public UserDTO GetUserDataByAccountName(AccountInfoData accountInfoData)
        {
            return loginBO.FindAccountData(accountInfoData.AccountName);
        }

        /// <summary>
        /// 透過UserID取得角色資料
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRoleDataByUserID(string userID)
        {
            return loginBO.GetRoleDataByAccountName(userID);
        }
    }
}
