using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace MathematicalExpression
{
    public enum TokenType
    {
        UNSET,
        BEGIN_INCREASE_PRESCENDENCE,
        END_INCREASE_PRESCENDENCE,
        NUMBER,
        VARIABLE,
        FUNCTION,
        OPERATOR,
        STRING //not needed for math
    }

    public enum TokenValidity
    {
        UNSET, //not yet set by parser
        VALID,
        INVALID
    }


    public enum Operator
    {
        Add,
        Subtract,
        Multiply,
        DivideIntegral,
        DivideNonIntegral,
        Mod,
        Power,
        UNSET
    }

    internal static class Constants
    {
        public const int MAXPRES = 5;
    }

    public class OperatorDetails
    {
        public OperatorArguments OpArgs = OperatorArguments.UNSET;
        public string OperatorString = "";
        public Operator Op = Operator.UNSET;
        public int Precedence = 0;
        //public List<double> Arguments = new List<double>();
    }

    public class FuncDetails
    {
        public int NumberOfArguments = 0;
        public string FuncString = "";
        public List<double> arguments = new List<double>();
    }

    public enum OperatorArguments
    {
        UNSET,
        UNARY,
        BINARY,
        ANBIGUOUS
    }

    public class Token
    {
        public override string ToString()
        {
            if (type == TokenType.NUMBER)
            {
                return Value.ToString();
            }

            if (type == TokenType.OPERATOR)
            {
                return StringValue;
            }

            if (type == TokenType.FUNCTION)
            {
                return FuncDet.FuncString;
            }

            return "";
        }

        public TokenType type = TokenType.UNSET;
        public TokenValidity Validity = TokenValidity.UNSET;
        public double Value { get; set; } = 0.0;

        public string StringValue { get; set; }
        public string UnparseableValue { get; } = "";
        public Func<double, double> OneParamFunc { get; set; } = r => 0;
        public Func<double, double, double> TwoParamFunc { get; set; } = (x, y) => 0;

        public bool IsOperator = false;

        public OperatorDetails OpDet { get; private set; } = new OperatorDetails
        {
            OperatorString = null,
            Op = Operator.UNSET
        };

        public FuncDetails FuncDet { get; private set; } = new FuncDetails
        {
            NumberOfArguments = 0,
            FuncString = null
        };

        public int NumberOfParameters = 0; //only applies to functions
        public Token() { }
    }


    internal class LexicalAnalyzer
    {
        public List<Queue<Token>> TokensList { get; private set; }

        public LexicalAnalyzer(string input)
        {
            var list = new Queue<Token>();
            var program = new List<Queue<Token>>();

            var ExprString = input.ToUpper();
            var LexPos = 0;

            // OP PRECENDENCE
            // --------------
            // Note: This is not exactly the same as c#
            // I felt it may be useful to put the / and % operators at a higher level
            // 0 : - +
            // 1 : / *
            // 2 : \ %|MOD
            // 3 : ^

            while (LexPos < ExprString.Length)
            {
                LexPos++;

                switch (ExprString[LexPos - 1])
                {
                    case ' ':
                    case (char) 9:
                        break;
                    case '\0':
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '\\':
                        list.Enqueue(item: new Token
                        {
                            type = TokenType.OPERATOR,
                            Validity = TokenValidity.VALID,
                            OpDet =
                            {
                                OperatorString = char.ToString('\\'),
                                Op = Operator.DivideNonIntegral,
                                Precedence = 2
                            }
                        });
                        break;
                    case '(':
                        list.Enqueue(new Token
                        {
                            type = TokenType.BEGIN_INCREASE_PRESCENDENCE,
                            Validity = TokenValidity.VALID,
                        });
                        break;
                    case ')':
                        list.Enqueue(new Token
                        {
                            type = TokenType.END_INCREASE_PRESCENDENCE,
                            Validity = TokenValidity.VALID,
                        });
                        break;
                    case ';':
                    case (char) 13:
                    case (char) 10:
                        if (list.Count != 0)
                        {
                            program.Add(list);
                            list = new Queue<Token>();
                        }

                        break;
                    default:
                        var Temp = "";
                        if (Utils.IsAlpha(ExprString[LexPos - 1].ToString()))
                        {
                            while (Utils.IsAlphaNumeric(ExprString[LexPos - 1].ToString()))
                            {
                                Temp = Temp + ExprString[LexPos - 1];
                                LexPos++;
                                if (LexPos > ExprString.Length)
                                {
                                    break;
                                }
                            }

                            LexPos--;


                            if (Temp.ToUpper() == "MOD")
                            {
                                list.Append(new Token
                                {
                                    type = TokenType.UNSET,
                                    Validity = TokenValidity.UNSET,
                                    Value = 0,
                                    OneParamFunc = null,
                                    TwoParamFunc = null,
                                    NumberOfParameters = 0
                                });
                            }

                            try
                            {
                                var f = Utils.GetFunctionDelegate(Temp);
                                list.Append(new Token
                                {
                                    type = TokenType.UNSET,
                                    Validity = TokenValidity.UNSET,
                                    Value = 0,
                                    NumberOfParameters = 0, OneParamFunc = null,
                                    TwoParamFunc = null
                                });
                            }
                            catch
                            {
                                throw new Exception("Syntax Error: Unsupported Function Or Unrecognized Token Found!");
                            }

                            //assume its a variable
                            list.Append(new Token
                            {
                                type = TokenType.UNSET,
                                Validity = TokenValidity.UNSET,
                                Value = 0,
                                OneParamFunc = null,
                                TwoParamFunc = null,
                                NumberOfParameters = 0
                            });
                        }
                        else if (Utils.IsNumber(ExprString[LexPos - 1].ToString()))
                        {
                            while (Utils.IsNumber(ExprString[LexPos - 1].ToString()) ||
                                   ExprString[LexPos - 1].ToString() == ".")
                            {
                                Temp = Temp + ExprString[LexPos - 1];
                                LexPos++;
                                if (LexPos > ExprString.Length)
                                {
                                    break;
                                }
                            }


                            LexPos = LexPos - 1;
                            list.Enqueue(new Token
                            {
                                type = TokenType.NUMBER,
                                Validity = TokenValidity.VALID,
                                Value = Convert.ToDouble(Temp),
                            });
                        }

                        break;
                }
            }

            TokensList = program;
        }
    }
}