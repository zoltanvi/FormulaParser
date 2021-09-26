using System;
using FormulaParser.Parser.SyntaxTree;
using FormulaParser.Tokenizer;

namespace FormulaParser.Interpreter
{
    /// <summary>
    /// Abstract syntax tree interpreter
    /// </summary>
    internal class ASTInterpreter
    {
        private readonly FunctionParameterValidator m_Validator = new FunctionParameterValidator();

        public dynamic Calculate(TernaryOperationNode ternaryOperationNode)
        {
            throw new NotImplementedException("Ternary operations are not yet implemented!");
        }

        public dynamic Calculate(BinaryOperationNode binaryOperationNode)
        {
            Console.WriteLine($"Calculating: {binaryOperationNode.Token}({string.Join(",", binaryOperationNode.Operands)})");

            // The type is only known at runtime!
            dynamic left;
            dynamic right;

            CheckOperands(binaryOperationNode, 2);

            left = binaryOperationNode.Operands[0].AcceptInterpreter(this);
            right = binaryOperationNode.Operands[1].AcceptInterpreter(this);

            switch (binaryOperationNode.Token.TokenType)
            {
                case TokenType.Minus:
                {
                    return left - right;
                }
                case TokenType.Plus:
                {
                    return left + right;
                }
                case TokenType.Multiplication:
                {
                    return left * right;
                }
                case TokenType.Division:
                {
                    return left / right;
                }
                case TokenType.MIN:
                {
                    m_Validator.ValidateMinParameters(left, right);
                    return Formulas.MIN(left, Convert.ToDouble(right));
                }
                case TokenType.MAX:
                {
                    m_Validator.ValidateMaxParameters(left, right);
                    return Formulas.MAX(left, Convert.ToDouble(right));
                }
                case TokenType.POW:
                {
                    m_Validator.ValidatePowParameters(left, right);
                    return Formulas.POW(left, Convert.ToDouble(right));
                }
            }

            throw new Exception("Unexpected binary operation!");
        }

        public dynamic Calculate(UnaryOperationNode unaryOperationNode)
        {
            Console.WriteLine($"Calculating: {unaryOperationNode.Token}({unaryOperationNode.Operands[0]})");

            dynamic resultValue;
            switch (unaryOperationNode.Token.TokenType)
            {
                case TokenType.Minus:
                {
                    resultValue = -unaryOperationNode.Operands[0].AcceptInterpreter(this);
                    return resultValue;
                }
                case TokenType.Plus:
                {
                    resultValue = unaryOperationNode.Operands[0].AcceptInterpreter(this);
                    return resultValue;
                }
                case TokenType.SQRT:
                {
                    resultValue = Formulas.SQRT(unaryOperationNode.Operands[0].AcceptInterpreter(this));
                    return resultValue;
                }
            }
            throw new Exception("Unexpected unary operation!");
        }

        public Signal Calculate(SignalNode signalNode)
        {
            Console.WriteLine($"Returning: \"{signalNode}\"");
            var returnValue = signalNode.Signal;
            return returnValue;
        }

        public double Calculate(DoubleNode doubleNode)
        {
            Console.WriteLine($"Returning: {doubleNode}");
            return doubleNode.Value;
        }

        public dynamic InterpretAbstractSyntaxTree(ASTNode astNode)
        {
            try
            {
                Console.WriteLine("Start interpreting Abstract Syntax Tree");
                return astNode.AcceptInterpreter(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        private void CheckOperands(ASTNode node, int expectedOperandCount)
        {
            if (node.ChildNodes != expectedOperandCount)
            {
                if (node.ChildNodes < expectedOperandCount)
                {
                    throw new Exception($"The operation {{{node.Token}}} was called with less operands than it should be.");
                }

                throw new Exception($"The operation {{{node.Token}}} was called with more operands than it should be.");
            }
        }
    }
}