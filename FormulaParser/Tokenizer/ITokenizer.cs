using System.Collections.Generic;

namespace FormulaParser.Tokenizer
{
    internal interface ITokenizer
    {
        Token NextToken();

        IList<Token> GetAllTokens();
    }
}