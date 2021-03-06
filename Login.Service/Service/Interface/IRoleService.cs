﻿using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Service
{
    public interface IRoleService
    {
        IEnumerable<RoleVO> GetRoleData(PageDataVO pageDataVO);
        string AddRole(RoleVO roleVO);
        string DeleteRole(string id);
        string EditRole(RoleVO roleVO);
        IEnumerable<UserCheckVO> GetUserCheckByRole(string roleID, PageDataVO pageDataVO);
        string SaveRoleUserSetting(IEnumerable<UserCheckVO> userCheckVO);
        string ClearRoleUserByRoleID(string roleID);
    }
}
