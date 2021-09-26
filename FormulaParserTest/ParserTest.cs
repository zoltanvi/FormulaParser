using FormulaParser.Parser;
using FormulaParser.Tokenizer;
using NSubstitute;
using NUnit.Framework;

namespace FormulaParserTest
{
    [TestFixture]
    internal class ParserTest
    {
        [Test]
        public void Parse_Add()
        {
            // Arrange
            // Input formula: 2 + 5
            var two = new Token(TokenType.Number, "2");
            var plus = new Token(TokenType.Plus, "+");
            var five = new Token(TokenType.Number, "5");

            ITokenizer tokenizer = Substitute.For<ITokenizer>();
            tokenizer.NextToken().Returns(two, plus, five);
            ASTParser astParser = new ASTParser(tokenizer);

            // Act
            var rootNode = astParser.Parse();

            // Assert
            Assert.True(rootNode.Token.Equals(plus));
            Assert.True(rootNode.Operands[0].Token.Equals(two));
            Assert.True(rootNode.Operands[1].Token.Equals(five));
            Assert.True(rootNode.ChildNodes == 2);
            Assert.True(rootNode.Operands[0].ChildNodes == 0);
            Assert.True(rootNode.Operands[1].ChildNodes == 0);

            // result tree should look like this
            //           Plus
            //           /   \
            //          2     5
        }

        [Test]
        public void Parse_NegativeNumbers()
        {
            // Arrange
            // Input formula: -5 * -1
            var minus = new Token(TokenType.Minus, "-");
            var five = new Token(TokenType.Number, "5");
            var asterisk = new Token(TokenType.Multiplication, "*");
            var minus2 = new Token(TokenType.Minus, "-");
            var one = new Token(TokenType.Number, "1");

            ITokenizer tokenizer = Substitute.For<ITokenizer>();
            tokenizer.NextToken().Returns(minus, five, asterisk, minus2, one);
            ASTParser astParser = new ASTParser(tokenizer);

            // Act
            var rootNode = astParser.Parse();

            // Assert
            Assert.True(rootNode.Token.Equals(asterisk));
            Assert.True(rootNode.Operands[0].Token.Equals(minus));
            Assert.True(rootNode.Operands[1].Token.Equals(minus2));
            Assert.True(rootNode.Operands[0].Operands[0].Token.Equals(five));
            Assert.True(rootNode.Operands[1].Operands[0].Token.Equals(one));
            Assert.True(rootNode.ChildNodes == 2);
            Assert.True(rootNode.Operands[0].ChildNodes == 1);
            Assert.True(rootNode.Operands[1].ChildNodes == 1);
            Assert.True(rootNode.Operands[0].Operands[0].ChildNodes == 0);
            Assert.True(rootNode.Operands[1].Operands[0].ChildNodes == 0);

            // result tree should look like this
            //            Mul
            //           /   \
            //      Minus     Minus
            //        |         |
            //        5         1
        }

        [Test]
        public void Parse_DoubleOperatorAdd()
        {
            // Arrange
            // Input formula: 2 ++ 5
            var two = new Token(TokenType.Number, "2");
            var plus = new Token(TokenType.Plus, "+");
            var plus2 = new Token(TokenType.Plus, "+");
            var five = new Token(TokenType.Number, "5");

            ITokenizer tokenizer = Substitute.For<ITokenizer>();
            tokenizer.NextToken().Returns(two, plus, plus2, five);
            ASTParser astParser = new ASTParser(tokenizer);

            // Act
            var rootNode = astParser.Parse();

            // Assert
            Assert.True(rootNode.Token.Equals(plus));
            Assert.True(rootNode.Operands[0].Token.Equals(two));
            Assert.True(rootNode.Operands[1].Token.Equals(plus2));
            Assert.True(rootNode.Operands[1].Operands[0].Token.Equals(five));
            Assert.True(rootNode.ChildNodes == 2);
            Assert.True(rootNode.Operands[0].ChildNodes == 0);
            Assert.True(rootNode.Operands[1].ChildNodes == 1);
            Assert.True(rootNode.Operands[1].Operands[0].ChildNodes == 0);
            // result tree should look like this
            //           Plus
            //           /   \
            //          2     Plus
            //                 |
            //                 5
        }

