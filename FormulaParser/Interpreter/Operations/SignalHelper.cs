namespace FormulaParser.Interpreter.Operations
{
    internal static class SignalHelper
    {
        public static ISignalOperation SignalOperationAdd { get; } = new SignalOperationAdd();
        public static ISignalOperation SignalOperationSubtract { get; } = new SignalOperationSubtract();
        public static ISignalOperation SignalOperationMultiply { get; } = new SignalOperationMultiply();
        public static ISignalOperation SignalOperationDivide { get; } = new SignalOperationDivide();

        public static Signal Operate(Signal a, Signal b, ISignalOperation operation)
        {
            Signal resultSignal = new Signal(a.Name, a.Samples);
            int rightLength = b.Samples.Length;
            for (int i = 0; i < resultSignal.Samples.Length; i++)
            {
                if (rightLength <= i)
                {
                    break;
                }

                operation.DoOperation(ref resultSignal.Samples[i], b.Samples[i]);
            }
            return resultSignal;
        }

        public static Signal Operate(Signal a, double b, ISignalOperation operation)
        {
            Signal tmp = new Signal(a.Name, a.Samples);
            for (int i = 0; i < tmp.Samples.Length; i++)
            {
                operation.DoOperation(ref tmp.Samples[i], b);
            }
            return tmp;
        }

        public static Signal Operate(double a, Signal b, ISignalOperation operation)
        {
            Signal tmp = new Signal($"ConstantSignal_{a}", a, b.Samples.Length);
            for (int i = 0; i < tmp.Samples.Length; i++)
            {
                operation.DoOperation(ref tmp.Samples[i], b.Samples[i]);
            }
            return tmp;
        }
    }
}