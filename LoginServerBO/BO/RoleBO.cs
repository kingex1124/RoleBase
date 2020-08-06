using AutoMapper;
using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.SQLDAL;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using KevanFramework.DataAccessDAL.SQLDAL.Model;
using LoginDTO.DTO;
using LoginDTO.EFModel;
using LoginServerBO.BO.Interface;
using LoginServerBO.EfRepository;
using LoginServerBO.EfRepository.Interface;
using LoginServerBO.Helper;
using LoginServerBO.Repository;
using LoginServerBO.Repository.Interface;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LoginServerBO.BO
{
    public class RoleBO : IRoleBO
    {
        #region 屬性

        IRoleRepository _roleRepo;
        IRoleUserRepository _roleUserRepo;
        IRoleFunctionRepository _roleFunctionRepo;
        ISQLTransactionHelper _sqlConnectionHelper;

        IRoleEfRepository _roleEfRepo;
        IRoleUserEfRepository _roleUserEfRepo;
        IRoleFunctionEfRepository _roleFunctionEfRepo;

        #endregion

        #region 建構子

        public RoleBO()
        {
            _roleRepo = new RoleRepository();
            _roleUserRepo = new RoleUserRepository();
            _roleFunctionRepo = new RoleFunctionRepository();
            _sqlConnectionHelper = new SQLTransactionHelper(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);

            _roleEfRepo = new RoleEfRepository(new RoleBaseEntities());
            _roleUserEfRepo = new RoleUserEfRepository(new RoleBaseEntities());
            _roleFunctionEfRepo = new RoleFunctionEfRepository(new RoleBaseEntities());
        }

        public RoleBO(IRoleRepository roleRepo, IRoleUserRepository roleUserRepo, IRoleFunctionRepository roleFunctionRepo , ISQLTransactionHelper sqlConnectionHelper)
        {
            _roleRepo = roleRepo;
            _roleUserRepo = roleUserRepo;
            _roleFunctionRepo = roleFunctionRepo;
            _sqlConnectionHelper = sqlConnectionHelper;
        }

        public RoleBO(IRoleEfRepository roleEfRep, IRoleUserEfRepository roleUserEfRep , IRoleFunctionEfRepository roleFunctionEfRepo)
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
        public IEnumerable<RoleVO> GetRoleData()
        {
            IEnumerable<RoleVO> result = Utility.MigrationIEnumerable<RoleDTO, RoleVO>(_roleRepo.GetRoleData());

            return result;

            // EF
            //return Utility.MigrationIEnumerable<RoleDTO, RoleVO>(_roleEfRepo.GetRoleData()); 
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

            // EF
            //int result = _roleEfRepo.AddRole(roleVO);

            //if (result > 0)
            //    return "";
            //else
            //    return "新增失敗。";
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

            // EF
            //string result = string.Empty;

            //using (TransactionScope ts = new TransactionScope())
            //{
            //    int deleteRoleUserResult = _roleUserEfRepo.DeleteRoleUserByRoleID(id);

            //    int deleteRoleFunctionResult = _roleFunctionEfRepo.DeleteRoleFunctionByRoleID(id);

            //    int deleteRoleResult = _roleEfRepo.DeleteRole(id);

            //    if (deleteRoleUserResult >= 0 && deleteRoleFunctionResult >= 0 && deleteRoleResult > 0)
            //        result = "";
            //    else
            //        result = "刪除失敗。";

            //    ts.Complete();
            //}

            //return result;
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

            // Ef
            //int result = _roleEfRepo.EditRole(roleVO);
            //if (result > 0)
            //    return "";
            //else
            //    return "編輯失敗。";
        }

        /// <summary>
        /// 取得角色設定使用者的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IEnumerable<UserCheckVO> GetUserCheckByRole(string roleID)
        {
            IEnumerable<UserCheckVO> result = Utility.MigrationIEnumerable<UserCheckDTO, UserCheckVO>(_roleUserRepo.GetUserCheckByRole(roleID));
            return result;

            // Ef
            //return Utility.MigrationIEnumerable<UserCheckDTO, UserCheckVO>(_roleUserEfRepo.GetUserCheckByRole(roleID).ToList());
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

            // Ef
            //string result = string.Empty;
            //string roleID;
            //if (userCheckVO != null && userCheckVO.Any())
            //{
            //    roleID = userCheckVO.First().RoleID.ToString();
            //    List<RoleUserDTO> roleUserDTOs = new List<RoleUserDTO>();
            //    foreach (var item in userCheckVO)
            //    {
            //        RoleUserDTO roleUserDTO = new RoleUserDTO();
            //        roleUserDTO.RoleID = item.RoleID;
            //        roleUserDTO.UserID = item.UserID;
            //        roleUserDTOs.Add(roleUserDTO);
            //    }

            //    using (TransactionScope ts = new TransactionScope())
            //    {
            //        int deleteResult = _roleUserEfRepo.DeleteRoleUserByRoleID(roleID);

            //        if (deleteResult < 0)
            //        {
            //            result = "刪除失敗。";
            //            return result;
            //        }

            //        int insertResult = 0;
            //        foreach (var item in roleUserDTOs)
            //            insertResult += _roleUserEfRepo.InsertRoleUser(item);

            //        ts.Complete();
            //        if (insertResult < 0)
            //            result = "設定失敗。";
            //    }
            //}

            //return result;
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

            // Ef
            //string result = string.Empty;

            //using (TransactionScope ts = new TransactionScope())
            //{
            //    int deleteResult = _roleUserEfRepo.DeleteRoleUserByRoleID(roleID);

            //    if (deleteResult < 0)
            //    {
            //        _sqlConnectionHelper.Rollback();  //tran.Rollback();
            //        result = "刪除失敗。";
            //    }

            //    ts.Complete();
            //}

            //return result;
        }

        #endregion
    }
}
