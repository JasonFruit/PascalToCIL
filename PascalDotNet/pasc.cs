using System;
using System.IO;
using System.Collections.Generic;

namespace PascalDotNet {
    class Program {
        static void Main(string[] args) {
            string code = File.ReadAllText(
                "/home/jason/Code/PascalCIL/samples/idents-and-ints.pas");

            Tokenizer t = new Tokenizer();
            List<Token> tokens = t.Tokenize(code);

            Parser parser = new Parser();

            BlockNode block = parser.Parse(tokens) as BlockNode;

            foreach (ASTNode child in block.Children) {
                if (child.Type == NodeTypes.Identifier) {
                    Console.WriteLine((child as IdentifierNode).Name);
                }
                else if (child.Type == NodeTypes.Integer) {
                    Console.WriteLine((child as IntegerNode).Value);
                }
            }


            // foreach (Token token in tokens)
            // {
            //     Console.Write(token.Type);
            //     Console.Write(": ");
            //     Console.WriteLine(token.Text + " (" +
            //                       token.Line.ToString() + ", " +
            //                       token.Column.ToString() + ")");
            // }

        }
    }
}
