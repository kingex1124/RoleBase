using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.SQLDAL;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LoginServerBO.BO
{
    public class FunctionBO : IFunctionBO
    {
        #region 屬性

        IFunctionRepository _functionRepo;
        IRoleFunctionRepository _roleFunctionRepo;
        ISQLTransactionHelper _sqlConnectionHelper;

        IFunctionEfRepository _functionEfRepo;
        IRoleFunctionEfRepository _roleFunctionEfRepo;

        #endregion

        #region 建構子

        public FunctionBO()
        {
            _functionRepo = new FunctionRepository();
            _roleFunctionRepo = new RoleFunctionRepository();
            _sqlConnectionHelper = new SQLTransactionHelper(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);

            _functionEfRepo = new FunctionEfRepository(new RoleBaseEntities());
            _roleFunctionEfRepo = new RoleFunctionEfRepository(new RoleBaseEntities());
        }

        public FunctionBO(IFunctionRepository functionRepo, IRoleFunctionRepository roleFunctionRepo, ISQLTransactionHelper sqlConnectionHelper)
        {
            _functionRepo = functionRepo;
            _roleFunctionRepo = roleFunctionRepo;
            _sqlConnectionHelper = sqlConnectionHelper;
        }

        public FunctionBO(IFunctionEfRepository functionEfRepo, IRoleFunctionEfRepository roleFunctionEfRepo)
        {
            _functionEfRepo = functionEfRepo;
            _roleFunctionEfRepo = roleFunctionEfRepo;
        }

        #endregion

        #region 方法  

        /// <summary>
        /// 取得Function資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionVO> GetFunctionData()
        {
            IEnumerable<FunctionVO> result = Utility.MigrationIEnumerable<FunctionDTO, FunctionVO>(_functionRepo.GetFunctionData());

            return result;

            // EF
            // return Utility.MigrationIEnumerable<FunctionDTO, FunctionVO>(_functionEfRepo.GetFunctionData()); 
        }

        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public string AddFunction(FunctionVO functionVO)
        {
            int result = _functionRepo.AddFunction(functionVO);

            if (result > 0)
                return "";
            else
                return "新增失敗。";

            // Ef
            //int result = _functionEfRepo.AddFunction(functionVO);

            //if (result > 0)
            //    return "";
            //else
            //    return "新增失敗。";
        }

        /// <summary>
        /// 刪除功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteFunction(string id)
        {
            string result = string.Empty;

            var sqlConnTrans = _sqlConnectionHelper.BeginTransaction();

            int deleteRoleFunctionResult = _roleFunctionRepo.DeleteRoleFunctionByFunctionID(id, ref sqlConnTrans.SqlConn, ref sqlConnTrans.SqlTrans);

            int deleteFunctionResult = _functionRepo.DeleteFunction(id, ref sqlConnTrans.SqlConn, ref sqlConnTrans.SqlTrans);

            if (deleteRoleFunctionResult >= 0 && deleteFunctionResult > 0)
                result = "";
            else
                result = "刪除失敗。";

            _sqlConnectionHelper.Commit();

            return result;

            // Ef
            //string result = string.Empty;

            //using (TransactionScope ts = new TransactionScope())
            //{

            //    int deleteRoleFunctionResult = _roleFunctionEfRepo.DeleteRoleFunctionByFunctionID(id);

            //    int deleteFunctionResult = _functionEfRepo.DeleteFunction(id);

            //    if (deleteRoleFunctionResult >= 0 && deleteFunctionResult > 0)
            //        result = "";
            //    else
            //        result = "刪除失敗。";

            //    ts.Complete();
            //}

            //return result;
        }

        /// <summary>
        /// 編輯功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public string EditFunction(FunctionVO functionVO)
        {
            int result = _functionRepo.EditFunction(functionVO);
            if (result > 0)
                return "";
            else
                return "編輯失敗";

            // Ef
            //int result = _functionEfRepo.EditFunction(functionVO);
            //if (result > 0)
            //    return "";
            //else
            //    return "編輯失敗";
        }

        /// <summary>
        /// 取得角色設定功能的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IEnumerable<FunctionCheckVO> GetFunctionCheckByRole(string roleID)
        {
            IEnumerable<FunctionCheckVO> result = Utility.MigrationIEnumerable<FunctionCheckDTO, FunctionCheckVO>(_roleFunctionRepo.GetFunctionCheckByRole(roleID));
            return result;

            // Ef
            // return Utility.MigrationIEnumerable<FunctionCheckDTO, FunctionCheckVO>(_roleFunctionEfRepo.GetFunctionCheckByRole(roleID));
        }

        /// <summary>
        /// 角色編輯功能
        /// 儲存勾選功能時的變更
        /// </summary>
        /// <param name="functionCheckVO"></param>
        /// <returns></returns>
        public string SaveRoleFunctionSetting(IEnumerable<FunctionCheckVO> functionCheckVO)
        {
            string result = string.Empty;
            string roleID;

            if (functionCheckVO != null && functionCheckVO.Any())
            {
                roleID = functionCheckVO.First().RoleID.ToString();
                List<RoleFunctionDTO> roleFunctionDTOs = new List<RoleFunctionDTO>();
                foreach (var item in functionCheckVO)
                {
                    RoleFunctionDTO roleFunctionDTO = new RoleFunctionDTO();
                    roleFunctionDTO.RoleID = item.RoleID;
                    roleFunctionDTO.FunctionID = item.FunctionID;
                    roleFunctionDTOs.Add(roleFunctionDTO);
                }

                var sqlConnTrans = _sqlConnectionHelper.BeginTransaction();

                int deleteResult = _roleFunctionRepo.DeleteRoleFunctionByRoleID(roleID, ref sqlConnTrans.SqlConn, ref sqlConnTrans.SqlTrans);

                if (deleteResult < 0)
                {
                    _sqlConnectionHelper.Rollback();
                    result = "刪除失敗。";
                    return result;
                }

                int insertResult = 0;
                foreach (var item in roleFunctionDTOs)
                    insertResult += _roleFunctionRepo.InsertRoleFunction(item, ref sqlConnTrans.SqlConn, ref sqlConnTrans.SqlTrans);

                _sqlConnectionHelper.Commit();

                if (insertResult < 0)
                {
                    _sqlConnectionHelper.Rollback();
                    result = "設定失敗。";
                }
            }
            return result;

            // Ef
            //string result = string.Empty;
            //string roleID;

            //if (functionCheckVO != null && functionCheckVO.Any())
            //{
            //    roleID = functionCheckVO.First().RoleID.ToString();
            //    List<RoleFunctionDTO> roleFunctionDTOs = new List<RoleFunctionDTO>();
            //    foreach (var item in functionCheckVO)
            //    {
            //        RoleFunctionDTO roleFunctionDTO = new RoleFunctionDTO();
            //        roleFunctionDTO.RoleID = item.RoleID;
            //        roleFunctionDTO.FunctionID = item.FunctionID;
            //        roleFunctionDTOs.Add(roleFunctionDTO);
            //    }

            //    using (TransactionScope ts = new TransactionScope())
            //    {
            //        int deleteResult = _roleFunctionEfRepo.DeleteRoleFunctionByRoleID(roleID);

            //        if (deleteResult < 0)
            //        {
            //            _sqlConnectionHelper.Rollback();
            //            result = "刪除失敗。";
            //            return result;
            //        }

            //        int insertResult = 0;
            //        foreach (var item in roleFunctionDTOs)
            //            insertResult += _roleFunctionEfRepo.InsertRoleFunction(item);

            //        ts.Complete();

            //        if (insertResult < 0)
            //        {
            //            _sqlConnectionHelper.Rollback();
            //            result = "設定失敗。";
            //        }
            //    }
            //}
            //return result;
        }

        /// <summary>
        /// 角色編輯功能
        /// 儲存清空功能的變更
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public string ClearRoleFunctionByRoleID(string roleID)
        {
            string result = string.Empty;

            var sqlConnTrans = _sqlConnectionHelper.BeginTransaction();

            int deleteResult = _roleFunctionRepo.DeleteRoleFunctionByRoleID(roleID, ref sqlConnTrans.SqlConn, ref sqlConnTrans.SqlTrans);

            if (deleteResult < 0)
            {
                _sqlConnectionHelper.Rollback();
                result = "刪除失敗。";
            }
            _sqlConnectionHelper.Commit();

            return result;

            // Ef
            //string result = string.Empty;

            //using (TransactionScope ts = new TransactionScope())
            //{
            //    int deleteResult = _roleFunctionEfRepo.DeleteRoleFunctionByRoleID(roleID);

            //    if (deleteResult < 0)
            //    {
            //        _sqlConnectionHelper.Rollback();
            //        result = "刪除失敗。";
            //    }

            //    ts.Complete();
            //}

            //return result;
        }

        #endregion
    }
}
