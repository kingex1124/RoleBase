using Login.DTO;
using Login.DTO.EFModel;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public class FunctionEfRepository : RoleBaseEfContext<Function>, IFunctionEfRepository
    {
        #region 屬性

        private readonly RoleBaseEntities _db;

        #endregion

        #region 建構子

        public FunctionEfRepository(RoleBaseEntities db) : base(db)
        {
            _db = db;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得Function資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionDTO> GetFunctionData(PageDataVO pageDataVO)
        {
            var isMenu = Convert.ToBoolean(pageDataVO.WhereCondition[2].Value);
            var result = (from function in _db.Function
                          where function.Url == pageDataVO.WhereCondition[0].Value &&
                          function.Title == pageDataVO.WhereCondition[1].Value &&
                          function.IsMenu == isMenu
                          orderby function.FunctionID
                          select new FunctionDTO()
                          {
                              FunctionID = function.FunctionID,
                              Url = function.Url,
                              Description = function.Description
                          });

            return result.Skip(pageDataVO.LowerBound).Take(pageDataVO.PageSize == null ? 0 : Convert.ToInt32(pageDataVO.PageSize));
        }

        /// <summary>
        /// 取得資料總筆數
        /// </summary>
        /// <param name="pageDataVO"></param>
        /// <returns></returns>
        public int GetFunctionCount(PageDataVO pageDataVO)
        {
            var isMenu = Convert.ToBoolean(pageDataVO.WhereCondition[2].Value);
            var result = (from function in _db.Function
                          where function.Url == pageDataVO.WhereCondition[0].Value &&
                          function.Title == pageDataVO.WhereCondition[1].Value &&
                          function.IsMenu == isMenu
                          orderby function.FunctionID
                          select new FunctionDTO()
                          {
                              FunctionID = function.FunctionID,
                              Url = function.Url,
                              Description = function.Description
                          });

            return result.Count();
        }

        /// <summary>
        /// 取得作為上層的keyValue資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePairDTO> GetParentKeyValue()
        {
            var result = (from function in _db.Function
                          where function.IsMenu == true
                          orderby function.FunctionID
                          select new KeyValuePairDTO()
                          {
                              Key = function.FunctionID,
                              Value = function.Title
                          });

            return result;
        }

        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public int AddFunction(FunctionVO functionVO)
        {
            Insert(new Function()
            {
                Url = functionVO.Url,
                Description = functionVO.Description,
                IsMenu = functionVO.IsMenu,
                Parent = functionVO.Parent,
                Title = functionVO.Title
            });

            return SaveChanges();
        }

        /// <summary>
        /// 刪除功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteFunction(string id)
        {
            DeleteByKey(new object[] { Convert.ToInt32(id) });

            return SaveChanges();
        }

        /// <summary>
        /// 編輯功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public int EditFunction(FunctionVO functionVO)
        {
            var functionData = _db.Function.Where(o => o.FunctionID == functionVO.FunctionID).FirstOrDefault();
            functionData.FunctionID = functionVO.FunctionID;
            functionData.Url = functionVO.Url;
            functionData.Description = functionData.Description;

            Update(functionData);

            return SaveChanges();
        }

        /// <summary>
        /// 取得Menu資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionMenuDTO> GetMenuData(string userID)
        {
            int userIDNum = string.IsNullOrWhiteSpace(userID) ? -1 : Convert.ToInt32(userID);
            var result = (from roleUser in _db.RoleUser
                          join roleFunction in _db.RoleFunction
                          on roleUser.RoleID equals roleFunction.RoleID
                          join function in _db.Function
                          on roleFunction.FunctionID equals function.FunctionID
                          where function.IsMenu == true && roleUser.UserID == userIDNum
                          select new FunctionMenuDTO()
                          {
                               FunctionID = function.FunctionID,
                               Url = function.Url,
                               Parent = function.Parent.Value,
                               Title = function.Title,
                          });

            return result;
        }

        #endregion
    }
}
