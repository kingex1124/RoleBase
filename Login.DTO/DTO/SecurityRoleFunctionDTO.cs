using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DTO
{
    public class SecurityRoleFunctionDTO
    {
        /// <summary>
        /// 角色名稱
        /// </summary>
        public string RoleName { get; set; }
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
