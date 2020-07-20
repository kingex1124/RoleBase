using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.Interface;
using KevanFramework.DataAccessDAL.SQLDAL;
using LoginDTO.DTO;
using LoginServerBO.Repository.Interface;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Repository
{
    public class FunctionRepository : IFunctionRepository
    {
        #region 屬性

        private DataAccess _dataAccess = null;

        #endregion

        #region 建構子

        public FunctionRepository()
        {
            DataAccessIO.Register<IDataAccess, DataAccess>();
            _dataAccess = (DataAccess)DataAccessIO.Resolve<IDataAccess>("AccountConn");
        }

        #endregion

        #region 方法

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
        /// 刪除功能
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

        #endregion
    }
}
