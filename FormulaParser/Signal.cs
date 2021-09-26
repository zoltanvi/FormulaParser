using System;
using System.Text;
using FormulaParser.Interpreter.Operations;

namespace FormulaParser
{
    internal class Signal
    {
        private static int s_NextIndex;
        private static int NextIndex => s_NextIndex++;
        public double[] Samples { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }

        public Signal(string name)
        {
            Name = name;
            Id = NextIndex;
        }

        public Signal(string name, double[] samples) : this(name)
        {
            Samples = new double[samples.Length];
            Array.Copy(samples, Samples, samples.Length);
        }

        public Signal(string name, double value, int size) : this(name)
        {
            Samples = new double[size];
            for (int i = 0; i < size; i++)
            {
                Samples[i] = value;
            }
        }

        public Signal(string name, int size) : this(name)
        {
            Samples = new double[size];
        }

        ///// Signal operator overloading ///////////////////////////////////////////////////////////////////////////////////////

        // +signal
        public static Signal operator +(Signal a)
        {
            return a;
        }

        // -signal
        public static Signal operator -(Signal a)
        {
            return SignalHelper.Operate(a, -1d, SignalHelper.SignalOperationMultiply);
        }

        public static Signal operator -(Signal a, Signal b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationSubtract);
        }

        public static Signal operator +(Signal a, Signal b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationAdd);
        }

        public static Signal operator *(Signal a, Signal b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationMultiply);
        }

        public static Signal operator /(Signal a, Signal b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationDivide);
        }

        public static Signal operator +(Signal a, double b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationAdd);
        }

        public static Signal operator -(Signal a, double b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationSubtract);
        }

        public static Signal operator *(Signal a, double b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationMultiply);
        }

        public static Signal operator /(Signal a, double b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationDivide);
        }

        public static Signal operator +(double a, Signal b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationAdd);
        }

        public static Signal operator -(double a, Signal b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationSubtract);
        }

        public static Signal operator *(double a, Signal b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationMultiply);
        }

        public static Signal operator /(double a, Signal b)
        {
            return SignalHelper.Operate(a, b, SignalHelper.SignalOperationDivide);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(110);
            sb.Append($"${Id} values: ");
            int length = Samples.Length < 20 ? Samples.Length : 20;
            for (int i = 0; i < length; i++)
            {
                sb.Append(Samples[i]);
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}