using System;

namespace FormulaParser.Interpreter
{
    internal class FunctionParameterValidator
    {
        // Validation example
        public void ValidateMinParameters(dynamic left, dynamic right)
        {
            if (!(left is Signal && right is double))
            {
                throw new Exception("MIN(Signal, double) : Invalid parameter type used!");
            }

            double val = Convert.ToDouble(right);

            if (val < 0 || val > 5)
            {
                //Console.WriteLine("Validation example.");
            }
        }

        public void ValidateMaxParameters(dynamic left, dynamic right)
        {
            if (!(left is Signal && right is double))
            {
                throw new Exception("MAX(Signal, double) : Invalid parameter type used!");
            }

            double val = Convert.ToDouble(right);

            if (val < 0 || val > 5)
            {
                //Console.WriteLine("Validation example.");
            }
        }

        internal void ValidatePowParameters(dynamic left, dynamic right)
        {
            if (right is double && (left is Signal || left is double))
            {
                return;
            }

            throw new Exception("POW(Signal, double) OR POW(double, double): Invalid parameter type used!");
        }
    }
}
