using LoginDTO.DTO;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDAL.Repository.Interface
{
    public interface IFunctionRepository
    {
        IEnumerable<FunctionDTO> GetFunctionData();

        int AddFunction(FunctionVO functionVO);

        int DeleteFunction(string id, ref SqlConnection conn, ref SqlTransaction tran);

        int EditFunction(FunctionVO functionVO);
    }
}
