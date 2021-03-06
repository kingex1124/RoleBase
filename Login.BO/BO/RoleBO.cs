﻿using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.SQLDAL;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using KevanFramework.DataAccessDAL.SQLDAL.Model;
using Login.DAL;
using Login.DTO;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.BO
{
    public class RoleBO : IRoleBO
    {
        #region 屬性

        IRoleRepository _roleRepo;
        IRoleUserRepository _roleUserRepo;
        IRoleFunctionRepository _roleFunctionRepo;
        ISQLTransactionHelper _sqlConnectionHelper;

        #endregion

        #region 建構子

        public RoleBO()
        {
            _roleRepo = new RoleRepository();
            _roleUserRepo = new RoleUserRepository();
            _roleFunctionRepo = new RoleFunctionRepository();
            _sqlConnectionHelper = new SQLTransactionHelper(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);
        }

        public RoleBO(IRoleRepository roleRepo, IRoleUserRepository roleUserRepo, IRoleFunctionRepository roleFunctionRepo, ISQLTransactionHelper sqlConnectionHelper)
        {
            _roleRepo = roleRepo;
            _roleUserRepo = roleUserRepo;
            _roleFunctionRepo = roleFunctionRepo;
            _sqlConnectionHelper = sqlConnectionHelper;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得Role資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleVO> GetRoleData(PageDataVO pageDataVO)
        {
            pageDataVO.PageSize = pageDataVO.PageSize ?? Convert.ToInt32(ConfigurationManager.AppSettings["TablePageCount"]);

            pageDataVO.DataCount = _roleRepo.GetRoleCount(pageDataVO);

            if (pageDataVO.PageSize != null && pageDataVO.PageSize != 0)
            {
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
            }
            else
            {
                pageDataVO.UpperBound = pageDataVO.DataCount + 1;
                pageDataVO.LowerBound = 0;
            }
            if (string.IsNullOrEmpty(pageDataVO.OrderByColumn))
                pageDataVO.OrderByColumn = "RoleID";

            IEnumerable<RoleVO> result = Utility.MigrationIEnumerable<RoleDTO, RoleVO>(_roleRepo.GetRoleData(pageDataVO));

            return result;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public string AddRole(RoleVO roleVO)
        {
            int result = _roleRepo.AddRole(roleVO);

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

            SQLConnTran sqlConnTran = _sqlConnectionHelper.BeginTransaction();

            int deleteRoleUserResult = _roleUserRepo.DeleteRoleUserByRoleID(id, ref sqlConnTran.SqlConn, ref sqlConnTran.SqlTrans);

            int deleteRoleFunctionResult = _roleFunctionRepo.DeleteRoleFunctionByRoleID(id, ref sqlConnTran.SqlConn, ref sqlConnTran.SqlTrans);

            int deleteRoleResult = _roleRepo.DeleteRole(id, ref sqlConnTran.SqlConn, ref sqlConnTran.SqlTrans);

            if (deleteRoleUserResult >= 0 && deleteRoleFunctionResult >= 0 && deleteRoleResult > 0)
                result = "";
            else
                result = "刪除失敗。";

            _sqlConnectionHelper.Commit(); // tran.Commit();

            return result;
        }

        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        public string EditRole(RoleVO roleVO)
        {
            int result = _roleRepo.EditRole(roleVO);
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
        public IEnumerable<UserCheckVO> GetUserCheckByRole(string roleID, PageDataVO pageDataVO)
        {
            IEnumerable<UserCheckVO> result = Utility.MigrationIEnumerable<UserCheckDTO, UserCheckVO>(_roleUserRepo.GetUserCheckByRole(roleID, pageDataVO));
            return result;
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

                SQLConnTran sqlConnTran = _sqlConnectionHelper.BeginTransaction();

                int deleteResult = _roleUserRepo.DeleteRoleUserByRoleID(roleID, ref sqlConnTran.SqlConn, ref sqlConnTran.SqlTrans);

                if (deleteResult < 0)
                {
                    _sqlConnectionHelper.Rollback();// tran.Rollback();
                    result = "刪除失敗。";
                    return result;
                }

                int insertResult = 0;
                foreach (var item in roleUserDTOs)
                    insertResult += _roleUserRepo.InsertRoleUser(item, ref sqlConnTran.SqlConn, ref sqlConnTran.SqlTrans);

                _sqlConnectionHelper.Commit();

                if (insertResult < 0)
                {
                    _sqlConnectionHelper.Rollback();// tran.Rollback();
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

            SQLConnTran sqlConnTran = _sqlConnectionHelper.BeginTransaction();

            int deleteResult = _roleUserRepo.DeleteRoleUserByRoleID(roleID, ref sqlConnTran.SqlConn, ref sqlConnTran.SqlTrans);

            if (deleteResult < 0)
            {
                _sqlConnectionHelper.Rollback();  //tran.Rollback();
                result = "刪除失敗。";
            }

            _sqlConnectionHelper.Commit(); //tran.Commit();

            return result;
        }

        #endregion
    }
}
