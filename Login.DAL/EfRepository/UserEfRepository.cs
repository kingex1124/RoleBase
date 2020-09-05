using Login.DTO;
using Login.DTO.EFModel;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public class UserEfRepository : RoleBaseEfContext<User>, IUserEfRepository
    {
        private readonly RoleBaseEntities _db;

        public UserEfRepository(RoleBaseEntities db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// 查找帳號
        /// 驗證帳號是否重複
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public IEnumerable<UserDTO> FindAccountName(string accountName)
        {
            RoleBaseEntities _dba = new RoleBaseEntities();
            var test = _dba.User.ToList();
            var result = (from user in _db.User
                          //where user.AccountName == accountName
                          select new UserDTO()
                          {
                              UserID = user.UserID,
                              AccountName = user.AccountName,
                              Password = user.Password,
                              UserName = user.UserName,
                              Phone = user.Phone,
                              Email = user.Email
                          });

            return result;
        }

        /// <summary>
        /// 取得該帳號資料
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public UserDTO FindAccountData(string accountName)
        {
            var result = (from user in _db.User
                          where user.AccountName == accountName
                          select new UserDTO()
                          {
                              UserID = user.UserID,
                              AccountName = user.AccountName,
                              Password = user.Password,
                              UserName = user.UserName,
                              Phone = user.Phone,
                              Email = user.Email
                          });
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int UserInsert(Account account)
        {
            Insert(new User()
            {
                AccountName = account.AccountName,
                Password = account.Password,
                UserName = account.UserName,
                Phone = account.Phone,
                Email = account.Email
            });

            return SaveChanges();
        }
    }
}
