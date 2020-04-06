using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.Interface;
using KevanFramework.DataAccessDAL.SQLDAL;
using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO
{
    public class FunctionBO
    {
        private DataAccess _dataAccess = null;

        public FunctionBO()
        {
            DataAccessIO.Register<IDataAccess, DataAccess>();
            _dataAccess = (DataAccess)DataAccessIO.Resolve<IDataAccess>("AccountConn");
        }

        /// <summary>
        /// 取得Function資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionDTO> GetFunctionData()
        {
            string sqlStr = "Select * From [Function] Order by FunctionID ";

            return _dataAccess.QueryDataTable<FunctionDTO>(sqlStr);
        }

        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public int AddFunction(FunctionVO functionVO)
        {
            List<string> param = new List<string>();
            string sqlStr = @"Insert Into [Function] (Url,Description) 
                              Values(@p0,@p1) ";
            param.Add(functionVO.Url);
            param.Add(functionVO.Description);

            return _dataAccess.ExcuteSQL(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteFunction(string id, ref SqlConnection conn, ref SqlTransaction tran)
        {
            List<string> param = new List<string>();
            string sqlStr = @"Delete [Function]  Where FunctionID = @p0 ";

            param.Add(id);

            return _dataAccess.ExcuteSQL(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 透過FunctionID刪除RoleFunction資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteRoleFunctionByFunctionID(string functionID, ref SqlConnection conn, ref SqlTransaction tran)
        {
            try
            {
                List<string> param = new List<string>();

                string sqlStr = @"Delete [RoleFunction] where functionID = @p0 ";

                param.Add(functionID);

                return _dataAccess.ExcuteSQL(sqlStr, ref conn, ref tran, param.ToArray());
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 編輯功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public int EditFunction(FunctionVO functionVO)
        {
            List<string> param = new List<string>();
            string sqlStr = @"Update [Function]  
                            Set Url = @p0 , Description = @p1
                            Where FunctionID = @p2 ";

            param.Add(functionVO.Url);
            param.Add(functionVO.Description);
            param.Add(functionVO.FunctionID.ToString());

            return _dataAccess.ExcuteSQL(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 取得角色設定功能的資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<FunctionCheckDTO> GetFunctionCheckByRole(string id)
        {
            List<string> param = new List<string>();

            string sqlStr = @"SELECT 
case when A.[RoleID] IS NULL then CAST(0 AS BIT) Else CAST(1 AS BIT) end AS 'Check' ,
      B.[FunctionID],B.Url,B.Description
  FROM 
  (Select * From [RoleFunction] where RoleID=@p0) A 
  Right join [Function] B on A.FunctionID = B.FunctionID 
  Order By B.[FunctionID] ";

            param.Add(id);

            return _dataAccess.QueryDataTable<FunctionCheckDTO>(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 透過角色ID清空RoleFunciton的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteRoleFunctionByRoleID(string roleID , ref SqlConnection conn, ref SqlTransaction tran)
        {
            try
            {
                List<string> param = new List<string>();

                string sqlStr = @"Delete [RoleFunction] where RoleID = @p0 ";

                param.Add(roleID);

                return _dataAccess.ExcuteSQL(sqlStr, ref conn, ref tran, param.ToArray());
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 透過角色ID新增RoleFunction資料
        /// </summary>
        /// <param name="roleFunctionDTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int InsertRoleFunction(RoleFunctionDTO roleFunctionDTO, ref SqlConnection conn, ref SqlTransaction tran)
        {
            try
            {
                List<string> param1 = new List<string>();

                string sqlStr1 = @"Insert Into [dbo].[RoleFunction] (RoleID,FunctionID) Values(@p0,@p1)";

                param1.Add(roleFunctionDTO.RoleID.ToString());
                param1.Add(roleFunctionDTO.FunctionID.ToString());

                return _dataAccess.ExcuteSQL(sqlStr1, ref conn, ref tran, param1.ToArray());
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
