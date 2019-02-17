using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace Euclid {
    /* Construction class to handle parsing and outputting constructions based on the diagram files.
     */
    public class Construction {
        private RootStatement root;
        private string fileName;

        // Constructs a new construction from file
        public Construction(string fileName) {
            this.fileName = fileName;
            this.tokens = Tokenize(ReadFromFile());
            this.tokenLoc = 0;
            root = ConstructAbstractSyntaxTree();
            Debug.Log(root.ToString());
        }

        public List<Figure> Execute() {
            Dictionary<string, object> variableScope = new Dictionary<string, object>();
            Dictionary<string, Function> functionScope = new Dictionary<string, Function>();
            Function constructPoint = new ConstructPointFunction();
            Function constructPlane = new ConstructPlaneFunction();
            Function constructSphere = new ConstructSphereFunction();
            Function intersection = new IntersectionFunction();
            Function pointOn = new PointOnFunction();
            Function binormal = new BinormalFunction();
            Function constructLine = new ConstructLineFunction();
            Function center = new CenterFunction();
            functionScope.Add("point", constructPoint);
            functionScope.Add("plane", constructPlane);
            functionScope.Add("sphere", constructSphere);
            functionScope.Add("intersection", intersection);
            functionScope.Add("point_on", pointOn);
            functionScope.Add("binormal", binormal);
            functionScope.Add("line", constructLine);
            functionScope.Add("center", center);
            root.Run(variableScope, functionScope);
            List<Figure> render = new List<Figure>();
            foreach (string variable in variableScope.Keys) {
                if (variableScope[variable] is Figure) {
                    Figure fig = variableScope[variable] as Figure;
                    if (fig.properties.ContainsKey("render")) {
                        if (Mathf.Approximately((float) fig.properties["render"], 1)) {
                            render.Add(fig);
                        }
                    }
                }
            }
            return render;
        }

        private int tokenLoc;
        private List<Token> tokens;

        public Expression constructExpression() {
            Regex real = new Regex("-?([0-9]+|[0-9]+\\.?[0-9]*|[0-9]*\\.?[0-9]+)");
            if (tokenLoc + 1 < tokens.Count && tokens[tokenLoc + 1].token == "(") {
                string name = tokens[tokenLoc].token;
                List<Expression> args = new List<Expression>();
                tokenLoc += 2;
                while (tokens[tokenLoc].token != ")") {
                    args.Add(constructExpression());
                }
                tokenLoc++;
                return new FunctionExpression(name, args);
            }
            else if (real.Match(tokens[tokenLoc].token).Success) {
                return new RealExpression(float.Parse(tokens[tokenLoc++].token));
            }
            else {
                tokenLoc++;
                return new VariableExpression(tokens[tokenLoc - 1].token);
            }
        }

        public Statement constructStatement() {
            if (tokens[tokenLoc].token == "type_check") {
                List<string> variables = new List<string>();
                while (tokens[tokenLoc].token != "{") {
                    variables.Add(tokens[tokenLoc].token);
                }
                tokenLoc++;
                BlockStatement successBlock = constructBlockStatement();
                tokenLoc++;
                BlockStatement failBlock = constructBlockStatement();
                return new TypeCheckStatement(variables, successBlock, failBlock);
            }
            int j = tokenLoc;
            while (j < tokens.Count && tokens[j].tokenType == 0) {
                j++;
            }
            if (tokens[j].token == ".") {
                tokenLoc = j + 3;
                return new MemberAssignmentStatement(tokens[tokenLoc - 4].token, tokens[tokenLoc - 2].token, constructExpression());
            }
            if (tokens[j].token == "=") {
                if (tokens[j + 1].token == "construction") {
                    string name = tokens[j - 1].token;
                    List<string> args = new List<string>(), results = new List<String>();
                    j += 2;
                    while (tokens[j].token != "->") {
                        args.Add(tokens[j].token);
                        j++;
                    }
                    j++;
                    while (tokens[j].token != "{") {
                        results.Add(tokens[j].token);
                        j++;
                    }
                    j++;
                    tokenLoc = j;
                    return new ConstructionStatement(name, args, results, constructBlockStatement());
                }
                else {
                    List<string> variables = new List<string>();
                    while (tokens[tokenLoc].tokenType == 0) {
                        variables.Add(tokens[tokenLoc].token);
                        tokenLoc++;
                    }
                    tokenLoc++;
                    return new AssignmentStatement(variables, constructExpression());
                }
            }
            return null;
        }

        public BlockStatement constructBlockStatement() {
            List<Statement> children = new List<Statement>();
            while (tokenLoc < tokens.Count && tokens[tokenLoc].token != "}") {
                children.Add(constructStatement());
            }
            tokenLoc++;
            return new BlockStatement(children);
        }

        public RootStatement ConstructAbstractSyntaxTree() {
            return new RootStatement(constructBlockStatement());
            // RootStatement root = new RootStatement(new BlockStatement(new List<Statement>{new AssignmentStatement(new List<string>(){"a"}, new FunctionExpression("point", new List<Expression>() {
            //     new RealExpression(5.0f), new RealExpression(4.0f), new RealExpression(3.0f)
            // }))}));
            // RootStatement root = new RootStatement(new BlockStatement(new List<Statement>{
            //     new AssignmentStatement(new List<string>() {"alpha"}, new FunctionExpression("point", new List<Expression>(){ new RealExpression(3.0f), new RealExpression(4.0f), new RealExpression(5.0f) })),
            //     new AssignmentStatement(new List<string>() {"beta"}, new FunctionExpression("point", new List<Expression>(){ new RealExpression(6.0f), new RealExpression(4.0f), new RealExpression(5.0f) })),
            //     new AssignmentStatement(new List<string>() {"gamma"}, new FunctionExpression("point", new List<Expression>(){ new RealExpression(6.0f), new RealExpression(2.0f), new RealExpression(7.0f) })),
            //     new ConstructionStatement("construct_test", new List<string>() {"a", "b", "c"}, new List<string>() {"p"}, new BlockStatement(new List<Statement>() {
            //         new AssignmentStatement(new List<string>() {"p"}, new FunctionExpression("plane", new List<Expression>(){
            //             new VariableExpression("a"),
            //             new VariableExpression("b"),
            //             new VariableExpression("c")
            //         }))
            //     })),
            //     new AssignmentStatement(new List<string>() {"delta"}, new FunctionExpression("construct_test", new List<Expression>() { new VariableExpression("alpha"), new VariableExpression("beta"), new VariableExpression("gamma")})),
            //     new MemberAssignmentStatement("delta", "render", new RealExpression(1.0f)),
            //     new MemberAssignmentStatement("alpha", "render", new RealExpression(1.0f)),
            //     new MemberAssignmentStatement("beta", "render", new RealExpression(.0f)),
            //     new MemberAssignmentStatement("gamma", "render", new RealExpression(1.0f))
            // }));
            // return root;
        }

        public List<string> ReadFromFile() {
            StreamReader reader = File.OpenText(fileName);
            List<string> result = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null) {
                result.Add(line + "\n");
            }
            return result;
        }

        public List<Token> Tokenize(List<string> rawConstruction) {
            List<Regex> tokenRegexes = new List<Regex>();
            tokenRegexes.Add(new Regex("[A-Za-z_0-9]"));
            tokenRegexes.Add(new Regex("[{]"));
            tokenRegexes.Add(new Regex("[}]"));
            tokenRegexes.Add(new Regex("[(]"));
            tokenRegexes.Add(new Regex("[)]"));
            tokenRegexes.Add(new Regex("[\\->=.]"));
            List<Token> tokens = new List<Token>();
            string currentToken = "";
            int lastTokenType = -1, currentCharNum = 0, currentLineNum = 0;
            for (int lineNum = 0; lineNum < rawConstruction.Count; lineNum++) {
                string lineText = rawConstruction[lineNum];
                for (int charNum = 0; charNum < lineText.Length; charNum++) {
                    string lineChar = lineText.Substring(charNum, 1);
                    int currentTokenType = -1;
                    for (int j = 0; j < tokenRegexes.Count; j++) {
                        if (tokenRegexes[j].Match(lineChar).Success) {
                            currentTokenType = j;
                            break;
                        }
                    }
                    if (currentTokenType == -1 || (currentTokenType != lastTokenType && lastTokenType != -1)) {
                        if (currentToken.Length > 0) {
                            tokens.Add(new Token(currentToken, currentLineNum, currentCharNum, lastTokenType));
                        }
                        currentToken = "";
                        lastTokenType = -1;
                    }
                    if (currentTokenType != -1) {
                        lastTokenType = currentTokenType;
                        if (currentToken.Length > 0) {
                            currentLineNum = lineNum;
                            currentCharNum = charNum;
                        }
                        currentToken += lineChar;
                    }
                }
            }
            if (currentToken.Length > 0)
                tokens.Add(new Token(currentToken, currentLineNum, currentCharNum, lastTokenType));
            return tokens;
        }

        public class Token {
            public string token { get; }
            public int lineNum { get; }
            public int charNum { get; }
            public int tokenType { get; }

            public Token(string token, int lineNum, int charNum, int tokenType) {
                this.token = token;
                this.lineNum = lineNum;
                this.charNum = charNum;
                this.tokenType = tokenType;
            }

            public string Tostring() {
                return token;
            }
        }

        public abstract class Function : SyntaxNode {
            public abstract List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope);
        }

        public class EuclidFunction : Function {
            public List<string> arguments { get; }
            public List<string> results { get; }
            public BlockStatement block { get; }
            public EuclidFunction(List<string> arguments, List<string> results, BlockStatement block) {
                this.arguments = arguments;
                this.results = results;
                this.block = block;
            }
            public override List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                for (int i = 0; i < arguments.Count; i++) {
                    variableScope[arguments[i]] = args[i];
                }
                block.Run(variableScope, functionScope);
                List<object> res = new List<object>();
                for (int i = 0; i < results.Count; i++) {
                    res.Add(variableScope[results[i]]);
                }
                return res;
            }
        }

        public class ConstructPointFunction : Function {
            public override List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                return new List<object>() { Figure.ConstructPoint((float) args[0], (float) args[1], (float) args[2]) };
            }
        }

        public class ConstructPlaneFunction : Function {
            public override List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                return new List<object>() { Figure.ConstructPlane(args[0] as Figure, args[1] as Figure, args[2] as Figure) };
            }
        }

        public class ConstructSphereFunction : Function {
            public override List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                return new List<object> { Figure.ConstructSphere(args[0] as Figure, args[1] as Figure) };
            }
        }

        public class IntersectionFunction : Function {
            public override List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                List<Figure> res = Figure.Intersection(args[0] as Figure, args[1] as Figure);
                List<object> obj = new List<object>();
                foreach (Figure fig in res)
                    obj.Add(fig);
                return obj;
            }
        }

        public class PointOnFunction : Function {
            public override List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                return new List<object> { Figure.PointOn(args[0] as Figure) };
            }
        }

        public class BinormalFunction : Function {
            public override List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                return new List<object> { Figure.Binormal(args[0] as Figure, args[1] as Figure) };
            }
        }

        public class ConstructLineFunction : Function {
            public override List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                return new List<object> { Figure.ConstructLine(args[0] as Figure, args[1] as Figure) };
            }
        }

        public class CenterFunction : Function {
            public override List<object> Run(List<object> args, Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                return new List<object> { Figure.Center(args[0] as Figure) };
            }
        }

        public abstract class SyntaxNode {
        }

        public abstract class Expression : SyntaxNode {
            public abstract object Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope);
        }

        public abstract class Statement : SyntaxNode {
            public abstract void Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope);
        }

        public class RootStatement : Statement {
            public BlockStatement block { get; }
            public RootStatement(BlockStatement block) {
                this.block = block;
            }
            public override void Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                block.Run(variableScope, functionScope);
            }
            public override string ToString() {
                return block.ToString();
            }
        }

        public class BlockStatement : Statement {
            public List<Statement> children { get; }
            public BlockStatement(List<Statement> children) {
                this.children = children;
            }
            public override void Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                foreach (Statement node in children) {
                    node.Run(variableScope, functionScope);
                }
            }
            public override string ToString() {
                string res = "BlockStatement<\n";
                foreach (Statement s in children)
                    res += s.ToString();
                res += "\n";
                return res;
            }
        }

        public class AssignmentStatement : Statement {
            public List<string> variables { get; }
            public Expression expression { get; }
            public AssignmentStatement(List<string> variables, Expression expression) {
                this.variables = variables;
                this.expression = expression;
            }
            public override void Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                object expressionResult = expression.Run(variableScope, functionScope);
                if (expressionResult is List<object>) {
                    List<object> rightVals = expressionResult as List<object>;
                    for (int i = 0; i < variables.Count; i++) {
                        variableScope.Add(variables[i], rightVals[i]);
                    }
                }
                else {
                    variableScope.Add(variables[0], expression.Run(variableScope, functionScope));
                }
            }
            public override String ToString() {
                string res = String.Format("AssignmentStatement<");
                foreach (string s in variables) {
                    res += s + " ";
                }
                res += expression.ToString();
                res += ">";
                return res;
            }
        }

        public class MemberAssignmentStatement : Statement {
            public string variable { get; }
            public string member { get; }
            public Expression expression { get; }
            public MemberAssignmentStatement(string variable, string member, Expression expression) {
                this.variable = variable;
                this.member = member;
                this.expression = expression;
            }
            public override void Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                object var = variableScope[variable];
                if (var is Figure) {
                    Figure fig = var as Figure;
                    fig.properties[member] = expression.Run(variableScope, functionScope);
                }
            }
            public override String ToString() {
                return String.Format("MemberAssignmentStatement<{0} {1} {2}>", variable, member, expression.ToString());
            }
        }

        public class TypeCheckStatement : Statement {
            public List<string> variables { get; }
            public BlockStatement successBlock { get; }
            public BlockStatement failBlock { get; }
            public TypeCheckStatement(List<string> variables, BlockStatement successBlock, BlockStatement failBlock) {
                this.variables = variables;
                this.successBlock = successBlock;
                this.failBlock = failBlock;
            }
            public override void Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                bool matchTypes = true;
                for (int i = 1; i < variables.Count; i++) {
                    if (!variables[i].GetType().Equals(variables[0].GetType())) {
                        matchTypes = false;
                    }
                }
                if (matchTypes)
                    successBlock.Run(variableScope, functionScope);
                else
                    failBlock.Run(variableScope, functionScope);
            }
        }

        public class ConstructionStatement : Statement {
            public string name { get; }
            public List<string> arguments { get; }
            public List<string> results { get; }
            public BlockStatement block { get; }
            public ConstructionStatement(string name, List<string> arguments, List<string> results, BlockStatement block) {
                this.name = name;
                this.arguments = arguments;
                this.results = results;
                this.block = block;
            }
            public override void Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                functionScope[name] = new EuclidFunction(arguments, results, block);
            }
            public override string ToString() {
                string res = String.Format("ConstructionStatement<{0}\n", name);
                foreach (string s in arguments) {
                    res += s + " ";
                }
                res += "\n";
                foreach (string s in results) {
                    res += s + " ";
                }
                res += "\n";
                res += block.ToString();
                return res;
            }
        }

        public class FunctionExpression : Expression {
            public string name { get; }
            public List<Expression> arguments { get; }
            public FunctionExpression(string name, List<Expression> arguments) {
                this.name = name;
                this.arguments = arguments;
            }
            public override object Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                Function func = functionScope[name];
                List<object> args = new List<object>();
                foreach (Expression e in arguments) {
                    args.Add(e.Run(variableScope, functionScope));
                }
                return func.Run(args, new Dictionary<string, object>(), functionScope);
            }
            public override string ToString() {
                string res = String.Format("FunctionExpression<{0}\n", name);
                foreach (Expression e in arguments)
                    res += e.ToString();
                res += ">";
                return res;
            }
        }

        public class RealExpression : Expression {
            public Single real { get;  }
            public RealExpression(float real) {
                this.real = real;
            }
            public override object Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                return this.real;
            }
            public override string ToString() {
                return String.Format("RealExpression<{0}>", real);
            }
        }

        public class VariableExpression : Expression { 
            public string name { get; }
            public VariableExpression(string name) {
                this.name = name;
            }
            public override object Run(Dictionary<string, object> variableScope, Dictionary<string, Function> functionScope) {
                return variableScope[name];
            }
            public override string ToString() {
                return String.Format("VariableExpression<{0}>", name);
            }
        }

    }
}
