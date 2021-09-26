using System;
using FormulaParser.Parser.SyntaxTree;
using FormulaParser.Tokenizer;

namespace FormulaParser.Parser
{
    /// <summary>
    /// Abstract Syntax Tree Parser
    /// Parses a list- or stream of <see cref="Token"/>s into an abstract syntax tree.
    /// </summary>
    /// <remarks>
    /// The following context free grammar is defined in this class:
    ///
    /// <code>
    /// Expression -->
    ///       MultiExpression '+' MultiExpression
    ///    |  MultiExpression '-' MultiExpression
    ///    |  MultiExpression
    ///
    /// MultiExpression -->
    ///       UnaryExpression '*' UnaryExpression
    ///    |  UnaryExpression '/' UnaryExpression
    ///    |  UnaryExpression '%' UnaryExpression
    ///    |  '(' Expression ')'
    ///
    /// UnaryExpression -->
    ///       '-' UnaryExpression
    ///    |  '+' UnaryExpression
    ///    |  '(' Expression ')'
    ///    |  Method
    ///
    /// Method -->
    ///       ABS  '(' Expression ')'
    ///    |  SQRT '(' Expression ')'
    ///    |  MIN  '(' Expression ',' Expression ')'
    ///    |  MAX  '(' Expression ',' Expression ')'
    ///    |  POW  '(' Expression ',' Expression ')'
    ///    |  Terminal
    ///
    /// Terminal -->
    ///       SignalRef
    ///    |  Number
    /// </code>
    /// 
    /// Some references:
    /// - https://en.wikipedia.org/wiki/Context-free_grammar
    /// - https://en.wikipedia.org/wiki/Abstract_syntax_tree
    /// </remarks>
    internal class ASTParser
    {
        private readonly ITokenizer m_Tokenizer;
        private Token m_CurrentToken;

        public ASTParser(ITokenizer tokenizer)
        {
            m_Tokenizer = tokenizer;
            m_CurrentToken = m_Tokenizer.NextToken();
        }

        private void ConsumeToken(TokenType tokenType)
        {
            if (m_CurrentToken.TokenType == tokenType)
            {
                m_CurrentToken = m_Tokenizer.NextToken();
            }
            else
            {
                throw new Exception($"Unexpected token: {tokenType}");
            }
        }

        // Expression -->
        //       MultiExpression '+' MultiExpression
        //    |  MultiExpression '-' MultiExpression
        //    |  MultiExpression
        private ASTNode Expression()
        {
            ASTNode astNode = MultiExpression();

            while (m_CurrentToken.TokenType == TokenType.Plus ||
                   m_CurrentToken.TokenType == TokenType.Minus)
            {
                Token token = m_CurrentToken;
                ConsumeToken(m_CurrentToken.TokenType);
                astNode = new BinaryOperationNode(token, astNode, MultiExpression());
            }

            return astNode;
        }

        // MultiExpression -->
        //       UnaryExpression '*' UnaryExpression
        //    |  UnaryExpression '/' UnaryExpression
        //    |  UnaryExpression '%' UnaryExpression
        //    |  '(' Expression ')'
        private ASTNode MultiExpression()
        {
            ASTNode astNode = UnaryExpression();
            Token token = m_CurrentToken;
            if (astNode != null)
            {
                // TODO: Modulo (%) is not yet implemented!
                while (m_CurrentToken.TokenType == TokenType.Multiplication ||
                       m_CurrentToken.TokenType == TokenType.Division)
                {
                    token = m_CurrentToken;
                    ConsumeToken(m_CurrentToken.TokenType);
                    astNode = new BinaryOperationNode(token, astNode, UnaryExpression());
                }
            }
            else
            {
                if (token.TokenType == TokenType.LeftParenthesis)
                {
                    astNode = Expression();
                    ConsumeToken(TokenType.RightParenthesis);
                }
            }

            return astNode;
        }

        // UnaryExpression -->
        //       '-' UnaryExpression
        //    |  '+' UnaryExpression
        //    |  '(' Expression ')'
        //    |  Method
        private ASTNode UnaryExpression()
        {
            ASTNode astNode;
            Token token = m_CurrentToken;

            while (m_CurrentToken.TokenType == TokenType.Minus ||
                   m_CurrentToken.TokenType == TokenType.Plus)
            {
                token = m_CurrentToken;
                ConsumeToken(token.TokenType);
                return new UnaryOperationNode(token, UnaryExpression());
            }

            if (token.TokenType == TokenType.LeftParenthesis)
            {
                ConsumeToken(token.TokenType);
                astNode = Expression();
                ConsumeToken(TokenType.RightParenthesis);
            }
            else
            {
                astNode = Method();
            }

            return astNode;
        }

        // Method -->
        //       ABS  '(' Expression ')'
        //    |  SQRT '(' Expression ')'
        //    |  MIN  '(' Expression ',' Expression ')'
        //    |  MAX  '(' Expression ',' Expression ')'
        //    |  POW  '(' Expression ',' Expression ')'
        //    |  Terminal
        private ASTNode Method()
        {
            bool processed = false;
            ASTNode astNode = null;
            Token token = m_CurrentToken;
            switch (token.TokenType)
            {
                case TokenType.ABS:
                case TokenType.SQRT:
                {
                    ConsumeToken(token.TokenType);
                    ConsumeToken(TokenType.LeftParenthesis);
                    astNode = new UnaryOperationNode(token, Expression());
                    ConsumeToken(TokenType.RightParenthesis);
                    processed = true;
                    break;
                }
                case TokenType.MIN:
                case TokenType.MAX:
                case TokenType.POW:
                {
                    ConsumeToken(token.TokenType);
                    ConsumeToken(TokenType.LeftParenthesis);
                    var leftOperand = Expression();
                    ConsumeToken(TokenType.Comma);
                    var rightOperand = Expression();
                    astNode = new BinaryOperationNode(token, leftOperand, rightOperand);
                    ConsumeToken(TokenType.RightParenthesis);
                    processed = true;
                    break;
                }
                // TODO: 3 parameters or more
            }

            if (!processed)
            {
                astNode = Primary();
            }

            return astNode;
        }

        // Terminal -->
        //       SignalRef
        //    |  Number
        private ASTNode Primary()
        {
            ASTNode astNode = null;
            Token token = m_CurrentToken;
            switch (token.TokenType)
            {
                case TokenType.Number:
                {
                    ConsumeToken(token.TokenType);
                    astNode = new DoubleNode(token);
                    break;
                }
                case TokenType.Variable:
                {
                    ConsumeToken(token.TokenType);
                    astNode = new SignalNode(token);
                    break;
                }
            }

            return astNode;
        }

        public ASTNode Parse()
        {
            var node = Expression();
            if (m_CurrentToken.TokenType != TokenType.EOL)
            {
                //throw new Exception();
            }
            return node;
        }
    }
}