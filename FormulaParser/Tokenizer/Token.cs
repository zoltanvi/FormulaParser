using System;

namespace FormulaParser.Tokenizer
{
    internal class Token : IEquatable<Token>
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }

        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public override string ToString()
        {
            return $"{{{TokenType}}}";
        }

        #region IEquatable for unit testing

        public bool Equals(Token other)
        {
            return TokenType == other?.TokenType;
        }

        public override bool Equals(object obj)
        {
            return Equals((Token)obj);
        }

        public override int GetHashCode()
        {
            return TokenType.GetHashCode() + Value.GetHashCode();
        }

        #endregion IEquatable for unit testing
    }
}