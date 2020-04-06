using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginVO.VO
{
    public class FunctionCheckVO
    {
        /// <summary>
        /// 選擇
        /// </summary>
        public bool Check { get; set; }
        /// <summary>
        /// 功能ID
        /// </summary>
        [Display(Name = "功能ID")]
        public int FunctionID { get; set; }
        /// <summary>
        /// 功能
        /// </summary>
        [Display(Name = "Url")]
        public string Url { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
    }
}
