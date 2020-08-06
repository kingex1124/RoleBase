using LoginDTO.DTO;
using LoginDTO.EFModel;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.EfRepository.Interface
{
    public interface IFunctionEfRepository : IRepository<Function>
    {
        IEnumerable<FunctionDTO> GetFunctionData();

        int AddFunction(FunctionVO functionVO);

        int DeleteFunction(string id);

        int EditFunction(FunctionVO functionVO);
    }
}
