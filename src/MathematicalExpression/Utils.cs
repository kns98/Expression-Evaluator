using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace MathematicalExpression
{
    public static class Utils
    {
        public static bool IsAlpha(string TestNo)
        {
            if (TestNo.Length == 0)
            {
                return false;
            }

            var cTestNo = TestNo[0];
            if (cTestNo >= 65 && cTestNo <= 90 || cTestNo >= 97 && cTestNo <= 122)
            {
                return true;
            }

            return false;
        }

        public static bool IsAlphaNumeric(string TestNo)
        {
            if (TestNo.Length == 0)
            {
                return false;
            }

            var cTestNo = TestNo[0];
            if (cTestNo >= 48 && cTestNo <= 57 || cTestNo >= 65 && cTestNo <= 90
                                               || cTestNo >= 97 && cTestNo <= 122)
            {
                return true;
            }

            return false;
        }

        public static bool IsNumber(string TestNo)
        {
            if (TestNo.Length == 0)
            {
                return false;
            }

            var cTestNo = TestNo[0];
            if (cTestNo >= 48 && cTestNo <= 57)
            {
                return true;
            }

            return false;
        }

        public static Func<double, double> GetFunctionDelegate(string functionName)
        {
            switch (functionName)
            {
                case "SQR":
                    return r => Math.Pow(r, 0.5d);
                case "SQUARE":
                    return r => Math.Pow(r, 2d);
                case "CUBE":
                    return r => Math.Pow(r, 3d);
                case "INT":
                    return r => Math.Floor(r);
                case "FIX":
                    return r => Conversion.Fix(r);
                case "ROUND":
                    return r => Math.Round(r, MidpointRounding.ToEven);
                case "RND":
                    return r => new Random().NextDouble();
                case "ABS":
                    return r => Math.Abs(r);
                case "SGN":
                    return r => Math.Sign(r);
                case "INVSGN":
                    return r => -Math.Sign(r);
                case "FACT":

                    double Fact(double r)
                    {
                        var i = Convert.ToInt64(r);
                        if (i == 1)
                        {
                            return 1;
                        }
                        else
                        {
                            return Convert.ToDouble(i * Fact(i - 1));
                        }
                    }

                    return Fact;


                case "SIN":
                    return r => Math.Sin(r);

                case "COS":
                    return r => Math.Cos(r);

                case "TAN":
                    return r => Math.Tan(r);

                case "SEC": //secant
                    return r => 1 / Math.Cos(r);

                case "COSEC": //cosecant
                    return r => 1 / Math.Sin(r);

                case "COT": //cotangent
                    return r => 1 / Math.Tan(r);

                case "HSIN":
                    return r => Math.Sinh(r);

                case "HCOS": //hyperbolic cosine
                    return r => Math.Cosh(r);

                case "HTAN":
                    return r => Math.Tanh(r);

                case "HSEC": //sech x = 1/cosh x.
                    return r => 1 / Math.Cosh(r);

                case "HCOSEC": //hyperbolic cosecant
                    return r => 1 / Math.Sinh(r); //check

                case "HCOT": //hyperbolic cotangent : coth x = cosh x/sinh x
                    return r => Math.Cosh(r) / Math.Sinh(r);

                default:
                    throw new UnsupportedFunctionException();
            }
        }
    }
}