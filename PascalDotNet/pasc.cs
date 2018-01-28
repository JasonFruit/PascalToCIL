using System;
using System.IO;
using System.Collections.Generic;

namespace PascalDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = File.ReadAllText(
                "/home/jason/Code/PascalCIL/samples/sample.pas");
            
            Tokenizer t = new Tokenizer();
            List<Token> tokens = t.Tokenize(code);
            
            foreach (Token token in tokens)
            {
                Console.Write(token.Type);
                Console.Write(": ");
                Console.WriteLine(token.Text + " (" +
                                  token.Line.ToString() + ", " +
                                  token.Column.ToString() + ")");
            }
        }
    }
}
