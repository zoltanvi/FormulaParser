using System.Collections.Generic;

namespace FormulaParser
{
    /// <summary>
    /// If a class implements this interface it must provide
    /// the SignalNames property that returns every
    /// existing signal name which can be resolved and used in a formula.
    /// </summary>
    internal interface ISignalNameProvider
    {
        IEnumerable<string> SignalNames { get; }
    }
}