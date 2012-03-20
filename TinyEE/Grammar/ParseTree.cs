// Generated by TinyPG v1.3 available at www.codeproject.com

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TinyEE
{
    #region ParseTree
    [Serializable]
    public class ParseErrors : List<ParseError>
    {
    }

    [Serializable]
    public class ParseError
    {
        private string message;
        private int code;
        private int line;
        private int col;
        private int pos;
        private int length;

        public int Code { get { return code; } }
        public int Line { get { return line; } }
        public int Column { get { return col; } }
        public int Position { get { return pos; } }
        public int Length { get { return length; } }
        public string Message { get { return message; } }

        // just for the sake of serialization
        public ParseError()
        {
        }

        public ParseError(string message, int code, ParseNode node) : this(message, code,  0, node.Token.StartPos, node.Token.StartPos, node.Token.Length)
        {
        }

        public ParseError(string message, int code, int line, int col, int pos, int length)
        {
            this.message = message;
            this.code = code;
            this.line = line;
            this.col = col;
            this.pos = pos;
            this.length = length;
        }
    }

    // rootlevel of the node tree
    [Serializable]
    public partial class ParseTree : ParseNode
    {
        public ParseErrors Errors;

        public List<Token> Skipped;

        public ParseTree() : base(new Token(), "ParseTree")
        {
            Token.Type = TokenType.Start;
            Token.Text = "Root";
            Errors = new ParseErrors();
        }

        public string PrintTree()
        {
            StringBuilder sb = new StringBuilder();
            int indent = 0;
            PrintNode(sb, this, indent);
            return sb.ToString();
        }

        private void PrintNode(StringBuilder sb, ParseNode node, int indent)
        {
            
            string space = "".PadLeft(indent, ' ');

            sb.Append(space);
            sb.AppendLine(node.Text);

            foreach (ParseNode n in node.Nodes)
                PrintNode(sb, n, indent + 2);
        }
        
        /// <summary>
        /// this is the entry point for executing and evaluating the parse tree.
        /// </summary>
        /// <param name="paramlist">additional optional input parameters</param>
        /// <returns>the output of the evaluation function</returns>
        public object Eval(params object[] paramlist)
        {
            return Nodes[0].Eval(this, paramlist);
        }
    }

    [Serializable]
    [XmlInclude(typeof(ParseTree))]
    public partial class ParseNode
    {
        protected string text;
        protected List<ParseNode> nodes;
        
        public List<ParseNode> Nodes { get {return nodes;} }
        
        [XmlIgnore] // avoid circular references when serializing
        public ParseNode Parent;
        public Token Token; // the token/rule

        [XmlIgnore] // skip redundant text (is part of Token)
        public string Text { // text to display in parse tree 
            get { return text;} 
            set { text = value; }
        } 

        public virtual ParseNode CreateNode(Token token, string text)
        {
            ParseNode node = new ParseNode(token, text);
            node.Parent = this;
            return node;
        }

        protected ParseNode(Token token, string text)
        {
            this.Token = token;
            this.text = text;
            this.nodes = new List<ParseNode>();
        }

        protected object GetValue(ParseTree tree, TokenType type, int index)
        {
            return GetValue(tree, type, ref index);
        }

        protected object GetValue(ParseTree tree, TokenType type, ref int index)
        {
            object o = null;
            if (index < 0) return o;

            // left to right
            foreach (ParseNode node in nodes)
            {
                if (node.Token.Type == type)
                {
                    index--;
                    if (index < 0)
                    {
                        o = node.Eval(tree);
                        break;
                    }
                }
            }
            return o;
        }

        /// <summary>
        /// this implements the evaluation functionality, cannot be used directly
        /// </summary>
        /// <param name="tree">the parsetree itself</param>
        /// <param name="paramlist">optional input parameters</param>
        /// <returns>a partial result of the evaluation</returns>
        internal object Eval(ParseTree tree, params object[] paramlist)
        {
            object Value = null;

            switch (Token.Type)
            {
                case TokenType.Start:
                    Value = EvalStart(tree, paramlist);
                    break;
                case TokenType.Expression:
                    Value = EvalExpression(tree, paramlist);
                    break;
                case TokenType.OrExpression:
                    Value = EvalOrExpression(tree, paramlist);
                    break;
                case TokenType.AndExpression:
                    Value = EvalAndExpression(tree, paramlist);
                    break;
                case TokenType.NotExpression:
                    Value = EvalNotExpression(tree, paramlist);
                    break;
                case TokenType.Compare:
                    Value = EvalCompare(tree, paramlist);
                    break;
                case TokenType.Addition:
                    Value = EvalAddition(tree, paramlist);
                    break;
                case TokenType.Multiplication:
                    Value = EvalMultiplication(tree, paramlist);
                    break;
                case TokenType.Power:
                    Value = EvalPower(tree, paramlist);
                    break;
                case TokenType.Negation:
                    Value = EvalNegation(tree, paramlist);
                    break;
                case TokenType.MethodCall:
                    Value = EvalMethodCall(tree, paramlist);
                    break;
                case TokenType.Member:
                    Value = EvalMember(tree, paramlist);
                    break;
                case TokenType.MemberAccess:
                    Value = EvalMemberAccess(tree, paramlist);
                    break;
                case TokenType.Base:
                    Value = EvalBase(tree, paramlist);
                    break;
                case TokenType.Variable:
                    Value = EvalVariable(tree, paramlist);
                    break;
                case TokenType.IndexAccess:
                    Value = EvalIndexAccess(tree, paramlist);
                    break;
                case TokenType.FunctionCall:
                    Value = EvalFunctionCall(tree, paramlist);
                    break;
                case TokenType.ArgumentList:
                    Value = EvalArgumentList(tree, paramlist);
                    break;
                case TokenType.Literal:
                    Value = EvalLiteral(tree, paramlist);
                    break;
                case TokenType.Group:
                    Value = EvalGroup(tree, paramlist);
                    break;

                default:
                    Value = Token.Text;
                    break;
            }
            return Value;
        }

        protected virtual object EvalStart(ParseTree tree, params object[] paramlist)
        {
            return "Could not interpret input; no semantics implemented.";
        }

        protected virtual object EvalExpression(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalOrExpression(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalAndExpression(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalNotExpression(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalCompare(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalAddition(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalMultiplication(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalPower(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalNegation(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalMethodCall(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalMember(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalMemberAccess(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalBase(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalVariable(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalIndexAccess(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalFunctionCall(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalArgumentList(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalLiteral(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }

        protected virtual object EvalGroup(ParseTree tree, params object[] paramlist)
        {
            throw new NotImplementedException();
        }


    }
    
    #endregion ParseTree
}
