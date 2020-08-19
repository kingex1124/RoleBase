using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.VO
{
    public class PageDataVO
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public PageDataVO()
        {
        }

        /// <summary>
        /// 總頁碼數
        /// </summary>
        public int AllPageNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 總資料筆數
        /// </summary>
        public int DataCount
        {
            get;
            set;
        }

        /// <summary>
        /// Table的主Key
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// 排序的欄位
        /// </summary>
        public string OrderByColumn
        {
            get;
            set;
        }

        /// <summary>
        /// 排序的型態DESC or ASC
        /// 可Null or "" (為預設ASC)
        /// </summary>
        public string OrderByType
        {
            get;
            set;
        }

        /// <summary>
        /// 正在第幾頁
        /// </summary>
        public int PageNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int? PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 選取的欄位
        /// 逗號分隔
        /// </summary>
        public string SelectColumn
        {
            get;
            set;
        }

        /// <summary>
        /// 查詢條件
        /// </summary>
        public List<KeyValueVO> WhereCondition
        {
            get;
            set;
        }

        /// <summary>
        /// 所取下限
        /// </summary>
        public int LowerBound { get; set; }
        /// <summary>
        /// 所取上限
        /// </summary>
        public int UpperBound { get; set; }
    }
}
