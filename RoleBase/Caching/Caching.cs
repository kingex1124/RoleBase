using Login.Service;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoleBase
{
    /// <summary>
    /// 全域暫存變數
    /// </summary>
    public static class Caching
    {
        /// <summary>
        /// Service物件
        /// </summary>
        private static IRoleService _roleService = new RoleService();
        /// <summary>
        /// 用來暫存取回來的資料包
        /// </summary>
        public static List<UserCheckVO> UserCheckVOList = new List<UserCheckVO>();

        /// <summary>
        /// 取Table的資料包回來
        /// 可以放在Application_Start、資料有新增、修改、刪除後的時候執行
        /// </summary>
        public static void GetTableDataToCaching()
        {
            PageDataVO pageDataVO = new PageDataVO() { OrderByColumn = "UserID", OrderByType = "ASC" };
            UserCheckVOList = _roleService.GetUserCheckByRole("1", pageDataVO).ToList();
        }
    }
}