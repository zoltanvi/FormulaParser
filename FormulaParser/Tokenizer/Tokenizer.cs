using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FormulaParser.Tokenizer
{
    internal class Tokenizer : ITokenizer
    {
        private readonly ISignalNameProvider m_SignalNameProvider;
        private readonly List<TokenDefinition> m_TokenDefinitions;
        private readonly string m_OriginalString;
        private string m_ProcessingString;
        private Token m_LastToken;
        private readonly StringBuilder m_StringBuilder;

        public Tokenizer(string str, ISignalNameProvider signalNameProvider = null)
        {
            m_SignalNameProvider = signalNameProvider;
            m_ProcessingString = str;
            m_OriginalString = str;
            m_StringBuilder = new StringBuilder();

            // Convert input string to uppercase
            //m_ProcessingString = m_ProcessingString.ToUpperInvariant();
            //m_ProcessingString = ResolveVariableNames(m_ProcessingString);

            // It is important to define later the tokens which are matches another tokens as well
            // e.g: SQR matches to SQRT
            // if the formula is the following: SQRT(5),
            // and we create an SQR token, the leftover string cannot be tokenized: T(5)   !!
            m_TokenDefinitions = new List<TokenDefinition>
            {
                new TokenDefinition(TokenType.Plus, @"^(\+)"),
                new TokenDefinition(TokenType.Minus, @"^(\-)"),
                new TokenDefinition(TokenType.Multiplication, @"^(\*)"),
                new TokenDefinition(TokenType.Division, @"^(\/)"),
                new TokenDefinition(TokenType.Number, @"^(\d+(\.\d+)?)"),
                new TokenDefinition(TokenType.LeftParenthesis, @"^(\()"),
                new TokenDefinition(TokenType.RightParenthesis, @"^(\))"),
                new TokenDefinition(TokenType.Comma, @"^(\,)"),
                new TokenDefinition(TokenType.Variable, @"^(\$\d+)"), // e.g:  $5
                new TokenDefinition(TokenType.Dot, @"^(\.)"),
                new TokenDefinition(TokenType.LeftBracket, @"^(\[)"),
                new TokenDefinition(TokenType.RightBracket, @"^(\])"),
                //  There is no need for namedVariable if we replace
                //  the "namedVariables" with Variables before tokenizing the input,
                // but it's a possibility
                //new TokenDefinition(TokenType.NamedVariable, @"^(\"".+?\"")"),
                new TokenDefinition(TokenType.ABS, @"^(ABS)"),
                new TokenDefinition(TokenType.SQRT, @"^(SQRT)"),
                new TokenDefinition(TokenType.POW, @"^(POW)"),
                new TokenDefinition(TokenType.MIN, @"^(MIN)"),
                new TokenDefinition(TokenType.MAX, @"^(MAX)"),
            };
        }

        public Token NextToken()
        {
            if (string.IsNullOrWhiteSpace(m_ProcessingString))
            {
                return m_LastToken = new Token(TokenType.EOL, string.Empty);
            }

            // The signal names have higher priority than everything else in the tokenization process,
            // because a signal name literally can be everything.
            if (m_SignalNameProvider != null)
            {
                foreach (var signalName in m_SignalNameProvider.SignalNames)
                {
                    // remove leading spaces from the beginning of the string
                    m_ProcessingString = m_ProcessingString.TrimStart(' ');
                    var nameRegex = new Regex(CreateSignalNameRegexPattern(signalName));
                    Match match = nameRegex.Match(m_ProcessingString);

                    if (match.Success)
                    {
                        string token = match.Groups[1].Value;
                        m_ProcessingString = m_ProcessingString.Remove(0, token.Length);
                        return m_LastToken = new Token(TokenType.Variable, token);
                    }
                }
            }

            foreach (var tokenDefinition in m_TokenDefinitions)
            {
                // remove leading spaces from the beginning of the string
                m_ProcessingString = m_ProcessingString.TrimStart(' ');
                Match match = tokenDefinition.Regex.Match(m_ProcessingString);

                if (match.Success)
                {
                    string token = match.Groups[1].Value;
                    m_ProcessingString = m_ProcessingString.Remove(0, token.Length);

                    return m_LastToken = new Token(tokenDefinition.TokenType, token);
                }
            }

            WriteErrorMessage();
            m_ProcessingString = string.Empty;
            return m_LastToken = new Token(TokenType.Invalid, string.Empty);
        }

        public IList<Token> GetAllTokens()
        {
            List<Token> tokens = new List<Token>();
            Token current = NextToken();
            while (current.TokenType != TokenType.EOL &&
                   current.TokenType != TokenType.Invalid)
            {
                tokens.Add(current);
                current = NextToken();
            }

            return tokens;
        }

        private void WriteErrorMessage()
        {
            Console.WriteLine(m_OriginalString);
            int startIndex = m_OriginalString.IndexOf(m_ProcessingString, StringComparison.InvariantCulture);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < startIndex; i++) { sb.Append(" "); }
            sb.Append("^");
            Console.WriteLine(sb.ToString());
            Console.WriteLine($"Syntax error at character {startIndex}");
        }

        private string CreateSignalNameRegexPattern(string signalName)
        {
            string escapedSignalName = Regex.Escape(signalName);
            m_StringBuilder.Clear();
            m_StringBuilder.Append("^(");
            m_StringBuilder.Append("\\\"");
            m_StringBuilder.Append(escapedSignalName);
            m_StringBuilder.Append("\\\"");
            m_StringBuilder.Append("|");
            m_StringBuilder.Append(escapedSignalName);
            m_StringBuilder.Append(")");
            return m_StringBuilder.ToString();
        }
    }
}