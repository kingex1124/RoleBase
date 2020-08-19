using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.VO
{
    public class RoleTableResultVO
    {
        public IEnumerable<RoleVO> RoleData { get; set; }
        public int TableMaxPage { get; set; }
    }
}
