using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Repository.Interface
{
    public interface IRoleRepository
    {
        IEnumerable<RoleDTO> GetRoleDataByAccountName(string userID);

        IEnumerable<RoleDTO> GetRoleData();

        int AddRole(RoleVO roleVO);

        int DeleteRole(string id, ref SqlConnection conn, ref SqlTransaction tran);

        int EditRole(RoleVO roleVO);

    }
}
