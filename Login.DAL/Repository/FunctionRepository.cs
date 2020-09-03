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
            _dataAccess = UnityContainer.Resolve<IDataAccess, DataAccess>("AccountConn");
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
        public IEnumerable<FunctionDTO> GetFunctionData(PageDataVO pageDataVO)
        {
            List<string> param = new List<string>();

            string condition = string.Empty;

            if (pageDataVO.WhereCondition != null)
            {
                for (int i = 0; i < pageDataVO.WhereCondition.Count; i++)
                {
                    if (i != pageDataVO.WhereCondition.Count - 1)
                    {
                        if (pageDataVO.WhereCondition[i].Key == "IsMenu")
                        {
                            condition = condition + pageDataVO.WhereCondition[i].Key + " = @p" + i.ToString() + " And ";
                            param.Add(pageDataVO.WhereCondition[i].Value);
                            continue;
                        }
                        else
                            condition = condition + pageDataVO.WhereCondition[i].Key + " like @p" + i.ToString() + " And ";
                    }
                    else
                    {
                        if (pageDataVO.WhereCondition[i].Key == "IsMenu")
                        {
                            condition = condition + pageDataVO.WhereCondition[i].Key + " = @p" + i.ToString() + " ";
                            param.Add(pageDataVO.WhereCondition[i].Value);
                            continue;
                        }
                        condition = condition + pageDataVO.WhereCondition[i].Key + " like @p" + i.ToString() + " ";
                    }
                    param.Add("%" + pageDataVO.WhereCondition[i].Value + "%");
                }
            }
            else
                condition = "1=1";

            string sqlStr = string.Format(@"Select [FunctionID], [Url], [Description], [IsMenu], [Parent], [Title] , 
case when A.[Parent] = -1
then 'Not Menu'
when  A.[Parent] = 0 
then 'No'
else (select B.[Title] from [Function] B where B.FunctionID = A.[Parent]) end as 'ParentName'
From (Select ROW_NUMBER() OVER(ORDER BY {0} {1} ) AS row, * from [Function] where {2} ) as A
 where row > @p{3}  and row < @p{4} ", pageDataVO.OrderByColumn, pageDataVO.OrderByType, condition, param.Count, param.Count + 1);

            param.Add(pageDataVO.LowerBound.ToString());
            param.Add(pageDataVO.UpperBound.ToString());

            return _dataAccess.QueryDataTable<FunctionDTO>(sqlStr, param.ToArray());
        }

        /// <summary>
        /// 取得功能資料總筆數
        /// </summary>
        /// <param name="pageDataVO"></param>
        /// <returns></returns>
        public int GetFunctionCount(PageDataVO pageDataVO)
        {
            List<string> param = new List<string>();

            string condition = string.Empty;

            if (pageDataVO.WhereCondition != null)
            {
                for (int i = 0; i < pageDataVO.WhereCondition.Count; i++)
                {
                    if (i != pageDataVO.WhereCondition.Count - 1)
                    {
                        if (pageDataVO.WhereCondition[i].Value == "False" || pageDataVO.WhereCondition[i].Value == "True")
                        {
                            condition = condition + pageDataVO.WhereCondition[i].Key + " = @p" + i.ToString() + " And ";
                            param.Add(pageDataVO.WhereCondition[i].Value);
                            continue;
                        }
                        else
                            condition = condition + pageDataVO.WhereCondition[i].Key + " like @p" + i.ToString() + " And ";
                    }
                    else
                    {
                        if (pageDataVO.WhereCondition[i].Value == "False" || pageDataVO.WhereCondition[i].Value == "True")
                        {
                            condition = condition + pageDataVO.WhereCondition[i].Key + " = @p" + i.ToString() + " ";
                            param.Add(pageDataVO.WhereCondition[i].Value);
                            continue;
                        }
                        condition = condition + pageDataVO.WhereCondition[i].Key + " like @p" + i.ToString() + " ";
                    }
                    param.Add("%" + pageDataVO.WhereCondition[i].Value + "%");
                }
            }
            else
                condition = "1=1";

            string sqlStr = string.Format(@"Select count(*) From [Function] where {0}", condition);

            return (int)_dataAccess.ExecuteScalar(sqlStr, param.ToArray());
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
