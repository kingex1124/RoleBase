using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoleBase.CurrentStatus
{
    public class SessionConnectionPool
    {
        public SessionConnectionPool()
        {
        }

        public static SecurityLevel CurrentUserInfo
        {
            get
            {
                return (SecurityLevel)HttpContext.Current.Session[AccountInfoData.LoginInfo];
            }
        }

        public static void SetCurrentUserInfo(SecurityLevel currentUserData)
        {
            HttpContext.Current.Session[AccountInfoData.LoginInfo] = currentUserData;
        }
    }
}