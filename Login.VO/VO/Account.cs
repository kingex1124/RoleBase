using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.VO
{
    public class Account
    {
        /// <summary>
        /// 帳號ID
        /// </summary>
        [Display(Name = "帳號ID")]
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
        [Display(Name = "用戶名")]
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name = "密碼")]
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 密碼確認
        /// </summary>
        [Display(Name = "密碼確認")]
        [Required]
        public string PasswordConfirm { get; set; }
        /// <summary>
        /// 信箱
        /// </summary>
        [Display(Name = "信箱")]
        public string Email { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        [Display(Name = "電話")]
        public string Phone { get; set; }
        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; }
    }
}
