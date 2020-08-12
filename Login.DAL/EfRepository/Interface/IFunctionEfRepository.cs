﻿using Login.DTO;
using Login.DTO.EFModel;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public interface IFunctionEfRepository : IRepository<Function>
    {
        IEnumerable<FunctionDTO> GetFunctionData();

        IEnumerable<KeyValuePairDTO> GetParentKeyValue();

        int AddFunction(FunctionVO functionVO);

        int DeleteFunction(string id);

        int EditFunction(FunctionVO functionVO);
    }
}
