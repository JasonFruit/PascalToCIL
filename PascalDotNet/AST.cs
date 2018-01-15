using System;
using System.Collections.Generic;

namespace PascalDotNet {

    public enum NodeTypes {
        Program,
        Var,
        Vars,
        Const,
        Unit,
        Block,
        Conditional,
        IfStatement,
        WhileStatement,
        ForStatement,
        Parameter,
        Function,
        Procedure,
        Argument,
        Call,
        Operator,
        Expression,
        Identifier,
        Integer,
        String
    };

    public abstract class ASTNode {
        public int LineNumber;
        public int CharacterNumber;
        public string Text;
        public NodeTypes Type;
    }

    public class VarNode: ASTNode {
        public string Name;
        public IdentifierNode TypeNode;
        public ASTNode ValueExpression;

        public VarNode(string name, IdentifierNode typ, ASTNode value) {
            Type = NodeTypes.Var;
            Name = name;
            TypeNode = typ;
            ValueExpression = value;
        }

        public VarNode(string name, IdentifierNode typ) {
            Type = NodeTypes.Var;
            Name = name;
            TypeNode = typ;
        }
    }

    public class VarSectionNode: ASTNode {
        public List<VarNode> Vars;

        public VarSectionNode() {
            Type = NodeTypes.Vars;
        }
    }

    public class IntegerNode: ASTNode {
        public int Value;

        public IntegerNode(int value) {
            Type = NodeTypes.Integer;
            Value = value;
        }
    }

    public class StringNode: ASTNode {
        public string Value;

        public StringNode(string value) {
            Type = NodeTypes.String;
            Value = value;
        }
    }

    public class OperatorNode: ASTNode {
        public string Operator;

        public OperatorNode(string theOperator) {
            Type = NodeTypes.Operator;
            Operator = theOperator;
        }
    }

    public class IdentifierNode: ASTNode {
        public string Name;

        public IdentifierNode(string name) {
            Type = NodeTypes.Identifier;
            Name = name;
        }
    }

    public abstract class RoutineNode: ASTNode {
        public IdentifierNode Name;
        public List<ParameterNode> Parameters = new List<ParameterNode>();
        public List<VarNode> Vars = new List<VarNode>();
        public BlockNode Body = new BlockNode();
    }

    public class ParameterNode: ASTNode {
        public IdentifierNode Identifier;
        public IdentifierNode TypeIdentifier;
        public Boolean IsVariable = false;
        public BlockNode Body;

        public ParameterNode(IdentifierNode ident,
                             IdentifierNode typ) {
            Type = NodeTypes.Parameter;
            Identifier = ident;
            TypeIdentifier = typ;
            Body = new BlockNode();
        }
    }

    public class FunctionNode: RoutineNode {
        public IdentifierNode ReturnTypeIdentifier;

        public FunctionNode(IdentifierNode name,
                            IdentifierNode returnType) {
            Type = NodeTypes.Function;
            Name = name;
            ReturnTypeIdentifier = returnType;
        }
    }

    public class ProcedureNode: RoutineNode {

        public ProcedureNode(IdentifierNode name,
                            IdentifierNode returnType) {
            Type = NodeTypes.Function;
            Name = name;
        }
    }

    public class ArgumentNode: ASTNode {
        public IdentifierNode Identifier;
        public IdentifierNode TypeIdentifier;
        public Boolean IsVariable;
        public ASTNode ValueExpression;

        public ArgumentNode(IdentifierNode ident,
                            IdentifierNode typ,
                            Boolean var,
                            ASTNode val) {
            Type = NodeTypes.Argument;
            Identifier = ident;
            TypeIdentifier = typ;
            IsVariable = var;
            ValueExpression = val;
        }
    }

    public class CallNode: ASTNode {
        public IdentifierNode RoutineIdentifier;
        public List<ASTNode> Args;

        public CallNode(IdentifierNode identifier,
                        List<ASTNode> args) {
            Type = NodeTypes.Call;
            RoutineIdentifier = identifier;
            Args = args;
        }

        public CallNode(IdentifierNode identifier) {
            Type = NodeTypes.Call;
            RoutineIdentifier = identifier;
            Args = new List<ASTNode>();
        }
    }

    public class BlockNode: ASTNode {
        public List<ASTNode> Children;

        public BlockNode() {
            Type = NodeTypes.Block;
            Children = new List<ASTNode>();
        }
    }

    public class ProgramNode: ASTNode {
        public string Name;
        public VarSectionNode Vars = new VarSectionNode();
        public BlockNode Body = new BlockNode();

        public ProgramNode(string name) {
            Type = NodeTypes.Program;
        }
    }

    public class ConditionBlockNode: ASTNode {
        public ASTNode Condition;
        public BlockNode Block;

        public ConditionBlockNode(ASTNode condition,
                                  BlockNode block) {
            Type = NodeTypes.Conditional;
            Condition = condition;
            Block = block;
        }

    }

    public class IfNode: ASTNode {
        public List<ConditionBlockNode> ConditionBlocks;

        public IfNode() {
            Type = NodeTypes.IfStatement;
            ConditionBlocks = new List<ConditionBlockNode>();
        }
    }

    public class WhileNode: ASTNode {
        public ConditionBlockNode ConditionBlock;

        public WhileNode(ConditionBlockNode block) {
            Type = NodeTypes.WhileStatement;
            ConditionBlock = block;
        }
    }

    public class ForNode: ASTNode {
        IdentifierNode LoopVar;
        ASTNode InitExpression;
        ASTNode TerminateExpression;
        Boolean DownTo = false;
        BlockNode Body = new BlockNode();

        public ForNode(IdentifierNode var,
                       ASTNode init,
                       ASTNode term) {
            Type = NodeTypes.ForStatement;
            LoopVar = var;
            InitExpression = init;
            TerminateExpression = term;
        }
    }

}