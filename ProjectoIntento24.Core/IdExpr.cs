using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoIntento24.Core
{
    public class IdExpr
    {
        public string Name { get; set; }
        public ExpressionType Type { get; set; }
        public IdExpr(string name, ExpressionType type) 
        {
            Name = name;
            Type = type;
        }
    }
}
