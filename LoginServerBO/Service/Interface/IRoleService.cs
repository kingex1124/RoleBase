﻿using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Service.Interface
{
    public interface IRoleService
    {
        IEnumerable<RoleVO> GetRoleData();
        string AddRole(RoleVO roleVO);
        string DeleteRole(string id);
        string EditRole(RoleVO roleVO);
        IEnumerable<UserCheckVO> GetUserCheckByRole(string roleID);
        string SaveRoleUserSetting(IEnumerable<UserCheckVO> userCheckVO);
        string ClearRoleUserByRoleID(string roleID);

    }
}
