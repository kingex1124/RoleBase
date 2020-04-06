using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginVO.VO
{
    public class FunctionVO
    {
        /// <summary>
        /// 功能ID
        /// </summary>
        public int FunctionID { get; set; }
        /// <summary>
        /// 功能
        /// </summary>
        [Required]
        public string Url { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "敘述")]
        public string Description { get; set; }
        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; }
    }
}
