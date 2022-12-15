using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoIntento24.Core
{
    public class PrintStatement : Statement
    {
        public List<Expression> Params { get; set; }
        public PrintStatement(List<Expression> Params) 
        {
            this.Params = Params;
        }

        public void GeneratedCode(List<Expression>  Params)
        {
            Console.WriteLine($"JSON.stringify({Params});");
        }
    }
}
