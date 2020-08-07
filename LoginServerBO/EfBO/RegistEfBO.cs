﻿using LoginDTO.EFModel;
using LoginServerBO.BO.Interface;
using LoginServerBO.EfRepository;
using LoginServerBO.EfRepository.Interface;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.EfBO
{
    public class RegistEfBO : IRegistBO
    {
        #region 屬性

        private IUserEfRepository _userEfRepo;

        #endregion

        #region 建構子

        public RegistEfBO()
        {     
            _userEfRepo = new UserEfRepository(new RoleBaseEntities());
        }

        public RegistEfBO(IUserEfRepository userEfRep)
        {
            _userEfRepo = userEfRep;
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
            if (_userEfRepo.FindAccountName(account.AccountName).Any())
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
            if (_userEfRepo.UserInsert(account) > 0)
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