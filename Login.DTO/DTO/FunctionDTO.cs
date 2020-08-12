using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DTO
{
    public class FunctionDTO
    {
        /// <summary>
        /// 功能ID
        /// </summary>
        public int FunctionID { get; set; }
        /// <summary>
        /// 功能
        /// </summary>

        public string Url { get; set; }
        /// <summary>
        /// 描述
        /// </summary>

        public string Description { get; set; }

        /// <summary>
        /// 是否為選單
        /// </summary>
        public bool IsMenu { get; set; }

        /// <summary>
        /// 上層選單對象
        /// </summary>
        public int Parent { get; set; }
        
        /// <summary>
        /// 上層名稱
        /// </summary>

        public string ParentName { get; set; }

        /// <summary>
        /// Title名稱
        /// </summary>
        public string Title { get; set; }
    }
}