        [Test]
        public void Parse_Parentheses()
        {
            // Arrange
            // Input formula: ((((4.3))))
            var leftPar1 = new Token(TokenType.LeftParenthesis, "(");
            var leftPar2 = new Token(TokenType.LeftParenthesis, "(");
            var leftPar3 = new Token(TokenType.LeftParenthesis, "(");
            var leftPar4 = new Token(TokenType.LeftParenthesis, "(");
            var number = new Token(TokenType.Number, "4.3");
            var rightPar1 = new Token(TokenType.RightParenthesis, ")");
            var rightPar2 = new Token(TokenType.RightParenthesis, ")");
            var rightPar3 = new Token(TokenType.RightParenthesis, ")");
            var rightPar4 = new Token(TokenType.RightParenthesis, ")");

            ITokenizer tokenizer = Substitute.For<ITokenizer>();
            tokenizer.NextToken().Returns(leftPar1, leftPar2, leftPar3, leftPar4, number, rightPar1, rightPar2, rightPar3, rightPar4);
            ASTParser astParser = new ASTParser(tokenizer);

            // Act
            var rootNode = astParser.Parse();

            // Assert
            Assert.True(rootNode.Token.Equals(number));
            Assert.True(rootNode.ChildNodes == 0);
            // result tree should look like this
            //           4.3
        }

        [Test]
        public void Parse_PlusMinusParentheses()
        {
            // Arrange
            // Input formula: 5+(-32)
            var five = new Token(TokenType.Number, "5");
            var plus = new Token(TokenType.Plus, "+");
            var leftPar = new Token(TokenType.LeftParenthesis, "(");
            var minus = new Token(TokenType.Minus, "-");
            var thirtyTwo = new Token(TokenType.Number, "32");
            var rightPar = new Token(TokenType.RightParenthesis, ")");

            ITokenizer tokenizer = Substitute.For<ITokenizer>();
            tokenizer.NextToken().Returns(five, plus, leftPar, minus, thirtyTwo, rightPar);
            ASTParser astParser = new ASTParser(tokenizer);

            // Act
            var rootNode = astParser.Parse();

            // Assert
            Assert.True(rootNode.Token.Equals(plus));
            Assert.True(rootNode.Operands[0].Token.Equals(five));
            Assert.True(rootNode.Operands[1].Token.Equals(minus));
            Assert.True(rootNode.Operands[1].Operands[0].Token.Equals(thirtyTwo));
            Assert.True(rootNode.ChildNodes == 2);
            Assert.True(rootNode.Operands[0].ChildNodes == 0);
            Assert.True(rootNode.Operands[1].ChildNodes == 1);
            Assert.True(rootNode.Operands[1].Operands[0].ChildNodes == 0);
            // result tree should look like this
            //           Plus
            //           /  \
            //          5   Minus
            //                |
            //               32
        }

        [Test]
        public void Parse_MINFunction()
        {
            // Arrange
            // Input formula: MIN($2, 7)
            var min = new Token(TokenType.MIN, "MIN");
            var leftPar = new Token(TokenType.LeftParenthesis, "(");
            var signal2 = new Token(TokenType.Variable, "$2");
            var comma = new Token(TokenType.Comma, ",");
            var seven = new Token(TokenType.Number, "7");
            var rightPar = new Token(TokenType.RightParenthesis, ")");

            ITokenizer tokenizer = Substitute.For<ITokenizer>();
            tokenizer.NextToken().Returns(min, leftPar, signal2, comma, seven, rightPar);
            ASTParser astParser = new ASTParser(tokenizer);

            // Act
            var rootNode = astParser.Parse();

            // Assert
            Assert.True(rootNode.Token.Equals(min));
            Assert.True(rootNode.Operands[0].Token.Equals(signal2));
            Assert.True(rootNode.Operands[1].Token.Equals(seven));
            Assert.True(rootNode.ChildNodes == 2);
            Assert.True(rootNode.Operands[0].ChildNodes == 0);
            Assert.True(rootNode.Operands[1].ChildNodes == 0);

            // result tree should look like this
            //           MIN
            //          /  \
            //        $2    7
        }

