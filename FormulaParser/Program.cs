using System;
using System.Diagnostics;
using FormulaParser.Interpreter;
using FormulaParser.Parser;
using FormulaParser.Parser.SyntaxTree;

namespace FormulaParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SignalCollection signalCollection = new SignalCollection();
            SignalCollection.AddRandomSignalToCollection("real", 200000);

            Stopwatch sw = new Stopwatch();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===========================");
                Console.WriteLine("Available signals:");
                Console.WriteLine(string.Join(", ", signalCollection.SignalNames));
                Console.WriteLine("===========================");
                Console.ResetColor();
                Console.WriteLine("Input formula: ");
                // example:  (4 - $2 / "Sinus")+ ( 2 + 5 * -3 / sqrt($4) )
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                string inputStr = Console.ReadLine();
                Console.ResetColor();

                if (string.IsNullOrWhiteSpace(inputStr)) continue;

                sw.Restart();
                Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(inputStr, signalCollection);

                ASTParser astParser = new ASTParser(tokenizer);
                ASTNode rootNode = astParser.Parse();

                long elapsed = sw.ElapsedMilliseconds;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"The formula is tokenized and parsed in {{ {elapsed} }} milliseconds.");
                Console.ResetColor();

                ASTInterpreter interpreter = new ASTInterpreter();
                dynamic result = interpreter.InterpretAbstractSyntaxTree(rootNode);

                Console.WriteLine($"Result type: {result?.GetType()}");
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Result: {result}");
                Console.ResetColor();

                if (result is Signal)
                {
                    SignalCollection.AddResultSignalToCollection(result);
                }
            }
        }
    }
}