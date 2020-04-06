using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDTO.DTO
{
    public class UserCheckDTO
    {
        /// <summary>
        /// 選擇
        /// </summary>
        public bool Check { get; set; }
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
    }
}
