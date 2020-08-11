using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DTO
{
    public class RoleDTO
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 角色名稱
        /// </summary>

        public string RoleName { get; set; }
        /// <summary>
        /// 敘述
        /// </summary>

        public string Description { get; set; }
    }
}
