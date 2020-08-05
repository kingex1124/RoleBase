﻿using LoginDTO.DTO;
using LoginDTO.EFModel;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServerBO.Repository.Interface
{
    public interface IUserEfRepository : IRepository<User>
    {
        IEnumerable<UserDTO> FindAccountName(string accountName);

        UserDTO FindAccountData(string accountName);

        int UserInsert(Account account);
    }
}
