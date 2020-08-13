using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.VO
{
    public class FunctionMenuNode
    {
        public FunctionMenuNode(FunctionMenuVO fMVO)
        {
            Val = fMVO;
        }
        public FunctionMenuVO Val { get; set; }
        public List<FunctionMenuNode> Next { get; set; } 
    }
}
