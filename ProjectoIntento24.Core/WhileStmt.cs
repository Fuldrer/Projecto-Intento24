using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoIntento24.Core
{
    public class WhileStmt : Statement
    {
        public Expression expr { get; set; }

        public Statement stmt { get; set; }

        public WhileStmt(Expression expr, Statement stmt)
        {
            this.expr = expr;
            this.stmt = stmt;
        }
    }
}
