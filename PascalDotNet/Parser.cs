using System;
using System.Collections.Generic;

namespace PascalDotNet {
    public class Parser {

        private const string Digits = "0123456789";
        private const string Letters = "abcdefghijklmnopqrstuvwxyz";

        public Boolean TryParseIdentifier(List<Token> tokens,
                                          ref int position,
                                          out ASTNode node) {

            Token token = tokens[position];

            if (token.Type == TokenTypes.Identifier) {
                node = new IdentifierNode(token.Text);
                node.CharacterNumber = token.Column;
                node.LineNumber = token.Line;
                position++;
                return true;
            }
            node = null;
            return false;
        }

        public Boolean TryParseInteger(List<Token> tokens,
                                       ref int position,
                                       out ASTNode node) {

            Token token = tokens[position];
            int tmpInt;

            if (token.Type == TokenTypes.Number && int.TryParse(token.Text, out tmpInt)) {
                IntegerNode outNode = new IntegerNode(tmpInt);
                node = outNode;
                position++;
                return true;
            }
            node = null;
            return false;
        }



        public ASTNode Parse(List<Token> tokens) {
            BlockNode block = new BlockNode();

            ASTNode curNode = null;
            int pos = 0;

            Boolean cont = true;

            while (cont) {

                if ((cont = TryParseIdentifier(tokens,
                                               ref pos,
                                               out curNode))) {
                    block.Children.Add(curNode);
                }
                else if ((cont = TryParseInteger(tokens,
                                                 ref pos,
                                                 out curNode))) {
                    block.Children.Add(curNode);
                }

                // don't continue if there are no more tokens
                cont = cont && pos < tokens.Count;

            }

            return block;
        }

        public Parser() {
        }
    }
}
