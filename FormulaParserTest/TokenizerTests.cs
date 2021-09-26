using System.Collections.Generic;
using FormulaParser;
using FormulaParser.Tokenizer;
using NSubstitute;
using NUnit.Framework;

namespace FormulaParserTest
{
    [TestFixture]
    internal class TokenizerTests
    {
        [Test]
        public void GetAllTokens_UppercaseInput()
        {
            // Arrange
            string input = @"abs ABS abS sQRt mAx - --";
            Tokenizer tokenizer = new Tokenizer(input);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.ABS, "ABS"),
                new Token(TokenType.ABS, "ABS"),
                new Token(TokenType.ABS, "ABS"),
                new Token(TokenType.SQRT, "SQRT"),
                new Token(TokenType.MAX, "MAX"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Minus, "-"),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        [Test]
        public void GetAllTokens_NoWhitespace()
        {
            // Arrange
            string input = @"2+5";
            Tokenizer tokenizer = new Tokenizer(input);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Number, "5"),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        [Test]
        public void GetAllTokens_Whitespace()
        {
            // Arrange
            string input = @"  2 +     5";
            Tokenizer tokenizer = new Tokenizer(input);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Number, "5"),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        [Test]
        public void GetAllTokens_Add_DoubleOperator()
        {
            // Arrange
            string input = @"  2 ++ 5";
            Tokenizer tokenizer = new Tokenizer(input);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Number, "5"),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        [Test]
        public void GetAllTokens_Subtract_DecimalPointNumbers()
        {
            // Arrange
            string input = @"  2.46 - 5.001";
            Tokenizer tokenizer = new Tokenizer(input);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.Number, "2.46"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Number, "5.001"),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        [Test]
        public void GetAllTokens_RandomTokens()
        {
            // Arrange
            string input = @"2 - 4 * 10.3 MiN ( ))) ( ,, $32 2 100     /  MIN   ";
            Tokenizer tokenizer = new Tokenizer(input);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Multiplication, "*"),
                new Token(TokenType.Number, "10.3"),
                new Token(TokenType.MIN, "MIN"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Variable, "$32"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "100"),
                new Token(TokenType.Division, "/"),
                new Token(TokenType.MIN, "MIN"),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        [Test]
        public void GetAllTokens_SignsAsOperators()
        {
            // Arrange
            string input = @"+1-2-33--4---5+6+-7+-+-8";
            Tokenizer tokenizer = new Tokenizer(input);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Number, "33"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Number, "7"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Number, "8"),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        [Test]
        public void GetAllTokens_InvalidCharactersError()
        {
            // Arrange
            string input = @"5///.._á(())";
            Tokenizer tokenizer = new Tokenizer(input);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Division, "/"),
                new Token(TokenType.Division, "/"),
                new Token(TokenType.Division, "/"),
                new Token(TokenType.Dot, "."),
                new Token(TokenType.Dot, "."),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            // It should parse each token up until an invalid character comes.
            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        [Test]
        public void GetAllTokens_WhiteSpaceInput()
        {
            // Arrange
            string input = @"      ";
            Tokenizer tokenizer = new Tokenizer(input);
            IList<Token> expectedTokenList = new List<Token>();

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        [Test]
        public void GetAllTokens_SignalNameProvider_OneRecognized()
        {
            // Arrange
            ISignalNameProvider nameProviderMock = Substitute.For<ISignalNameProvider>();
            nameProviderMock.SignalNames.Returns(new[]
            {
                "A4 + 3 byte"
            });

            string input = @"5-A4 + 3 byte*2";
            Tokenizer tokenizer = new Tokenizer(input, nameProviderMock);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Minus, "-"),
                new Token(TokenType.Variable, "A4 + 3 byte"),
                new Token(TokenType.Multiplication, "*"),
                new Token(TokenType.Number, "2"),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }

        //"Data_block_1"."qwe')asd+"[0]
        //"/faxc'\(sda0)-)+"
        [Test]
        public void GetAllTokens_SignalNameProvider_QuotationMark()
        {
            // Arrange
            ISignalNameProvider nameProviderMock = Substitute.For<ISignalNameProvider>();
            nameProviderMock.SignalNames.Returns(new[]
            {
                "Data_block_1",
                "qwe')asd+",
            });

            string input = @"""Data_block_1"".""qwe')asd+""[0]";                //"Data_block_1"."qwe')asd+"[0]
            Tokenizer tokenizer = new Tokenizer(input, nameProviderMock);
            IList<Token> expectedTokenList = new List<Token>
            {
                new Token(TokenType.Variable, @"""Data_block_1"""),
                new Token(TokenType.Dot, "."),
                new Token(TokenType.Variable, @"""qwe')asd+"""),
                new Token(TokenType.LeftBracket, "["),
                new Token(TokenType.Number, "0"),
                new Token(TokenType.RightBracket, "]"),
            };

            // Act
            IList<Token> tokenList = tokenizer.GetAllTokens();

            // Assert
            Assert.True(tokenList.Count == expectedTokenList.Count);

            for (int i = 0; i < tokenList.Count; i++)
            {
                Assert.AreEqual(tokenList[i], expectedTokenList[i]);
            }
        }
    }
}