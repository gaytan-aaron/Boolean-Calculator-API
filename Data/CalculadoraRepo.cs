using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CalculadoraBooleanaT7.Interfaces;

namespace CalculadoraBooleanaT7.Data
{
    public class CalculadoraRepo : ICalculadoraRepo
    {
        public void ConvertToPostFix(List<string> symbols, Stack<string> s, ref List<string> PostFix)
        {
            int n;
            foreach (string c in symbols)
            {
                if (int.TryParse(c.ToString(), out n))
                {
                    PostFix.Add(c);
                }
                if (c == "(") s.Push(c);
                if (c == ")")
                {
                    while (s.Count != 0 && s.Peek() != "(")
                    {
                        PostFix.Add(s.Pop());
                    }
                    s.Pop();
                }
                if (IsOperator(c))
                {
                    while (s.Count != 0 && Priority(s.Peek()) >= Priority(c))
                    {
                        PostFix.Add(s.Pop());
                    }
                    s.Push(c);
                }
            }
            while (s.Count != 0)
            {
                PostFix.Add(s.Pop());
            }
        }

        public List<string> GetRPN(List<string> token)
        {
            Stack<string> s = new Stack<string>();
            List<string> PostFix = new List<string>();

            try
            {
                ConvertToPostFix(token, s, ref PostFix);
                return PostFix;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool IsOperator(string c)
        {
            if (c == "*" || c == "+" || c == "^" || c == "'")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Priority(string c)
        {
            if (c == "'")
            {
                return 3;
            }
            else if (c == "*" || c == "^")
            {
                return 2;
            }
            else if (c == "+")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public bool Solver(List<string> token, bool[] valuesOfVariables)
        {
            Console.WriteLine("[SOLVER]");
            Stack<bool> stack = new Stack<bool>();

            for (int i = 0; i < token.Count; i++)
            {
                if (int.TryParse(token[i], out int n))
                {
                    stack.Push(Convert.ToBoolean(valuesOfVariables[Convert.ToInt32(token[i]) - 1]));
                }
                else if (token[i] == "^" || token[i] == "*" || token[i] == "+" || token[i] == "'")
                {
                    switch (token[i])
                    {
                        case "+":
                            stack.Push(stack.Pop() | stack.Pop());
                            break;
                        case "^":
                            stack.Push(stack.Pop() ^ stack.Pop());
                            break;
                        case "*":
                            stack.Push(stack.Pop() & stack.Pop());
                            break;
                        case "'":
                            stack.Push(!stack.Pop());
                            break;
                        default:
                            throw new Exception("ERROR: Invalid Operation");
                    }
                }
            }
            return stack.Pop();
        }

        public List<string> Tokenize(string input)
        {
            List<string> TokenResult = new List<string>();
            input = Regex.Replace(input, @"\s+", "");
            string numberString = "";
            foreach (char c in input)
            {
                if (Char.IsLetter(c))
                {
                    numberString += c;
                }
                else
                {
                    TokenResult.Add(numberString);
                    numberString = "";
                    TokenResult.Add(c.ToString());
                }
            }
            return TokenResult;
        }
    }
}