namespace FormulaParser.Tokenizer
{
    public enum TokenType
    {
        Invalid,
        Plus,
        Minus,
        Multiplication,
        Division,
        LeftParenthesis,   //  (
        RightParenthesis,  //  )
        Comma,
        Number,
        Variable,
        Dot,
        LeftBracket,      //  [
        RightBracket,     //  ]
        ABS,
        POW,
        SQRT,
        MIN,
        MAX,
        EOL         // End Of Line
    }
}