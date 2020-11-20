using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalExpression
{
    public interface IEvaluator
    {
        double Calculate(IEnumerable<string> postFixExpression);
        IEnumerable<string> GetQueue();

        void Add(string value);
    }
}