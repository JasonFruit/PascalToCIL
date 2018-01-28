using System;
using System.Collections.Generic;

namespace PascalDotNet {
    public class Parser {

        private Boolean TryParseBlock(List<Token> tokens,
                                      out int position,
                                      out ASTNode node) {
            position = 0;
            node = null;
            return false;
        }
        
        private Boolean TryParseProgram(List<Token> tokens,
                                        ref int position,
                                        ref ASTNode node) {

            int nextPos = position;
            
            if (tokens[nextPos].Text == "program") {
                nextPos++;
                node = new ProgramNode(tokens[nextPos].Text);

                nextPos++;
                
                //TODO: for ISO Pascal, more stuff can be here
                
                if (tokens[nextPos].Text != ";") {
                    return false;
                }

                nextPos++;

                
                
                position = nextPos;
                return true;
            }

            return false;

        }
        
        public ASTNode Parse(List<Token> tokens) {
            // TODO: Make this a real parser
            return new ProgramNode("wat");
        }

        public Parser() {
        }
    }
}
