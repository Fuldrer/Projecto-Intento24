using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoIntento24.Core
{
    public class AssignationStmt : Statement
    {
        public IdExpr Id { get; set; }

        public Expression Expr { get; set; }

        public AssignationStmt(IdExpr id, Expression Expr)
        {
            this.Expr = Expr;
            this.Id = id;
        }
    }
}
