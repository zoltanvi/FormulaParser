using FormulaParser.Interpreter;
using FormulaParser.Tokenizer;

namespace FormulaParser.Parser.SyntaxTree
{
    internal class UnaryOperationNode : ASTNode
    {
        public UnaryOperationNode(Token op, ASTNode operand)
        {
            AddOperand(operand);
            Token = op;
        }

        public override string ToString()
        {
            return Token.Value;
        }

        public override dynamic AcceptInterpreter(ASTInterpreter interpreter)
        {
            return interpreter.Calculate(this);
        }
    }
}