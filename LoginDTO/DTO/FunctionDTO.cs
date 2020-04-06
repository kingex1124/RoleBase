using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDTO.DTO
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
    }
}