        [Test]
        public void Parse_OperationWithFunction()
        {
            // Arrange
            // Input formula: 5 - ABS(10)
            var five = new Token(TokenType.Number, "5");
            var minus = new Token(TokenType.Minus, "-");
            var abs = new Token(TokenType.ABS, "ABS");
            var leftPar = new Token(TokenType.LeftParenthesis, "(");
            var ten = new Token(TokenType.Number, "10");
            var rightPar = new Token(TokenType.RightParenthesis, ")");

            ITokenizer tokenizer = Substitute.For<ITokenizer>();
            tokenizer.NextToken().Returns(five, minus, abs, leftPar, ten, rightPar);
            ASTParser astParser = new ASTParser(tokenizer);

            // Act
            var rootNode = astParser.Parse();

            // Assert
            Assert.True(rootNode.Token.Equals(minus));
            Assert.True(rootNode.Operands[0].Token.Equals(five));
            Assert.True(rootNode.Operands[1].Token.Equals(abs));
            Assert.True(rootNode.Operands[1].Operands[0].Token.Equals(ten));
            Assert.True(rootNode.ChildNodes == 2);
            Assert.True(rootNode.Operands[0].ChildNodes == 0);
            Assert.True(rootNode.Operands[1].ChildNodes == 1);
            Assert.True(rootNode.Operands[1].Operands[0].ChildNodes == 0);

            // result tree should look like this
            //          Minus
            //          /  \
            //         5   ABS
            //              |
            //             10
        }

        // (-11-sqrt((11*11)-4*6*-35))/(2*6)

        [Test]
        public void Parse_QuadraticEquation()
        {
            // Arrange
            // The quadratic formula: 6x^2 + 11x - 35 = 0
            // Input formula: (-11-sqrt((11*11)-4*6*-35))/(2*6)

            var leftPar = new Token(TokenType.LeftParenthesis, "(");
            var minus = new Token(TokenType.Minus, "-");
            var eleven = new Token(TokenType.Number, "11");
            var sqrt = new Token(TokenType.SQRT, "SQRT");
            var multiplication = new Token(TokenType.Multiplication, "*");
            var rightPar = new Token(TokenType.RightParenthesis, ")");
            var four = new Token(TokenType.Number, "4");
            var six = new Token(TokenType.Number, "6");
            var thirtyFive = new Token(TokenType.Number, "35");
            var division = new Token(TokenType.Division, "/");
            var two = new Token(TokenType.Number, "2");

            ITokenizer tokenizer = Substitute.For<ITokenizer>();
            tokenizer.NextToken().Returns(leftPar, minus, eleven, minus, sqrt, leftPar, leftPar, eleven,
                multiplication, eleven, rightPar, minus, four, multiplication, six, multiplication, minus,
                thirtyFive, rightPar, rightPar, division, leftPar, two, multiplication, six, rightPar);

            ASTParser astParser = new ASTParser(tokenizer);

            // Act
            var rootNode = astParser.Parse();

            // Assert
            Assert.True(rootNode.Token.Equals(division));
            Assert.True(rootNode.Operands[0].Token.Equals(minus));
            Assert.True(rootNode.Operands[1].Token.Equals(multiplication));
            Assert.True(rootNode.Operands[1].Operands[0].Token.Equals(two));
            Assert.True(rootNode.Operands[1].Operands[1].Token.Equals(six));
            Assert.True(rootNode.Operands[0].Operands[0].Token.Equals(minus));
            Assert.True(rootNode.Operands[0].Operands[1].Token.Equals(sqrt));
            Assert.True(rootNode.Operands[0].Operands[0].Operands[0].Token.Equals(eleven));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Token.Equals(minus));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Operands[0].Token.Equals(multiplication));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Operands[1].Token.Equals(multiplication));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Operands[0].Operands[0].Token.Equals(eleven));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Operands[0].Operands[1].Token.Equals(eleven));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Operands[1].Operands[0].Token.Equals(multiplication));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Operands[1].Operands[1].Token.Equals(minus));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Operands[1].Operands[0].Operands[0].Token.Equals(four));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Operands[1].Operands[0].Operands[1].Token.Equals(six));
            Assert.True(rootNode.Operands[0].Operands[1].Operands[0].Operands[1].Operands[1].Operands[0].Token.Equals(thirtyFive));

            // result tree should look like this
            //                    Division
            //                   /       \
            //              Minus         Mul
            //              /   \         /  \
            //          Minus   SQRT     2    6
            //           |        |
            //          11      Minus
            //                 /     \
            //              Mul       Mul
            //             /  \       /  \
            //           11   11    Mul   Minus
            //                     /  \     |
            //                    4    6    35
        }
    }
}