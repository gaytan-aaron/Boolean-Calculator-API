using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CalculadoraBooleanaT7.DTOs;
using CalculadoraBooleanaT7.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraBooleanaT7.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculadoraController : ControllerBase
    {
        private readonly ICalculadoraRepo _calculadoraRepo;

        public CalculadoraController(ICalculadoraRepo calculadoraRepo)
        {
            _calculadoraRepo = calculadoraRepo;
        }

        [HttpPost]
        public  ActionResult<DataTable> Solve(ExpresionCreateDto expresionCreateDto)
        {
            string expresion = expresionCreateDto.expresion;
            int numberOfVariables = expresionCreateDto.numberOfVariables;

            DataTable dt = new DataTable();
            int biggestValue = Convert.ToInt32(Math.Pow(2, numberOfVariables)) - 1;
            int biggestDigitLength = Convert.ToString(biggestValue, 2).Length;
            DataColumn output = new DataColumn("Output", typeof(string));

            for (int i = 1; i <= numberOfVariables; i++)
            {
                dt.Columns.Add(new DataColumn(i.ToString(), typeof(string)));
            }
            dt.Columns.Add(output);

            for (int i = 0; i < Math.Pow(2, numberOfVariables); i++)
            {
                string binary = Convert.ToString(i, 2);
                binary = binary.PadLeft(biggestDigitLength, '0');
                bool[] binaryExpression = binary.Select(c => c == '1').ToArray();

                DataRow inputRow = dt.NewRow();

                //Add
                for (int j = 0; j < binaryExpression.Length; j++)
                {
                    if (binaryExpression[j] == true)
                    {
                        inputRow[j] = "True";
                    }
                    else if (binaryExpression[j] == false)
                    {
                        inputRow[j] = "False";
                    }
                }

                List<string> token = _calculadoraRepo.Tokenize(expresion);
                List<string> RPN = _calculadoraRepo.GetRPN(token);
                
                bool truthTableValue = _calculadoraRepo.Solver(RPN, binaryExpression);
                if (truthTableValue == true)
                {
                    inputRow[binaryExpression.Length] = "True";
                }
                else
                {
                    inputRow[binaryExpression.Length] = "False";
                }
                dt.Rows.Add(inputRow);
            }
            
            return Ok(dt);
        }
    }
}