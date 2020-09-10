using Login.DAL;
using Login.DTO;
using Login.DTO.EFModel;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.BO
{
    public class LoginEfBO : ILoginBO
    {
        #region 屬性

        IUserEfRepository _userEfRepo;
        IRoleEfRepository _roleEfRepo;

        #endregion

        #region 建構子

        public LoginEfBO()
        {
            _userEfRepo = new UserEfRepository(new RoleBaseEntities());
            _roleEfRepo = new RoleEfRepository(new RoleBaseEntities());
        }

        public LoginEfBO(IUserEfRepository userEfRepo, IRoleEfRepository roleEfRepo)
        {
            _userEfRepo = userEfRepo;
            _roleEfRepo = roleEfRepo;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 驗證登入帳號密碼
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        public ExecuteResult AccountValid(AccountInfoData accountInfoData)
        {
            ExecuteResult result = new ExecuteResult();

            try
            {
                result.IsSuccessed = _userEfRepo.FindAccountName(accountInfoData.AccountName).Any();
                //驗證帳號
                if (!result.IsSuccessed)
                {
                    result.Message = "該帳號不存在。";
                    return result;
                }

                string key = ConfigurationManager.AppSettings["EncryptKey"] == null ? "1qaz@WSX" : ConfigurationManager.AppSettings["EncryptKey"];

                accountInfoData.Password = AESEncryptHelper.AESEncryptBase64(accountInfoData.Password, key);

                result.IsSuccessed = _userEfRepo.FindAccountData(accountInfoData.AccountName).Password == accountInfoData.Password;
                //驗證密碼
                if (!result.IsSuccessed)
                    result.Message = "密碼輸入錯誤。";
            }
            catch (Exception ex)
            {
                result.IsSuccessed = false;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 透過帳號名稱取得帳號資料
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        public UserDTO GetUserDataByAccountName(AccountInfoData accountInfoData)
        {
            return _userEfRepo.FindAccountData(accountInfoData.AccountName);
        }

        /// <summary>
        /// 透過UserID取得角色資料
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRoleDataByUserID(string userID)
        {
            return _roleEfRepo.GetRoleDataByAccountName(userID);
        }

        #endregion
    }
}
