using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicalExpression
{
    public class ListEvaluator : IEvaluator
    {
        private List<string> _queue = new List<string>();
        private ListEvaluator() { }
        public static IEvaluator Instance { get; } = new ListEvaluator();

        public double Calculate(IEnumerable<string> postFixExpression)
        {
            if (!(postFixExpression is List<string>))
            {
                throw new UnsupportedTypeException();
            }

            var l = postFixExpression as List<string>;
            for (var i = 0; i < l.Count; i++)
            {
                switch (l[i])
                {
                    case "+":
                        l[i] =
                            (Convert.ToDouble(l[i - 2]) +
                             Convert.ToDouble(l[i - 1])).ToString();
                        l.RemoveAt(i - 2);
                        l.RemoveAt(i - 2);
                        i -= 2;
                        break;
                    case "-":
                        l[i] =
                            (Convert.ToDouble(l[i - 2]) -
                             Convert.ToDouble(l[i - 1])).ToString();
                        l.RemoveAt(i - 2);
                        l.RemoveAt(i - 2);
                        i -= 2;
                        break;
                    case "/":
                        l[i] =
                            (Convert.ToDouble(l[i - 2]) /
                             Convert.ToDouble(l[i - 1])).ToString();
                        l.RemoveAt(i - 2);
                        l.RemoveAt(i - 2);
                        i -= 2;
                        break;
                    case "\\":
                        l[i] =
                            (Convert.ToInt32(l[i - 2]) /
                             Convert.ToInt32(l[i - 1]))
                            .ToString();
                        l.RemoveAt(i - 2);
                        l.RemoveAt(i - 2);
                        i -= 2;
                        break;
                    case "*":
                        l[i] =
                            (Convert.ToDouble(l[i - 2]) *
                             Convert.ToDouble(l[i - 1])).ToString();
                        l.RemoveAt(i - 2);
                        l.RemoveAt(i - 2);
                        i -= 2;
                        break;
                    case "MOD":
                        l[i] =
                            (Convert.ToInt32(l[i - 2]) %
                             Convert.ToInt32(l[i - 1]))
                            .ToString();
                        l.RemoveAt(i - 2);
                        l.RemoveAt(i - 2);
                        i -= 2;
                        break;
                    case "SQR":
                        l[i] = Math.Sqrt(Convert.ToDouble(l[i - 1]))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "SQUARE":
                        l[i] = Math.Pow(Convert.ToDouble(l[i - 1]), 2)
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "CUBE":
                        l[i] = Math.Pow(Convert.ToDouble(l[i - 1]), 3)
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "INT":
                        l[i] = Convert.ToInt32(l[i - 1]).ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "FIX":
                        l[i] = Convert.ToInt32(l[i - 1]).ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "ROUND":
                        l[i] = Math.Round(Convert.ToDouble(l[i - 1]))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "RND":
                        var rnd = new Random();
                        var value = rnd.Next(0, Convert.ToInt32(l[i - 1]));
                        l[i] = value.ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "ABS":
                        l[i] =
                            Math.Abs(Convert.ToDouble(l[i - 1])).ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "SGN":
                        l[i] = Math.Sign(Convert.ToDouble(l[i - 1]))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "REPROC":
                        l[i] = Math.Sign(Convert.ToDouble(l[i - 1]))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "INVSGN":
                        l[i] = Convert.ToDouble(l[i - 1]).ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "FACT":
                        break;
                    case "SIN":
                        l[i] =
                            Math.Sin(Convert.ToDouble(l[i - 1])).ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "COS":
                        l[i] =
                            Math.Cos(Convert.ToDouble(l[i - 1])).ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "TAN":
                        l[i] =
                            Math.Tan(Convert.ToDouble(l[i - 1])).ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "SEC":
                        l[i] = (1.0 / Math.Cos(Convert.ToDouble(l[i - 1])))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "COSEC":
                        l[i] = (1.0 / Math.Sin(Convert.ToDouble(l[i - 1])))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "COT":
                        l[i] = (1.0 / Math.Tan(Convert.ToDouble(l[i - 1])))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "HSIN":
                        l[i] = (1.0 / Math.Sinh(Convert.ToDouble(l[i - 1])))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "HCOS":
                        l[i] = (1.0 / Math.Cosh(Convert.ToDouble(l[i - 1])))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "HTAN":
                        l[i] = (1.0 / Math.Tanh(Convert.ToDouble(l[i - 1])))
                            .ToString();
                        l.RemoveAt(i - 1);
                        i -= 1;
                        break;
                    case "HSEC":
                        break;
                    case "HCOSEC":
                        break;
                    case "HCOT":
                        break;
                }
            }

            double output = 0;

            var ret = double.TryParse(l.Last(), out output);

            if (!ret)
            {
                throw new InvalidFormatException();
            }

            return output;
        }

        public IEnumerable<string> GetQueue()
        {
            return _queue;
        }

        public void Add(string value)
        {
            _queue.Add(value);
        }
    }
}