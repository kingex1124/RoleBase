using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.VO
{
    public class FunctionTableResultVO
    {
        public IEnumerable<FunctionVO> FunctionData { get; set; }
        public int TableMaxPage { get; set; }
    }
}
