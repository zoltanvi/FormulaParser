using System.Text.RegularExpressions;

namespace FormulaParser.Tokenizer
{
    internal class TokenDefinition
    {
        private static readonly RegexOptions s_RegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;

        public string Pattern { get; }
        
        public TokenType TokenType { get; }
        
        public Regex Regex { get; set; }

        /// <summary>
        /// Creates a new TokenDefinition which is used to identify tokens in the formula string
        /// </summary>
        /// <param name="tokenType">The tokenType</param>
        /// <param name="pattern"></param>
        public TokenDefinition(TokenType tokenType, string pattern)
        {
            TokenType = tokenType;
            Pattern = pattern;
            Regex = new Regex(Pattern, s_RegexOptions);
        }
    }
}