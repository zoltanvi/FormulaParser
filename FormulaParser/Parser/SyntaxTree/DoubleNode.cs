using System;
using System.Globalization;
using FormulaParser.Interpreter;
using FormulaParser.Tokenizer;

namespace FormulaParser.Parser.SyntaxTree
{
    internal class DoubleNode : ASTNode
    {
        public double Value { get; protected set; }

        public DoubleNode(Token token)
        {
            Token = token;

            // Leading signs are never used because the plus and minus characters are parsed earlier as operators
            NumberStyles numberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
            if (!double.TryParse(token.Value, numberStyles, CultureInfo.InvariantCulture, out double tokenValue))
            {
                throw new InvalidCastException($"Failed to cast '{token.Value}' to double!");
            }

            Value = tokenValue;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override dynamic AcceptInterpreter(ASTInterpreter interpreter)
        {
            return interpreter.Calculate(this);
        }
    }
}