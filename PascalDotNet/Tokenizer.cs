using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PascalDotNet
{

    public enum TokenTypes { Symbol, WordSymbol, Identifier, Number, String, Comment, Unknown };

    public class TokenizeError : Exception
    {
        public TokenizeError(string msg) : base(msg)
        {

        }
    }

    public class Token
    {
        public string Text = "";
        public int Line = 0;
        public int Column = 0;
        public TokenTypes Type = TokenTypes.Unknown;

        public Token()
        {

        }

        public Token(int line, int col)
        {
            Line = line;
            Column = col;
        }
    }

    public class Tokenizer
    {
        private const string whiteSpace = " \t\n";
        private const string alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string digits = "0123456789";

        private string[] symbols = new string[] {"+", "-", "*", "/", "=", "<", ">", "[", "]",
            ".", ",", ":", ";", "^", "(", ")", "<>", "<=", ">=", ":=", ".."};

        private string[] wordSymbols = new string[] {"and", "array", "begin", "case", "const", "div",
            "do", "downto", "else", "end", "file", "for",
            "function", "goto", "if", "in", "label", "mod",
            "nil", "not", "of", "or", "packed", "procedure",
            "program", "record", "repeat", "set", "then",
            "to", "type", "until", "var", "while", "with"};

        private Boolean IsSymbol(string s)
        {
            foreach (string sym in symbols)
            {
                if (s == sym)
                {
                    return true;
                }
            }
            return false;
        }

        private Boolean ExtractSymbol(ref Token token, StringCollection code, int line, ref int column)
        {
            char c = code[line][column];

            //every two-char symbol starts with a one-char symbol; thank you, Niklaus.
            if (IsSymbol(c.ToString()))
            {
                column++;
                token.Text += c;
                token.Type = TokenTypes.Symbol;

                if (column < code[line].Length)
                {
                    // if a valid two-char symbol, consume it
                    if (IsSymbol(c.ToString() + code[line][column].ToString()))
                    {
                        token.Text += code[line][column];
                        column++;
                    }
                }

                return true;
            }

            return false;
        }

        private Boolean ExtractInteger(ref Token token, StringCollection code, int line, ref int column)
        {
            char c;

            c = code[line][column];

            if (digits.Contains(c.ToString()))
            {
                token.Line = line;
                token.Column = column;
                token.Text = "";
                token.Type = TokenTypes.Number;

                while (digits.Contains(c.ToString()))
                {
                    token.Text += c;
                    column++;

                    // don't read past end of line
                    if (column < code[line].Length)
                    {
                        c = code[line][column];
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private Boolean ExtractString(ref Token token, StringCollection code, int line, ref int column)
        {
            char c = code[line][column];

            // strings start with a single-quote
            if (c == '\'')
            {
                column++;
                token.Type = TokenTypes.String;
                token.Text = "'";
                c = code[line][column];

                // continue until a single quote is encountered
                while (c != '\'')
                {
                    token.Text += c;
                    column++;

                    // Don't read past the end of the line without a close quote
                    if (column < code[line].Length)
                    {
                        c = code[line][column];
                    }
                    else
                    {
                        throw new TokenizeError("Unterminated string constant, line " + (line + 1).ToString() + ", column " + (column + 1).ToString() + ".");
                    }
                }

                // close quote
                token.Text += '\'';
            }
            else
            {
                return false;
            }

            return true;
        }

        private Boolean ExtractWordSymbol(ref Token token, StringCollection code, int line, ref int column)
        {
            Token t = new Token(line, column);
            int col = column;

            if (ExtractIdentifier(ref t, code, line, ref col))
            {
                token.Column = t.Column;
                token.Line = t.Line;
                token.Text = t.Text;
                token.Type = TokenTypes.WordSymbol;
                column = col;
            }
            else
            {
                return false;
            }
            return true;
        }

        private Boolean ExtractComment(ref Token token, StringCollection code, ref int line, ref int column)
        {
            char c = code[line][column];

            if (c == '{')
            {
                token.Text = "";
                token.Type = TokenTypes.Comment;

                while (c != '}')
                {

                    token.Text += c;
                    column++;

                    // if we pass the end of the line, start the next; comments can span multiple lines
                    if (column < code[line].Length)
                    {
                        c = code[line][column];
                    }
                    else
                    {
                        line++;
                        column = 0;
                        c = code[line][column];
                    }

                }

                token.Text += '}';
                column++;
            }
            else
            {
                return false;
            }

            return true;
        }

        private Boolean ExtractIdentifier(ref Token token, StringCollection code, int line, ref int column)
        {
            char c;

            c = code[line][column];

            // the first character must be alphabetic
            if (alpha.Contains(c.ToString()))
            {
                token.Line = line;
                token.Column = column;
                token.Text = "";
                token.Type = TokenTypes.Identifier;

                //we know the first is alpha; succeeding can be either
                while ((alpha + digits).Contains(c.ToString()))
                {
                    token.Text += c;
                    column++; // so the calling procedure knows where to start looking for next token

                    // don't read past end of line
                    if (column < code[line].Length)
                    {
                        c = code[line][column];
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;

        }

        private Boolean SkipToNonWhite(StringCollection code, ref int line, ref int column)
        {
            // skip to next line until you're no longer at the end of a line
            while (column >= code[line].Length)
            {
                line++;
                column = 0;

                if (line == code.Count)
                {
                    return false;
                }
            }

            while (whiteSpace.Contains(code[line][column].ToString()))
            {
                column++;
                if (column >= code[line].Length)
                {
                    line++;

                    //if you run out of lines, you're done
                    if (line == code.Count)
                    {
                        return false;
                    }
                    column = 0;
                }
            }

            return true;
        }

        public List<Token> Tokenize(string code)
        {
            List<Token> tokens = new List<Token>();

            StringCollection lines = new StringCollection();

            foreach (string s in code.Split('\n'))
            {
                lines.Add(s);
            }

            int line = 0;
            int column = 0;

            while (line < lines.Count)
            {
                if (!SkipToNonWhite(lines, ref line, ref column))
                {
                    return tokens;
                }

                Token token = new Token(line, column);

                // these empty blocks do their work on the reference parameters
                if (ExtractComment(ref token, lines, ref line, ref column))
                {
                }
                else if (ExtractWordSymbol(ref token, lines, line, ref column))
                {
                }
                else if (ExtractIdentifier(ref token, lines, line, ref column))
                {
                }
                else if (ExtractInteger(ref token, lines, line, ref column))
                {
                }
                else if (ExtractSymbol(ref token, lines, line, ref column))
                {
                }
                else if (ExtractString(ref token, lines, line, ref column))
                {
                }
                else
                {
                    throw new TokenizeError("Unrecognized token at line " + line.ToString() + ", column " + column.ToString() + ".");
                }

                tokens.Add(token);

            }


            return tokens;
        }

        public Tokenizer()
        {

        }
    }
}
