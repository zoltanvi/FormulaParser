namespace FormulaParser.Interpreter.Operations
{
    internal class SignalOperationAdd : ISignalOperation
    {
        public void DoOperation(ref double operateValue, double value) => operateValue += value;
    }

    internal class SignalOperationSubtract : ISignalOperation
    {
        public void DoOperation(ref double operateValue, double value) => operateValue -= value;
    }

    internal class SignalOperationMultiply : ISignalOperation
    {
        public void DoOperation(ref double operateValue, double value) => operateValue *= value;
    }

    internal class SignalOperationDivide : ISignalOperation
    {
        public void DoOperation(ref double operateValue, double value) => operateValue /= value;
    }
}