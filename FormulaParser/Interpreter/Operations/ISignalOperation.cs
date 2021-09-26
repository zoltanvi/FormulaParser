namespace FormulaParser.Interpreter.Operations
{
    public interface ISignalOperation
    {
        void DoOperation(ref double operateValue, double value);
    }
}