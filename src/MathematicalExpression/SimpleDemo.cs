//
// CODE MADE AVAILABLE UNDER GPL LICENSE
// COPYRIGHT 2019-2010
// CloudAda
//
//  Date    : 
//  Comment : 
//  Author  : 
//
//  Date    : Sep 2019
//  Comment : Translated into c# from Thesis
//  Author  : Xuezhe Li
//
//  Date    : Sep 2019
//  Comment : Basic UI (Winforms) to demonstrate fucntionality
//  Author  : Xuezhe Li

//  Date    : July 24th, 2010
//  Comment : Reformat Code
//  Author  : Kevin Sheth
//
//  Date    : July 24th, 2010
//  Comment : Form name changed to SimpleDemo
//  Author  : Kevin Sheth
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MathematicalExpression.Properties;


//
// TODO: UPDATE GRAMMAR FOR NEW OPERATOR PRECENDENCE PARSER
// 
// expression --> term moreterms
// moreterms --> + term {print '+'} moreterms
// moreterms --> - term {print '-'} moreterms
//
// term --> factor morefactors
//
// morefactors --> * factor { print '*' }
// morefactors --> / factor { print '/' }
//




namespace MathematicalExpression
{
    public partial class SimpleDemo : Form
    {
        public const int NONE = 0;
        public const int LargestSize = 54;

        private static readonly Dictionary<string, int> id_dict = new Dictionary<string, int>();
        private static int id_count;
        public int Given;

        private static IEvaluator mycalc = LinkedListEvaluator.Instance;
        public List<Queue<string>> LookAhead = new List<Queue<string>>();
        public IEnumerable<string> PostFixQueue = mycalc.GetQueue();
        public List<string> PostFixString = new List<string>();


        public SimpleDemo()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e) { }


        private int GetID(string IDName)
        {
            if (id_dict.ContainsKey(IDName))
            {
                return id_dict[IDName];
            }

            id_dict.Add(IDName, id_count++);
            return id_dict[IDName];
        }


        private static void MatchAndIncrement(Queue<Token> queue, string checkValue)
        {
            if (queue.Peek().ToString() == checkValue)
            {
                queue.Dequeue();
            }
            else
            {
                throw new Exception(Resources.SimpleDemo_SyntaxError_Syntax_Error);
            }
        }

        private static List<string> ParseAll(string txtExpressionText)
        {
            var ret = new List<string>();
            var lex = new LexicalAnalyzer(txtExpressionText);
            var LookAhead = lex.TokensList;

            foreach (Queue<Token> queue in LookAhead)
            {
                ret.AddRange(ParseExpression(queue, Constants.MAXPRES));
                ret.Add(",");
            }

            return ret;
        }

        private static List<string> ParseExpression(Queue<Token> queue, int precedence)
        {
            var ret = ParseFactor(queue);
            while (true)
            {
                if (queue.Peek().IsOperator && queue.Peek().OpDet.Precedence == precedence)
                {
                    var Temp = queue.Peek().OpDet.OperatorString;

                    if (queue.Peek().OpDet.OpArgs == OperatorArguments.BINARY)
                    {

                    }
                    

                    MatchAndIncrement(queue, Temp);

                    ret.AddRange(ParseFactor(queue));
                    ret.Add(Temp);
                }
                else if (queue.Peek().IsOperator && queue.Peek().OpDet.Precedence <= precedence)
                {
                    return ParseExpression(queue, precedence - 1);
                }
                else
                {
                    break;
                }
            }

            return ret;
        }

        private static List<string> ParseFactor(Queue<Token> queue)
        {
            var ret = new List<string>();
            if (queue.Peek().type == TokenType.BEGIN_INCREASE_PRESCENDENCE)
            {
                MatchAndIncrement(queue, "(");

                ret.AddRange(ParseExpression(queue, Constants.MAXPRES));
                MatchAndIncrement(queue, ")");
            }
            else if (queue.Peek().type == TokenType.NUMBER || queue.Peek().type == TokenType.VARIABLE)
            {
                //var LexicalToken = queue.Peek().Split("_")[0];
                //var LexicalAttribute = queue.Peek().Split("_")[1];
                var temp = queue.Peek().ToString();
                MatchAndIncrement(queue, temp);
                ret.Add(temp);
            }
            else if (queue.Peek().type == TokenType.FUNCTION)
            {
                var func = queue.Peek().FuncDet.FuncString;
                MatchAndIncrement(queue, func);
                MatchAndIncrement(queue, "(");
                ret.AddRange(ParseListOfExpressions(queue));
                MatchAndIncrement(queue, ")");
                ret.Add(func);
            }
            else
            {
                throw new Exception(Resources.SimpleDemo_SyntaxError_Syntax_Error);
            }

            return ret;
        }

        private static List<string> ParseListOfExpressions(Queue<Token> queue)
        {
            var ret = ParseExpression(queue, Constants.MAXPRES);
            while (true)
            {
                if (queue.Peek().ToString() == ",")
                {
                    MatchAndIncrement(queue, queue.Peek().ToString());
                    ret.AddRange(ParseExpression(queue, Constants.MAXPRES));
                }
                else
                {
                    break;
                }
            }

            return ret;
        }

        //TODO: Integrate after catching error
        private void MessageBox_SyntaxError()
        {
            if (Given == 0)
            {
                MessageBox.Show(
                    Resources.SimpleDemo_SyntaxError_Syntax_Error, 
                    Resources.SimpleDemo_SyntaxError_Error);
                Given = -1;
            }

        }


        private static string Parse(string expression)
        {
            var postFix = ParseAll(expression);

            var bld = new StringBuilder();
            foreach (var item in postFix)
            {
                bld.Append(item);
                bld.Append(" ");
            }

            return bld.ToString();
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            Parse(txtExpression.Text);
            txtResult.Text = Parse(txtExpression.Text);
            txtValue.Text = mycalc.Calculate(PostFixQueue).ToString();
        }


        private void txtResult_TextChanged(object sender, EventArgs e) { }

        private void txtExpression_TextChanged(object sender, EventArgs e) { }
    }

    internal class UnsupportedFunctionException : Exception { }
}