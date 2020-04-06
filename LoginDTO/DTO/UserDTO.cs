using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDTO.DTO
{
    public class UserDTO
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 暱稱
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }

    }
}
