﻿using Login.DTO;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public interface IRoleFunctionRepository
    {
        IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string userID);

        int DeleteRoleFunctionByFunctionID(string functionID, ref SqlConnection conn, ref SqlTransaction tran);

        IEnumerable<FunctionCheckDTO> GetFunctionCheckByRole(string id, PageDataVO pageDataVO);

        int DeleteRoleFunctionByRoleID(string roleID, ref SqlConnection conn, ref SqlTransaction tran);

        int InsertRoleFunction(RoleFunctionDTO roleFunctionDTO, ref SqlConnection conn, ref SqlTransaction tran);
    }
}
