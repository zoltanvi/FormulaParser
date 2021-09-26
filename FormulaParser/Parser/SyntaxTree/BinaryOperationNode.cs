using FormulaParser.Interpreter;
using FormulaParser.Tokenizer;

namespace FormulaParser.Parser.SyntaxTree
{
    internal class BinaryOperationNode : ASTNode
    {
        public BinaryOperationNode(Token op, ASTNode leftOperand, ASTNode rightOperand)
        {
            Token = op;
            AddOperand(leftOperand);
            AddOperand(rightOperand);
        }

        public override string ToString()
        {
            return $"{Token.TokenType}";
        }

        public override dynamic AcceptInterpreter(ASTInterpreter interpreter)
        {
            return interpreter.Calculate(this);
        }
    }
}