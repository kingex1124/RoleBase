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
using System.Transactions;

namespace Login.BO
{
    public class RoleEfBO : IRoleBO
    {
        #region 屬性

        IRoleEfRepository _roleEfRepo;
        IRoleUserEfRepository _roleUserEfRepo;
        IRoleFunctionEfRepository _roleFunctionEfRepo;

        #endregion

        #region 建構子

        public RoleEfBO()
        {
            _roleEfRepo = new RoleEfRepository(new RoleBaseEntities());
            _roleUserEfRepo = new RoleUserEfRepository(new RoleBaseEntities());
            _roleFunctionEfRepo = new RoleFunctionEfRepository(new RoleBaseEntities());
        }

        public RoleEfBO(IRoleEfRepository roleEfRep, IRoleUserEfRepository roleUserEfRep, IRoleFunctionEfRepository roleFunctionEfRepo)
        {
            _roleEfRepo = roleEfRep;
            _roleUserEfRepo = roleUserEfRep;
            _roleFunctionEfRepo = roleFunctionEfRepo;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得Role資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleVO> GetRoleData(PageDataVO pageDataVO)
        {
            pageDataVO.PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TablePageCount"]);

            pageDataVO.DataCount = _roleEfRepo.GetRoleCount(pageDataVO);

            if (pageDataVO.DataCount % pageDataVO.PageSize.Value == 0)
                pageDataVO.AllPageNumber = pageDataVO.DataCount / pageDataVO.PageSize.Value;
            else
                pageDataVO.AllPageNumber = pageDataVO.DataCount / pageDataVO.PageSize.Value + 1;

            pageDataVO.LowerBound = (pageDataVO.PageNumber - 1) * pageDataVO.PageSize.Value;
            pageDataVO.UpperBound = pageDataVO.LowerBound + pageDataVO.PageSize.Value + 1;
            if (pageDataVO.LowerBound >= pageDataVO.DataCount)
            {
                pageDataVO.UpperBound = pageDataVO.DataCount + 1;
                pageDataVO.LowerBound = pageDataVO.UpperBound - (pageDataVO.PageSize.Value + 1);
            }

            return Utility.MigrationIEnumerable<RoleDTO, RoleVO>(_roleEfRepo.GetRoleData(pageDataVO));
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public string AddRole(RoleVO roleVO)
        {
            int result = _roleEfRepo.AddRole(roleVO);

            if (result > 0)
                return "";
            else
                return "新增失敗。";
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteRole(string id)
        {
            string result = string.Empty;

            using (TransactionScope ts = new TransactionScope())
            {
                int deleteRoleUserResult = _roleUserEfRepo.DeleteRoleUserByRoleID(id);

                int deleteRoleFunctionResult = _roleFunctionEfRepo.DeleteRoleFunctionByRoleID(id);

                int deleteRoleResult = _roleEfRepo.DeleteRole(id);

                if (deleteRoleUserResult >= 0 && deleteRoleFunctionResult >= 0 && deleteRoleResult > 0)
                    result = "";
                else
                    result = "刪除失敗。";

                ts.Complete();
            }

            return result;
        }

        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public string EditRole(RoleVO roleVO)
        {
            int result = _roleEfRepo.EditRole(roleVO);
            if (result > 0)
                return "";
            else
                return "編輯失敗。";
        }

        /// <summary>
        /// 取得角色設定使用者的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IEnumerable<UserCheckVO> GetUserCheckByRole(string roleID)
        {
            return Utility.MigrationIEnumerable<UserCheckDTO, UserCheckVO>(_roleUserEfRepo.GetUserCheckByRole(roleID).ToList());
        }

        /// <summary>
        /// 角色編輯使用者
        /// 儲存勾選使用者時的變更
        /// </summary>
        /// <param name="userCheckVO"></param>
        /// <returns></returns>
        public string SaveRoleUserSetting(IEnumerable<UserCheckVO> userCheckVO)
        {
            string result = string.Empty;
            string roleID;
            if (userCheckVO != null && userCheckVO.Any())
            {
                roleID = userCheckVO.First().RoleID.ToString();
                List<RoleUserDTO> roleUserDTOs = new List<RoleUserDTO>();
                foreach (var item in userCheckVO)
                {
                    RoleUserDTO roleUserDTO = new RoleUserDTO();
                    roleUserDTO.RoleID = item.RoleID;
                    roleUserDTO.UserID = item.UserID;
                    roleUserDTOs.Add(roleUserDTO);
                }

                using (TransactionScope ts = new TransactionScope())
                {
                    int deleteResult = _roleUserEfRepo.DeleteRoleUserByRoleID(roleID);

                    if (deleteResult < 0)
                    {
                        result = "刪除失敗。";
                        return result;
                    }

                    int insertResult = 0;
                    foreach (var item in roleUserDTOs)
                        insertResult += _roleUserEfRepo.InsertRoleUser(item);

                    ts.Complete();
                    if (insertResult < 0)
                        result = "設定失敗。";
                }
            }

            return result;
        }

        /// <summary>
        /// 角色編輯使用者
        /// 儲存清空使用者時的變更
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public string ClearRoleUserByRoleID(string roleID)
        {
            string result = string.Empty;

            using (TransactionScope ts = new TransactionScope())
            {
                int deleteResult = _roleUserEfRepo.DeleteRoleUserByRoleID(roleID);

                if (deleteResult < 0)
                    result = "刪除失敗。";

                ts.Complete();
            }

            return result;
        }

        #endregion
    }
}
