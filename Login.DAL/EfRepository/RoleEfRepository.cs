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
    public class RoleEfRepository : RoleBaseEfContext<Role>, IRoleEfRepository
    {
        #region 屬性

        private readonly RoleBaseEntities _db;

        #endregion

        #region 建構子

        public RoleEfRepository(RoleBaseEntities db) : base(db)
        {
            _db = db;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 透過帳號查找角色
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRoleDataByAccountName(string userID)
        {
            int userIDNum = Convert.ToInt32(userID);
            var result = (from user in _db.User
                          join roleUser in _db.RoleUser
                          on user.UserID equals roleUser.UserID
                          join role in _db.Role
                          on roleUser.RoleID equals role.RoleID
                          where user.UserID == userIDNum
                          select new RoleDTO()
                          {
                              RoleID = role.RoleID,
                              RoleName = role.RoleName,
                              Description = role.Description
                          });

            return result;
        }

        /// <summary>
        /// 取得Role資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRoleData(PageDataVO pageDataVO)
        {
            var result = (from role in _db.Role
                          where role.RoleName == pageDataVO.WhereCondition[0].Value
                          orderby role.RoleID
                          select new RoleDTO()
                          {
                              RoleID = role.RoleID,
                              RoleName = role.RoleName,
                              Description = role.Description
                          });

            return result.Skip(pageDataVO.LowerBound).Take(pageDataVO.PageSize == null ? 0 : Convert.ToInt32(pageDataVO.PageSize));
        }

        /// <summary>
        /// 取得資料總筆數
        /// </summary>
        /// <param name="pageDataVO"></param>
        /// <returns></returns>
        public int GetRoleCount(PageDataVO pageDataVO)
        {
            var result = (from role in _db.Role
                          where role.RoleName == pageDataVO.WhereCondition[0].Value
                          orderby role.RoleID
                          select new RoleDTO()
                          {
                              RoleID = role.RoleID,
                              RoleName = role.RoleName,
                              Description = role.Description
                          });

            return result.Count();
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public int AddRole(RoleVO roleVO)
        {
            Insert(new Role()
            {
                RoleName = roleVO.RoleName,
                Description = roleVO.Description
            });

            return SaveChanges();
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteRole(string id)
        {
            DeleteByKey(new object[] { Convert.ToInt32(id) });

            return SaveChanges();
        }

        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>

        public int EditRole(RoleVO roleVO)
        {
            var roleData = _db.Role.Where(o => o.RoleID == roleVO.RoleID).FirstOrDefault();
            roleData.RoleName = roleVO.RoleName;
            roleData.Description = roleVO.Description;
            Update(roleData);

            return SaveChanges();
        }

        #endregion
    }
}
