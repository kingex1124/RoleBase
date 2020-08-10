using LoginDAL.EfRepository.Interface;
using LoginDTO.DTO;
using LoginDTO.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDAL.EfRepository
{
    public class RoleUserEfRepository : RoleBaseEfContext<RoleUser>, IRoleUserEfRepository
    {
        #region 屬性

        private readonly RoleBaseEntities _db;

        #endregion

        #region 建構子

        public RoleUserEfRepository(RoleBaseEntities db) : base(db)
        {
            _db = db;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得角色設定使用者的資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<UserCheckDTO> GetUserCheckByRole(string id)
        {
            int idData = Convert.ToInt32(id);
            var result = (from user in _db.User
                          join selectRoleUser in (from roleUser in _db.RoleUser where roleUser.RoleID == idData select roleUser)
                          on user.UserID equals selectRoleUser.UserID
                          into joined
                          from j in joined.DefaultIfEmpty()
                          orderby user.UserID
                          select new UserCheckDTO()
                          {
                              UserID = user.UserID,
                              AccountName = user.AccountName,
                              UserName = user.UserName,
                              Check = j.RoleID == null ? false : true
                          });

            return result;
        }

        /// <summary>
        /// 透過角色ID清空RoleUer的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteRoleUserByRoleID(string roleID)
        {
            int roleIDData = Convert.ToInt32(roleID);
            var deleteData = _db.RoleUser.Where(o => o.RoleID == roleIDData);

            foreach (var item in deleteData)
                Delete(item);

            return SaveChanges();
        }

        /// <summary>
        /// 透過角色ID新增RoleUser的資料
        /// </summary>
        /// <param name="roleUserDTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int InsertRoleUser(RoleUserDTO roleUserDTO)
        {
            Insert(new RoleUser()
            {
                RoleID = roleUserDTO.RoleID,
                UserID = roleUserDTO.UserID
            });

            return SaveChanges();
        }

        #endregion
    }
}
