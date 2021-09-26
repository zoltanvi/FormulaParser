using FormulaParser.Interpreter;
using FormulaParser.Tokenizer;

namespace FormulaParser.Parser.SyntaxTree
{
    // https://en.wikipedia.org/wiki/Arity
    // Nullary, Unary, Binary, Ternary, Quaternary, Quinary, Senary, Octonary, Novenary, Denary
    internal class TernaryOperationNode : ASTNode
    {
        public TernaryOperationNode(Token op, ASTNode firstOperand, ASTNode secondOperand, ASTNode thirdOperand)
        {
            Token = op;
            AddOperand(firstOperand);
            AddOperand(secondOperand);
            AddOperand(thirdOperand);
        }

        public override dynamic AcceptInterpreter(ASTInterpreter interpreter)
        {
            return interpreter.Calculate(this);
        }
    }
}