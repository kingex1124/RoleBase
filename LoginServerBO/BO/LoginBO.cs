using LoginDTO.DTO;
using LoginDTO.EFModel;
using LoginServerBO.BO.Interface;
using LoginServerBO.EfRepository;
using LoginServerBO.EfRepository.Interface;
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
    public class LoginBO : ILoginBO
    {
        #region 屬性

        IUserRepository _userRepo;
        IRoleRepository _roleRepo;

        IUserEfRepository _userEfRepo;
        IRoleEfRepository _roleEfRepo;

        #endregion

        #region 建構子

        public LoginBO()
        {
            _userRepo = new UserRepository();
            _roleRepo = new RoleRepository();

            _userEfRepo = new UserEfRepository(new RoleBaseEntities());
            _roleEfRepo = new RoleEfRepository(new RoleBaseEntities());
        }

        public LoginBO(IUserRepository userRep, IRoleRepository roleRep)
        {
            _userRepo = userRep;
            _roleRepo = roleRep;
        }

        public LoginBO(IUserEfRepository userEfRepo, IRoleEfRepository roleEfRepo)
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
        public AccountInfoData AccountValid(AccountInfoData accountInfoData)
        {
            //驗證帳號
            if (!_userRepo.FindAccountName(accountInfoData.AccountName).Any())
            {
                accountInfoData.Message = "該帳號不存在。";
                return accountInfoData;
            }

            //驗證密碼
            if (_userRepo.FindAccountData(accountInfoData.AccountName).Password != accountInfoData.Password)
            {
                accountInfoData.Message = "密碼輸入錯誤。";
                return accountInfoData;
            }

            return accountInfoData;

            // Ef
            //驗證帳號
            //if (!_userEfRepo.FindAccountName(accountInfoData.AccountName).Any())
            //{
            //    accountInfoData.Message = "該帳號不存在。";
            //    return accountInfoData;
            //}

            ////驗證密碼
            //if (_userEfRepo.FindAccountData(accountInfoData.AccountName).Password != accountInfoData.Password)
            //{
            //    accountInfoData.Message = "密碼輸入錯誤。";
            //    return accountInfoData;
            //}

            //return accountInfoData;
        }

        /// <summary>
        /// 透過帳號名稱取得帳號資料
        /// </summary>
        /// <param name="accountInfoData"></param>
        /// <returns></returns>
        public UserDTO GetUserDataByAccountName(AccountInfoData accountInfoData)
        {
            return _userRepo.FindAccountData(accountInfoData.AccountName);

            // Ef
            // return _userEfRepo.FindAccountData(accountInfoData.AccountName);
        }

        /// <summary>
        /// 透過UserID取得角色資料
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRoleDataByUserID(string userID)
        {
            return _roleRepo.GetRoleDataByAccountName(userID);

            // Ef
            // return _roleEfRepo.GetRoleDataByAccountName(userID);
        }

        #endregion
    }
}
