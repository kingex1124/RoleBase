using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DTO
{
    public class FunctionCheckDTO
    {
        /// <summary>
        /// 選擇
        /// </summary>
        public bool Check { get; set; }
        /// <summary>
        /// 功能ID
        /// </summary>
        public int FunctionID { get; set; }
        /// <summary>
        /// 功能
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 功能名稱
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否為Menu
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 上層對象
        /// </summary>
        public string ParentName { get; set; }

    }
}
