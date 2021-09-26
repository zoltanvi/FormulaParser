using System;

namespace FormulaParser
{
    internal static class Formulas
    {
        public static Signal MIN(Signal a, double n)
        {
            return ForEachSample(a, n, Math.Min);
        }

        public static Signal MAX(Signal a, double n)
        {
            return ForEachSample(a, n, Math.Max);
        }

        public static double SQRT(double a)
        {
            return Math.Sqrt(a);
        }

        public static Signal SQRT(Signal a)
        {
            return ForEachSample(a, SQRT);
        }

        public static double POW(double left, double right)
        {
            return Math.Pow(left, right);
        }

        public static Signal POW(Signal left, double right)
        {
            return ForEachSample(left, right, POW);
        }

        private static Signal ForEachSample(Signal signal, double number, Func<double, double, double> function)
        {
            Signal tmp = new Signal(signal.Name, signal.Samples.Length);

            for (int i = 0; i < signal.Samples.Length; i++)
            {
                tmp.Samples[i] = function(signal.Samples[i], number);
            }

            return tmp;
        }

        private static Signal ForEachSample(Signal signal, Func<double, double> function)
        {
            Signal tmp = new Signal(signal.Name, signal.Samples.Length);

            for (int i = 0; i < signal.Samples.Length; i++)
            {
                tmp.Samples[i] = function(signal.Samples[i]);
            }

            return tmp;
        }
    }
}