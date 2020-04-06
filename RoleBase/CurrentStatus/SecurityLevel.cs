using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoleBase.CurrentStatus
{
    public class SecurityLevel
    {
        public SecurityLevel()
        {
        }

        public List<RoleDTO> SecurityRole = null;

        public List<SecurityRoleFunctionDTO> SecurityUrl = new List<SecurityRoleFunctionDTO>();

        public AccountInfoData UserData;
    }
}