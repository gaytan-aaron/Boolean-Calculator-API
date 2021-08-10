using System.Collections.Generic;

namespace CalculadoraBooleanaT7.Interfaces
{
    public interface ICalculadoraRepo
    {
        public List<string> Tokenize(string input);
        public void ConvertToPostFix(List<string> symbols, Stack<string> s, ref List<string> PostFix);
        public int Priority(string c);
        public bool IsOperator(string c);
        public List<string> GetRPN(List<string> token);
        public bool Solver(List<string> token, bool[] valuesOfVariables);
    }
}