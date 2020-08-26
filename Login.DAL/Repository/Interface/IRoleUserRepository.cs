using Login.DTO;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public interface IRoleUserRepository
    {
        IEnumerable<UserCheckDTO> GetUserCheckByRole(string id, PageDataVO pageDataVO);

        int DeleteRoleUserByRoleID(string roleID, ref SqlConnection conn, ref SqlTransaction tran);

        int InsertRoleUser(RoleUserDTO roleUserDTO, ref SqlConnection conn, ref SqlTransaction tran);
    }
}
