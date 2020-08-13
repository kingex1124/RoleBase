using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Login.DTO
{
    public class FunctionMenuDTO
    {
        /// <summary>
        /// ID
        /// </summary>
        public int FunctionID { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 上層
        /// </summary>
        public int Parent { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Title { get; set; }
    }
}
