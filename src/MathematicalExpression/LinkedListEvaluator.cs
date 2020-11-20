using System;
using System.Collections.Generic;
using System.Linq;

namespace MathematicalExpression
{
    public class LinkedListEvaluator : IEvaluator
    {
        private LinkedList<string> _queue = new LinkedList<string>();
        private LinkedListEvaluator() { }
        public static IEvaluator Instance { get; } = new LinkedListEvaluator();

        public double Calculate(IEnumerable<string> postFixExpression)
        {
            LinkedListNode<string> ApplyDoubleOperation(
                LinkedListNode<string> linkedListNode,
                LinkedList<string> linkedList,
                Func<double, double, double> operation)
            {
                linkedListNode = linkedListNode.Previous;
                var minus1 = linkedListNode;
                linkedListNode = linkedListNode.Previous;
                var minus2 = linkedListNode;

                linkedListNode = linkedListNode.Next.Next;

                linkedList.Remove(minus1);
                linkedList.Remove(minus2);

                linkedListNode.Value =
                    operation(Convert.ToDouble(minus2.Value),
                        Convert.ToDouble(minus1.Value)).ToString();
                return linkedListNode;
            }

            LinkedListNode<string> ApplySingleOperation(
                LinkedListNode<string> linkedListNode,
                LinkedList<string> linkedList,
                Func<double, double> operation)
            {
                linkedListNode = linkedListNode.Previous;
                var minus1 = linkedListNode;

                linkedListNode = linkedListNode.Next.Next;

                linkedList.Remove(minus1);

                linkedListNode.Value =
                    operation(Convert.ToDouble(minus1.Value)).ToString();
                return linkedListNode;
            }


            if (!(postFixExpression is LinkedList<string>))
            {
                throw new UnsupportedTypeException();
            }

            var l = postFixExpression as LinkedList<string>;
            var node = l.First;

            var start = DateTime.Now;

            while (node != null)
            {
                if (node.Value == "+")
                {
                    node = ApplyDoubleOperation(node, l, (x, y) => x + y);
                }

                if (node.Value.StartsWith("FUNC_"))
                {
                    var func = Utils.GetFunctionDelegate(node.Value.Split('_')[1]);
                    node = ApplySingleOperation(node, l, func);
                }

                node = node.Next;
            }

            var end = DateTime.Now;
            Console.WriteLine((end - start).TotalMilliseconds);
            double output = 0;

            var ret = double.TryParse(l.Last(), out output);

            if (!ret)
            {
                throw new InvalidFormatException();
            }

            return output;
        }

        IEnumerable<string> IEvaluator.GetQueue()
        {
            return _queue;
        }

        public void Add(string value)
        {
            if (_queue.Count == 0)
            {
                _queue.AddFirst(value);
            }
            else
            {
                _queue.AddLast(value);
            }
        }
    }
}