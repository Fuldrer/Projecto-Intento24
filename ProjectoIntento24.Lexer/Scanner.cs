using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectoIntento24.Lexer
{
    public class Scanner : IScanner
    {
        private Input input;
        private readonly Dictionary<string, TokenType> keywords;
        public Scanner(Input input)
        {
            this.input = input;
            this.keywords = new Dictionary<string, TokenType>
            {
                {"break", TokenType.BreakKeyword},
                {"const", TokenType.ConstKeyword},
                {"do", TokenType.DoKeyword},
                {"else", TokenType.ElseKeyword},
                {"false", TokenType.FalseKeyword},
                {"for", TokenType.ForKeyword},
                { "if", TokenType.IfKeyword },
                { "in", TokenType.InKeyword },
                { "return", TokenType.ReturnKeyword },
                { "true", TokenType.TrueKeyword },
                { "var", TokenType.VarKeyword },
                { "void", TokenType.VoidKeyword },
                { "while", TokenType.WhileKeyword },
                { "let", TokenType.LetKeyword },
                { "of", TokenType.OfKeyword },
                { "continue", TokenType.ContinueKeyword},
                { "number", TokenType.NumberKeyword},
                { "bool", TokenType.BoolKeyword},
                { "string", TokenType.StringKeyword},
                { "function", TokenType.FunctionKeyword },
                { "console", TokenType.ConsoleKeyword },
                { "log", TokenType.LogKeyword }
            };
        }
        public Token GetNextToken()
        {
            var lexeme = new StringBuilder();
            var currentchar = GetNextChar();
            while (true)
            {
                while (char.IsWhiteSpace(currentchar))
                {
                    currentchar = GetNextChar();
                }

                if (char.IsLetter(currentchar))
                {
                    lexeme.Append(currentchar);
                    currentchar = PeekNextChar();
                    while (char.IsLetterOrDigit(currentchar))
                    {
                        currentchar = GetNextChar();
                        lexeme.Append(currentchar);
                        currentchar = PeekNextChar();
                    }
                    if (this.keywords.ContainsKey(lexeme.ToString()))
                    {
                        return lexeme.ToToken(input, this.keywords[lexeme.ToString()]);
                    }
                    return new Token
                    {
                        TokenType = TokenType.ID,
                        Column = input.Position.Column,
                        Line = input.Position.Line,
                        Lexeme = lexeme.ToString()
                    };
                }
                else if (char.IsDigit(currentchar))
                {
                    lexeme.Append(currentchar);
                    currentchar = PeekNextChar();
                    while (char.IsDigit(currentchar))
                    {
                        currentchar = GetNextChar();
                        lexeme.Append(currentchar);
                        currentchar = PeekNextChar();
                    }

                    if (currentchar != '.')
                    {
                        return lexeme.ToToken(input, TokenType.IntConstant);
                    }

                    currentchar = GetNextChar();
                    lexeme.Append(currentchar);
                    currentchar = PeekNextChar();
                    while (char.IsDigit(currentchar))
                    {
                        currentchar = GetNextChar();
                        lexeme.Append(currentchar);
                        currentchar = PeekNextChar();
                    }
                    return lexeme.ToToken(input, TokenType.IntConstant);
                }
                switch (currentchar)
                {

                    case '+':
                        lexeme.Append(currentchar);
                        var nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.PlusEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        if (nextChar == '+')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.INCR,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.Plus,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '&':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.ANDEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        if (nextChar == '&')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.ANDLogical,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        break;
                    case '=':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.EqualEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.Equal,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '!':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.NotEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        if (nextChar == ':')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.TBD,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        break;
                    case '(':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.LeftParens,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case ')':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.RightParens,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '-':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.MinusEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        if (nextChar == '-')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.DECR,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.Minus,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '<':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.LessOrEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.LessThan,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '[':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.LeftBracket,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case ']':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.RightBracket,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '*':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.MultEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.Mult,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '^':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.ElevateEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.Elevate,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '>':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.GreaterorEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.GreaterThan,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '{':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.LeftKey,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '}':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.RightKey,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '/':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.DivEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.DIV,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '|':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '|')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.ORLogical,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.OREqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        break;
                    case ':':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.Asignacion,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {

                            TokenType = TokenType.DosPuntos,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case ',':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.Coma,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case ';':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.Semicolon,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '%':
                        lexeme.Append(currentchar);
                        nextChar = PeekNextChar();
                        if (nextChar == '=')
                        {
                            GetNextChar();
                            lexeme.Append(nextChar);
                            return new Token
                            {

                                TokenType = TokenType.MODEqual,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        }
                        return new Token
                        {
                            TokenType = TokenType.MOD,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '.':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.Punto,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    case '\0':
                        lexeme.Append(currentchar);
                        return new Token
                        {
                            TokenType = TokenType.EOF,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    default:
                        throw new ApplicationException($"Lexema {lexeme} invalido en la fila: {input.Position.Line}, columna: {input.Position.Column}");
                }
            }
        }

        private char GetNextChar()
        {
            var next = input.NextChar();
            input = next.Reminder;
            return next.Value;
        }

        private char PeekNextChar()
        {
            var next = input.NextChar();
            return next.Value;
        }
    }
}
