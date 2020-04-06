using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginVO.VO
{
    public class AccountInfoData
    {
        public readonly static string LoginInfo;

        static AccountInfoData()
        {
            LoginInfo = "LoginInfo";
        }

        /// <summary>
        /// 使用者ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        [Required]
        public string AccountName { get; set; }
        /// <summary>
        /// 暱稱
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name = "密碼")]
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; }
    }
}
