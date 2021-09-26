using FormulaParser.Interpreter;
using FormulaParser.Tokenizer;

namespace FormulaParser.Parser.SyntaxTree
{
    internal class SignalNode : ASTNode
    {
        public string Name { get; }

        public Signal Signal
        {
            get
            {
                Signal tmpSignal;

                if (int.TryParse(Name.Replace("$", string.Empty), out var index))
                {
                    tmpSignal = SignalCollection.GetSignal(index);
                    if (tmpSignal != null)
                    {
                        return tmpSignal;
                    }
                }
                else
                {
                    var processedName = Name.Replace(@"""", string.Empty);
                    tmpSignal = SignalCollection.GetSignal(processedName);
                    if (tmpSignal != null)
                    {
                        return tmpSignal;
                    }
                }

                return new Signal("constant_1000", 1000d, 30);
            }
        }

        public SignalNode(Token token)
        {
            Token = token;
            Name = token.Value;
        }

        public override string ToString()
        {
            return Name;
        }

        public override dynamic AcceptInterpreter(ASTInterpreter interpreter)
        {
            return interpreter.Calculate(this);
        }
    }
}