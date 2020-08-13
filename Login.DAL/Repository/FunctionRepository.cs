using KevanFramework.DataAccessDAL.Common;
using KevanFramework.DataAccessDAL.SQLDAL;
using KevanFramework.DataAccessDAL.SQLDAL.Interface;
using Login.DTO;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public class FunctionRepository : IFunctionRepository
    {
        #region 屬性

        private IDataAccess _dataAccess = null;

        #endregion

        #region 建構子

        public FunctionRepository()
        {
            UnityContainer.Register<IDataAccess, DataAccess>();
            _dataAccess = UnityContainer.Resolve<IDataAccess>("AccountConn");
        }

        public FunctionRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得Function資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionDTO> GetFunctionData()
        {
            string sqlStr = @"Select *, 
case when A.[Parent] = -1
then 'Not Menu'
when  A.[Parent] = 0 
then 'No'
else (select B.[Title] from [Function] B where B.FunctionID = A.[Parent]) end as 'ParentName'
From [Function] A Order by FunctionID ";

            return _dataAccess.QueryDataTable<FunctionDTO>(sqlStr);
        }

        /// <summary>
        /// 取得作為上層的keyValue資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePairDTO> GetParentKeyValue()
        {
            string sqlStr = @"Select FunctionID as 'Key',Title as 'Value'
From [Function] A 
where A.IsMenu = 1
Order by A.[FunctionID] ";

            return _dataAccess.QueryDataTable<KeyValuePairDTO>(sqlStr);
        }

        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public int AddFunction(FunctionVO functionVO)
        {
            List<string> param = new List<string>();
            string sqlStr = @"Insert Into [Function] (Url,Description,IsMenu,Parent,Title) 
                              Values(@p0,@p1,@p2,@p3,@p4) ";
            param.Add(functionVO.Url);
            param.Add(functionVO.Description);
            param.Add(functionVO.IsMenu.ToString());
            param.Add(functionVO.Parent.ToString());
            param.Add(functionVO.Title);

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

            return _dataAccess.ExcuteSQL(sqlStr, ref conn, ref tran, param.ToArray());
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
                            Set Url = @p0 , Title = @p1 , Description = @p2 , IsMenu = @p3 , Parent = @p4
                            Where FunctionID = @p5 ";

            param.Add(functionVO.Url);
            param.Add(functionVO.Title);
            param.Add(functionVO.Description);
            param.Add(functionVO.IsMenu.ToString());
            param.Add(functionVO.Parent.ToString());
            param.Add(functionVO.FunctionID.ToString());

            return _dataAccess.ExcuteSQL(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 取得Menu資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionMenuDTO> GetMenuData(string userID)
        {
            List<string> param = new List<string>();

            string sqlStr = @"Select distinct A.FunctionID , A.Url , A.Parent, A.Title
                              From [dbo].[RoleUser] B 
							  Join [dbo].[RoleFunction] C 
							  on B.[RoleID] = C.RoleID  
							  Join [dbo].[Function] A
							  On A.FunctionID = C.FunctionID
                              where A.IsMenu = 1 And B.UserID = @p0
                              Order by A.[FunctionID]  ";

            param.Add(userID);

            return _dataAccess.QueryDataTable<FunctionMenuDTO>(sqlStr, param.ToArray());
        }

        #endregion
    }
}
