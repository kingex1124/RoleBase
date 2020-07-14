﻿using LoginDTO.DTO;
using LoginServerBO.BO;
using LoginServerBO.BO.Interface;
using LoginServerBO.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Service
{
    public class SecurityService : ISecurityService
    {
        #region 屬性

        ISecurityBO _securityBO;

        #endregion

        #region 建構子

        public SecurityService()
        {
            _securityBO = new SecurityBO();
        }

        #endregion

        #region 方法

        public IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId)
        {
            return _securityBO.GetSecurityRoleFunction(roleId);
        }

        #endregion
    }
}
