using ProjectoIntento24.Core;
using ProjectoIntento24.Lexer;
using System;
using System.Linq.Expressions;

namespace ProjectoIntento24.Parser
{
    public class Parser
    {
        private Token lookAhead;
        private readonly IScanner scanner;
        public Parser(IScanner scanner)
        {
            this.scanner = scanner;
            this.Move();
        }

        public void Parse()
        {
             Code();
            Console.WriteLine("Parsing Complete");
        }

        private void Code()
        {
             Block();

            Console.WriteLine("Out of Block");
        }

        private void Block()
        {
            Decls();
            Stmts();
        }

        private void Stmts()
        {
            if (this.lookAhead.TokenType == TokenType.EOF)
            {
                //return null;
            }
            else
            {
                if(this.lookAhead.TokenType == TokenType.RightKey)
                {
                    Console.WriteLine("Out of loop");
                    //return null;
                }   
                else
                {
                    if(this.lookAhead.TokenType == TokenType.ReturnKeyword)
                    {
                        Match(TokenType.ReturnKeyword);

                    }    
                    Stmt();
                    Stmts();
                    //return new SequenceStatement(Stmt(), Stmts()); ;
                }

            }
            
        }

        private void Stmt()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.ID:
                    DetAction();
                    break;
                case TokenType.WhileKeyword:
                    Match(TokenType.WhileKeyword);
                    //return WhileStatement();
                    break;
                case TokenType.IfKeyword:
                    Match(TokenType.IfKeyword);
                    IfStmt();
                    break;
                case TokenType.ForKeyword:
                    Match(TokenType.ForKeyword);
                    ForStmt();
                    break;
                case TokenType.ConsoleKeyword:
                    PrintStmt();
                    break;
                case TokenType.RightKey:
                    break;
                case TokenType.TrueKeyword:
                    Match(TokenType.TrueKeyword);
                    Match(TokenType.Semicolon);
                    break;
                case TokenType.FalseKeyword:
                    Match(TokenType.FalseKeyword);
                    Match(TokenType.Semicolon);
                    break;
                default:
                    Block();
                    break;
            }
        }

        private void PrintStmt()
        {
            Match(TokenType.ConsoleKeyword);
            Match(TokenType.Punto);
            Match(TokenType.LogKeyword);
            Match(TokenType.LeftParens);
            //var expr = PrintParams();
            PrintParams();
            Match(TokenType.RightParens);
            Match(TokenType.Semicolon);
            //return new PrintStatement(expr);

        }

        private void PrintParams()
        {
            if (this.lookAhead.TokenType == TokenType.ID)
            {
                Match(TokenType.ID);
                if (this.lookAhead.TokenType == TokenType.Coma)
                {
                    Match(TokenType.Coma);
                    PrintParams();
                }
                else if (this.lookAhead.TokenType == TokenType.LeftParens)
                {
                    FunctionExpr();
                }
                else if(this.lookAhead.TokenType == TokenType.Plus || this.lookAhead.TokenType == TokenType.Minus || this.lookAhead.TokenType == TokenType.Mult
                    || this.lookAhead.TokenType == TokenType.DIV || this.lookAhead.TokenType == TokenType.Elevate ||
                    this.lookAhead.TokenType == TokenType.LessThan || this.lookAhead.TokenType == TokenType.GreaterThan ||
                    this.lookAhead.TokenType == TokenType.LessOrEqual || this.lookAhead.TokenType == TokenType.GreaterorEqual)
                {
                    ArithmeticExprVar();

                }
            }
            else if(this.lookAhead.TokenType == TokenType.IntConstant)
            {
                Match(TokenType.IntConstant);
                if (this.lookAhead.TokenType == TokenType.Coma)
                {
                    Match(TokenType.Coma);
                    PrintParams();
                }
            }
        }
        private void FunctionExpr()
        {
            Match(TokenType.LeftParens);
            PrintParams();
            Match(TokenType.RightParens);
        }

        private void VarParams()
        {

            
            
        }

        private void ForStmt()
        {
            //For in y for of causan loop infinito
            Match(TokenType.LeftParens);
            if(this.lookAhead.TokenType == TokenType.LetKeyword || this.lookAhead.TokenType == TokenType.ConstKeyword ||
                this.lookAhead.TokenType == TokenType.VarKeyword)
            {
                switch (this.lookAhead.TokenType)
                {
                    case TokenType.LetKeyword:
                        Match(TokenType.LetKeyword);
                        IdExpr id = new IdExpr(this.lookAhead.Lexeme, type: null);
                        Match(TokenType.ID);
                        AssignationStatement();
                        Match(TokenType.ID);
                        LogicOP();
                        if(this.lookAhead.TokenType == TokenType.IntConstant)
                        {
                            Match(TokenType.IntConstant);
                        }
                        else
                        {
                            Match(TokenType.ID);
                        }
                        Match(TokenType.Semicolon);
                        IncrDecrStmt();
                        Match(TokenType.RightParens);
                        Match(TokenType.LeftKey);
                        Stmts();
                        Match(TokenType.RightKey);
                        break;
                    case TokenType.VarKeyword:
                        break;
                    case TokenType.ConstKeyword:
                        break;
                }
            }
            else if(this.lookAhead.TokenType == TokenType.ID)
            {
                Match(TokenType.ID);
                switch (this.lookAhead.TokenType)
                {
                    case TokenType.InKeyword:
                        Match(TokenType.InKeyword);
                        Match(TokenType.ID);
                        Match(TokenType.RightParens);
                        break;
                    case TokenType.OfKeyword:
                        Match(TokenType.OfKeyword);
                        Match(TokenType.ID);
                        Match(TokenType.RightParens);
                        break;
                }
            }
        }

        private void IncrDecrStmt()
        {
            if(this.lookAhead.TokenType == TokenType.INCR)
            {
                Match(TokenType.INCR);
                Match(TokenType.ID);
            }
            else if(this.lookAhead.TokenType == TokenType.ID)
            {
                Match(TokenType.ID);
                if(this.lookAhead.TokenType == TokenType.INCR)
                {
                    Match(TokenType.INCR);
                    
                }
                else
                {
                    Match(TokenType.DECR);

                }
            }
            else if(this.lookAhead.TokenType == TokenType.DECR)
            {
                Match(TokenType.DECR);
                Match(TokenType.ID);
            }
        }

        private void IfStmt()
        {
            this.Match(TokenType.LeftParens);
            ConditionExpr();
            this.Match(TokenType.RightParens);
            Match(TokenType.LeftKey);
            Stmts();
            Match(TokenType.RightKey);
            if (this.lookAhead.TokenType == TokenType.ElseKeyword)
            {
                this.Match(TokenType.ElseKeyword);
                Match(TokenType.LeftKey);
                Stmts();
                Match(TokenType.RightKey);
            }

        }

        private void DetAction()
        {
            IdExpr id;
            id = new IdExpr(this.lookAhead.Lexeme, type: null);
            Match(TokenType.ID);
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Plus:
                    ArithmeticExpr();
                    break;
                case TokenType.Minus: 
                    ArithmeticExpr();
                    break;
                case TokenType.Mult:
                    ArithmeticExpr();
                    break;
                case TokenType.DIV:
                    ArithmeticExpr();
                    break;
                case TokenType.Equal:
                    AssignationStatement();
                    break;
            }
        }



        private void ConditionExpr()
        {
            Match(TokenType.ID);
            LogicOP();
            if(this.lookAhead.TokenType == TokenType.ID)
            {
                Match(TokenType.ID);
            }
            Match(TokenType.IntConstant);
        }

        private void LogicOP()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.EqualEqual:
                    Match(TokenType.EqualEqual);
                    break;
                case TokenType.NotEqual:
                    Match(TokenType.NotEqual);
                    break;
                case TokenType.LessThan: 
                    Match(TokenType.LessThan);
                    break;
                case TokenType.GreaterThan:
                    Match(TokenType.GreaterThan);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.GreaterorEqual);
                    }
                    break;
                case TokenType.GreaterorEqual:
                    Match(TokenType.GreaterorEqual);
                    break;
            }
        }

        private void WhileStatement()
        {
            Match(TokenType.LeftParens);
            //var expr = ConditionExpr();
            ConditionExpr();
            Match(TokenType.RightParens);
            Match(TokenType.LeftKey);
            Stmts();
            Match(TokenType.RightKey);
            Console.WriteLine("Sale del while");
            //return new WhileStmt(expr, stmts);
        }

        private void AssignationStatement()
        {
            Match(TokenType.Equal);
            switch(this.lookAhead.TokenType)
            {
                case TokenType.IntConstant:
                    Match(TokenType.IntConstant);
                    ArithmeticExpr();
                    Match(TokenType.Semicolon);
                    //return new AssignationStmt(id, expr);
                    break;
                case TokenType.ID:
                    Match(TokenType.ID);
                    ArithmeticExpr();
                    break;
                //Match(TokenType.Semicolon);
                //return new AssignationStmt(id, expr);
                case TokenType.LeftBracket:
                    ArrayStmt();
                    break;
            }
            
            Console.WriteLine("Asignado Correctamente");
            //return null;
        }

        private void ArrayStmt()
        {
            Match(TokenType.LeftBracket);
            PrintParams();
            Match(TokenType.RightBracket);
        }

        private void ArithmeticExprVar()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Plus:
                    Match(TokenType.Plus);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        if(this.lookAhead.TokenType == TokenType.IntConstant)
                        {
                            Match(TokenType.IntConstant);
                        }
                        else
                        {
                            Match(TokenType.ID);
                        }
                    }
                    else
                    {
                        Match(TokenType.ID);
                    }
                    break;
                case TokenType.Minus:
                    Match(TokenType.Minus);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        Match(TokenType.ID);
                    }
                    else
                    {
                        Match(TokenType.ID);
                    }
                    break;
                case TokenType.Mult:
                    Match(TokenType.Mult);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        Match(TokenType.ID);
                    }
                    else
                    {
                        Match(TokenType.ID);
                    }
                    break;
                case TokenType.DIV:
                    Match(TokenType.DIV);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.DivEqual);
                        Match(TokenType.ID);
                    }
                    else
                    {
                        Match(TokenType.ID);
                    }
                    break;
                case TokenType.MOD:
                    Match(TokenType.MOD);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        Match(TokenType.ID);
                    }
                    else
                    {
                        Match(TokenType.ID);
                    }
                    break;
                case TokenType.Elevate:
                    Match(TokenType.Elevate);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        Match(TokenType.ID);
                    }
                    else
                    {
                        Match(TokenType.ID);
                    }
                    break;
                case TokenType.GreaterThan:
                    Match(TokenType.GreaterThan);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        Match(TokenType.ID);
                    }
                    else
                    {
                        Match(TokenType.ID);
                    }
                    break;
                case TokenType.LessThan: 
                    Match(TokenType.LessThan);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        Match(TokenType.ID);
                    }
                    else
                    {
                        Match(TokenType.ID);
                    }
                    break;
                case TokenType.LessOrEqual: 
                    Match(TokenType.LessOrEqual);
                    Match(TokenType.ID);
                    break;
                case TokenType.GreaterorEqual: 
                    Match(TokenType.GreaterorEqual);
                    Match(TokenType.ID);
                    break;
                default: break;
            }
        }

        private void ArithmeticExpr()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Plus:
                    Match(TokenType.Plus);
                    if(this.lookAhead.TokenType == TokenType.Equal)
                    {
                        
                        Match(TokenType.Equal);
                        if(this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }
                    }
                    else
                    {
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }
                    }
                    break;
                case TokenType.Minus:
                    Match(TokenType.Minus);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {

                        Match(TokenType.Equal);
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }
                    }
                    else
                    {
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }
                    }
                    break;
                case TokenType.Mult:
                    Match(TokenType.Mult);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {

                        Match(TokenType.Equal);
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }

                    }
                    else
                    {
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }
                    }
                    break;
                case TokenType.MOD:
                    Match(TokenType.MOD);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {

                        Match(TokenType.Equal);
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }

                    }
                    else
                    {
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }
                    }
                    break;
                case TokenType.Elevate:
                    Match(TokenType.Elevate);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {

                        Match(TokenType.Equal);
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }

                    }
                    else
                    {
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }
                    }
                    break;
                case TokenType.DIV:
                    Match(TokenType.DIV);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {

                        Match(TokenType.Equal);
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }

                    }
                    else
                    {
                        if (this.lookAhead.TokenType == TokenType.ID)
                        {
                            Match(TokenType.ID);
                            Match(TokenType.Semicolon);
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                            Match(TokenType.Semicolon);
                        }
                    }
                    break;
                default: break;
            }
        }

        private void Decls()
        {
            if (this.lookAhead.TokenType == TokenType.LetKeyword || this.lookAhead.TokenType == TokenType.ConstKeyword || 
                this.lookAhead.TokenType == TokenType.VarKeyword || this.lookAhead.TokenType == TokenType.FunctionKeyword || this.lookAhead.TokenType == TokenType.ConsoleKeyword)
            {
                Decl();
                Decls();
            }
        }

        private void Decl()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LetKeyword:
                    Match(TokenType.LetKeyword);
                    Match(TokenType.ID);
                    Match(TokenType.DosPuntos);
                    Type();
                    Match(TokenType.Semicolon);
                    break;
                case TokenType.ConstKeyword:
                    Match(TokenType.ConstKeyword);
                    Match(TokenType.ID);
                    Match(TokenType.DosPuntos);
                    Type();
                    Match(TokenType.Semicolon);
                    break;
                case TokenType.VarKeyword:
                    Match(TokenType.VarKeyword);
                    Match(TokenType.ID);
                    Match(TokenType.DosPuntos);
                    Type();
                    Match(TokenType.Semicolon);
                    break;
                case TokenType.FunctionKeyword:
                    Match(TokenType.FunctionKeyword);
                    FunctionDeclStmt();
                    break;
                case TokenType.ConsoleKeyword:
                    PrintStmt();
                    break;
                default:
                    break;
            }
        }

        private void FunctionDeclStmt()
        {
            Match(TokenType.ID);
            Match(TokenType.LeftParens);
            ParamsFunc();
            Match(TokenType.RightParens);
            Match(TokenType.DosPuntos);
            Type();
            if(this.lookAhead.TokenType == TokenType.LeftKey)
            {
                Match(TokenType.LeftKey);
                Stmts();
                Match(TokenType.RightKey);
            }
            else
            {
                Match(TokenType.Semicolon);
            }

        }

        private void ParamsFunc()
        {
            Match(TokenType.ID);
            Match(TokenType.DosPuntos);
            Type();
            if(this.lookAhead.TokenType == TokenType.Coma)
            {
                Match(TokenType.Coma);
                ParamsFunc();
            }
        }

        private void Type()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.NumberKeyword:
                    Match(TokenType.NumberKeyword);
                    if(this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        if(this.lookAhead.TokenType == TokenType.LeftBracket)
                        {
                            ArrayStmt();
                        }
                        else
                        {
                            Match(TokenType.IntConstant);
                        }
                    }
                    break;
                case TokenType.StringKeyword:
                    Match(TokenType.StringKeyword);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        Match(TokenType.ID);
                    }
                    break;
                default:
                    Match(TokenType.BoolKeyword);
                    if (this.lookAhead.TokenType == TokenType.Equal)
                    {
                        Match(TokenType.Equal);
                        if(this.lookAhead.TokenType == TokenType.FalseKeyword) 
                        {
                            Match(TokenType.FalseKeyword);
                        }
                        else
                        {
                            Match(TokenType.TrueKeyword);
                        }
                    }
                    break;
            }
        }

        private void Move()
        {
            this.lookAhead = this.scanner.GetNextToken();
        }

        private void Match(TokenType tokenType)
        {
            if (this.lookAhead.TokenType != tokenType)
            {
                throw new ApplicationException($"Syntax error! expected {tokenType} found {this.lookAhead.TokenType}");
            }
            this.Move();
        }

    }


}
