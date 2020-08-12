using Login.DTO;
using Login.DTO.EFModel;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<FunctionDTO> GetFunctionData()
        {
            var result = (from function in _db.Function
                          orderby function.FunctionID
                          select new FunctionDTO()
                          {
                              FunctionID = function.FunctionID,
                              Url = function.Url,
                              Description = function.Description
                          });

            return result;
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

        #endregion
    }
}
