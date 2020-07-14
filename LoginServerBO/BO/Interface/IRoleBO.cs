using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.BO.Interface
{
    public interface IRoleBO
    {
        IEnumerable<RoleDTO> GetRoleData();
        int AddRole(RoleVO roleVO);
        int DeleteRole(string id, ref SqlConnection conn, ref SqlTransaction tran);
        int EditRole(RoleVO roleVO);
        IEnumerable<UserCheckDTO> GetUserCheckByRole(string id);
        int DeleteRoleUserByRoleID(string roleID, ref SqlConnection conn, ref SqlTransaction tran);
        int InsertRoleUser(RoleUserDTO roleUserDTO, ref SqlConnection conn, ref SqlTransaction tran);
    }
}
