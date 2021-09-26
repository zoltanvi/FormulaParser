using System.Collections.Generic;
using FormulaParser.Interpreter;
using FormulaParser.Tokenizer;

namespace FormulaParser.Parser.SyntaxTree
{
    /// <summary>
    /// Abstract Syntax Tree Node
    /// </summary>
    internal abstract class ASTNode
    {
        public Token Token { get; protected set; }

        public IList<ASTNode> Operands { get; protected set; }
        
        public int ChildNodes => Operands.Count;

        protected ASTNode()
        {
            Operands = new List<ASTNode>();
        }

        public abstract dynamic AcceptInterpreter(ASTInterpreter interpreter);

        public void AddOperand(ASTNode astNode)
        {
            if (astNode != null)
            {
                Operands.Add(astNode);
            }
        }

        public override string ToString()
        {
            return $"{Token.TokenType}";
        }
    }
}