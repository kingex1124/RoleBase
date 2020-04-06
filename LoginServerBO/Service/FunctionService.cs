using KevanFramework.DataAccessDAL.Common;
using LoginDTO.DTO;
using LoginServerBO.BO;
using LoginServerBO.Helper;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Service
{
    public class FunctionService
    {
        FunctionBO functionBO;
        public FunctionService()
        {
            functionBO = new FunctionBO();
        }

        /// <summary>
        /// 取得Function資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionVO> GetFunctionData()
        {
            IEnumerable<FunctionVO> result = Utility.MigrationIEnumerable<FunctionDTO, FunctionVO>(functionBO.GetFunctionData());

            return result;
        }

        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public string AddFunction(FunctionVO functionVO)
        {
            int result = functionBO.AddFunction(functionVO);

            if (result > 0)
                return "";
            else
                return "新增失敗。";
        }

        /// <summary>
        /// 刪除功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteFunction(string id)
        {
            string result = string.Empty;

            SqlConnection conn = new SqlConnection(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);
            conn.Open();

            SqlTransaction tran = conn.BeginTransaction();

            int deleteRoleFunctionResult = functionBO.DeleteRoleFunctionByFunctionID(id, ref conn, ref tran);

            int deleteFunctionResult = functionBO.DeleteFunction(id, ref conn, ref tran);

            if (deleteRoleFunctionResult >= 0 && deleteFunctionResult > 0)
                result = "";
            else
                result = "刪除失敗。";

            tran.Commit();

            return result;
        }

        /// <summary>
        /// 編輯功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public string EditFunction(FunctionVO functionVO)
        {
            int result = functionBO.EditFunction(functionVO);
            if (result > 0)
                return "";
            else
                return "編輯失敗";
        }

        /// <summary>
        /// 取得角色設定功能的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IEnumerable<FunctionCheckVO> GetFunctionCheckByRole(string roleID)
        {
            IEnumerable<FunctionCheckVO> result = Utility.MigrationIEnumerable<FunctionCheckDTO, FunctionCheckVO>(functionBO.GetFunctionCheckByRole(roleID));
            return result;
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

                SqlConnection conn = new SqlConnection(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);
                conn.Open();

                SqlTransaction tran = conn.BeginTransaction();

                int deleteResult = functionBO.DeleteRoleFunctionByRoleID(roleID, ref conn, ref tran);

                if (deleteResult < 0)
                {
                    tran.Rollback();
                    result = "刪除失敗。";
                    return result;
                }

                int insertResult = 0;
                foreach (var item in roleFunctionDTOs)
                    functionBO.InsertRoleFunction(item, ref conn, ref tran);

                tran.Commit();

                if (insertResult < 0)
                {
                    tran.Rollback();
                    result = "設定失敗。";
                }
            }
            return result;
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
            SqlConnection conn = new SqlConnection(new DBConnectionString(KevanFramework.DataAccessDAL.Common.Enum.ConnectionType.ConnectionKeyName, "AccountConn").ConnectionString);
            conn.Open();

            SqlTransaction tran = conn.BeginTransaction();
            int deleteResult = functionBO.DeleteRoleFunctionByRoleID(roleID, ref conn, ref tran);

            if (deleteResult < 0)
            {
                tran.Rollback();
                result = "刪除失敗。";
            }
            tran.Commit();

            return result;
        }
    }
}
