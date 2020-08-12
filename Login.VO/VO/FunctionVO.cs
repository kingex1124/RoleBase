using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.VO
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
        [Display(Name = "上層")]
        public string ParentName { get; set; }

        /// <summary>
        /// Title名稱
        /// </summary>
        [Display(Name = "名稱")]
        public string Title { get; set; }
    }
}
