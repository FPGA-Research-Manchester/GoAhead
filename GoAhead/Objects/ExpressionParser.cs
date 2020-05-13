using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GoAhead.Objects
{
    public class ExpressionParser
    {
        public bool Evaluate(string expression, out int result)
        {
            // string compare
            if (Regex.IsMatch(expression, @".*\D+.*=.*\D+.*"))
            {
                string[] atoms = expression.Split('=');
                result = atoms[0].Equals(atoms[1]) ? 1 : 0;
                return true;
            }

            Stack<string> operatorStack = new Stack<string>();
            // the epxression in reverse polish notation
            Queue<string> outQueue = new Queue<string>();

            int strPos = 0;
            char c = ' ';
            char lastC = ' ';
            bool negation = false;
            while (strPos < expression.Length)
            {
                string token = "";
                while (strPos < expression.Length)
                {
                    lastC = c;
                    c = expression[strPos];
                    if (c == ' ')
                    {
                        strPos++; // consume whitespace
                        break;
                    }
                    /*
                else if ((lastC == '(' && c == '-') || (strPos == 0 && c == '-') || (this.IsOperator(new String(lastC, 1)) && c == '-'))
                {
                    negation = true;
                    strPos++;
                }*/
                    else if (c == '(' || c == ')' || IsOperator(new string(c, 1)))
                    {
                        if (string.IsNullOrEmpty(token))
                        {
                            token += c;
                            strPos++;
                        }
                        break;
                    }
                    else
                    {
                        token += c;
                        strPos++;
                    }
                }

                if (IsNumber(token))
                {
                    outQueue.Enqueue((negation ? "-" : "") + token);
                    negation = false;
                }
                else if (IsOperator(token))
                {
                    /*    If the token is an operator, o1, then:
                            while there is an operator token, o2, at the top of the stack, and
                                    either o1 is left-associative and its precedence is less than or equal to that of o2,
                                    or o1 has precedence less than that of o2,
                                pop o2 off the stack, onto the output queue;
                            push o1 onto the stack.
                    */
                    while (operatorStack.Count > 0)
                    {
                        string lastOperator = operatorStack.Peek();

                        bool isOperator = IsOperator(lastOperator);
                        bool isLeftAssociativeAndCHasLEQPrecedence = IsLeftAssociative(token) && OperatorPrecedence(token) <= OperatorPrecedence(lastOperator);
                        bool smallerPrecedence = OperatorPrecedence(token) < OperatorPrecedence(lastOperator);
                        if (isOperator && (isLeftAssociativeAndCHasLEQPrecedence || smallerPrecedence))
                        {
                            outQueue.Enqueue(operatorStack.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }
                    operatorStack.Push(token);
                }
                else if (token.Equals("("))
                {
                    operatorStack.Push(token);
                }
                else if (token.Equals(")"))
                {
                    /*
                        If the token is a right parenthesis:
                            Until the token at the top of the stack is a left parenthesis, pop operators off the stack onto the output queue.
                            Pop the left parenthesis from the stack, but not onto the output queue.
                            If the token at the top of the stack is a function token, pop it onto the output queue.
                            If the stack runs out without finding a left parenthesis, then there are mismatched parentheses.
                    */

                    while (operatorStack.Peek() != "(" && operatorStack.Count > 0)
                    {
                        outQueue.Enqueue(operatorStack.Pop());
                    }
                    if (operatorStack.Count == 0)
                    {
                        throw new ArgumentException("Missing ( in " + expression);
                    }
                    operatorStack.Pop();
                }
            }

            // 3 4 2 * 1 5 − 2 3 ^ ^ / +
            while (operatorStack.Count > 0)
            {
                outQueue.Enqueue(operatorStack.Pop());
            }

            if (outQueue.Count == 0)
            {
                result = -1;
                return false;
            }

            try
            {
                result = CalculateRPN(outQueue);
            }
            catch
            {
                result = -1;
                return false;
            }
            return true;
        }

        private int CalculateRPN(Queue<string> outQueue)
        {
            Stack<int> stack = new Stack<int>();

            while (outQueue.Count > 0)
            {
                string token = outQueue.Dequeue();

                if (IsOperator(token))
                {
                    if (stack.Count < 2)
                    {
                        throw new ArgumentException("To few operands in " + outQueue + " for operand " + token);
                    }
                    int op2 = stack.Pop();
                    int op1 = stack.Pop();
                    int intermediateResult = ApplyOperand(token, op1, op2);
                    stack.Push(intermediateResult);
                }
                else if (IsNumber(token))
                {
                    stack.Push(int.Parse(token));
                }
                else
                {
                    throw new ArgumentException("Neither number nor operand found " + token);
                }
            }

            return stack.Peek();
        }

        private int OperatorPrecedence(string c)
        {
            switch (c)
            {
                case "!":
                    return 4;
                case "*":
                case "/":
                case "%":
                case "&":
                    return 3;
                case "+":
                case "-":
                case "|":
                    return 2;
                case "=":
                    return 1;
            }
            return 0;
        }

        private bool IsLeftAssociative(string c)
        {
            switch (c)
            {
                // left to right
                case "*":
                case "/":
                case "%":
                case "+":
                case "-":
                case "&":
                case "|":
                    return true;
                // right to left
                case "=":
                case "!":
                    return false;
            }
            return false;
        }

        private bool IsOperator(string c)
        {
            return (c.Equals("+") || c.Equals("-") || c.Equals("/") || c.Equals("*") || c.Equals("!") || c.Equals("%") || c.Equals("=") || c.Equals("<") || c.Equals(">") || c.Equals("&") || c.Equals("|"));
        }

        private bool IsNumber(string c)
        {
            int i;
            return int.TryParse(c, out i);
        }

        private int ApplyOperand(string operand, int left, int right)
        {
            switch (operand)
            {
                // left to right
                case "*": return left * right;
                case "+": return left + right;
                case "-": return left - right;
                case "/": return left / right;
                case "<": return (left < right ? 1 : 0);
                case ">": return (left > right ? 1 : 0);
                case "=": return (left == right ? 1 : 0);
                case "&": return (left == 1 && right == 1 ? 1 : 0);
                case "|": return (left == 1 || right == 1 ? 1 : 0);
                default: throw new ArgumentException(operand + " is not a valid operand");
            };
        }
    }
}