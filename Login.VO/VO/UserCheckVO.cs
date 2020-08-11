using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.VO
{
    public class UserCheckVO
    {
        /// <summary>
        /// 選擇
        /// </summary>
        public bool Check { get; set; }
        /// <summary>
        /// 使用者ID
        /// </summary>
        [Display(Name = "帳號ID")]
        public int UserID { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        public string AccountName { get; set; }
        /// <summary>
        /// 暱稱
        /// </summary>
        [Display(Name = "暱稱")]
        public string UserName { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
    }
}
