using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginVO.VO
{
    public class RoleVO
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 角色名稱
        /// </summary>
        [Display(Name = "角色名")]
        [Required]
        public string RoleName { get; set; }
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
