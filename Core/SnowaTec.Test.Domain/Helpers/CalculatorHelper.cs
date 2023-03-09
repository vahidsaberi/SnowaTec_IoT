using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SnowaTec.Test.Domain.Helper
{
    public enum Types
    {
        Number = 1,
        Variable = 2,
        Operator = 3
    };
    public class Item
    {
        public string Name { get; set; } = "";
        public double Value { get; set; }
        public Types Type { get; set; }
    }

    public class InfixToPostfix
    {
        private List<Item> _listPostfixItems = new List<Item>();
        // allowable operators
        private string _operators = "+-*/()%";
        private string _infix;
        private string _postfix;


        public string Infix
        {
            get { return _infix; }
            set
            {
                _infix = value.Replace("[", "(").Replace("]", ")");
                _postfix = ConvertToPostFix();
            }
        }

        public string Postfix
        {
            get { return _postfix; }
        }


        public InfixToPostfix()
        {
            _infix = "";
            _postfix = "";
        }

        public InfixToPostfix(string infix)
        {
            _postfix = "";
            _infix = "";
            Infix = infix.Replace("[", "(").Replace("]", ")");
        }


        public string GetPostfix()
        {
            if (string.IsNullOrEmpty(Infix))
            {
                throw new ArgumentNullException(nameof(Infix));
            }

            if (!IsValid(Infix))
            {
                throw new FormatException("The input is not currect Infix phrase");
            }
            return _postfix;
        }

        public double ComputeInfix()
        {
            if (string.IsNullOrEmpty(Infix))
            {
                throw new ArgumentNullException(nameof(Infix));
            }

            if (!IsValid(Infix))
            {
                throw new FormatException("The input is not currect Infix phrase");
            }

            return Compute();
        }

        private string ConvertToPostFix()
        {
            char arrival;
            Stack<char> oprerator = new Stack<char>(); //Creates a new Stack
            string temp = string.Empty;

            //Iterates characters in inFix
            for (int i = 0; i < _infix.Length; i++)
            {
                char c = _infix[i];
                if (char.IsNumber(c))
                {
                    temp += c;
                }
                else
                {
                    AddItemToList(ref temp);

                    if (c == '(')
                    {
                        oprerator.Push(c);
                    }
                    else if (c == ')') //Removes all previous elements from Stack and puts them in front of PostFix.  
                    {
                        arrival = oprerator.Pop();
                        while (arrival != '(')
                        {
                            temp += arrival;
                            arrival = oprerator.Pop();
                            AddItemToList(ref temp);
                        }
                    }
                    else
                    {
                        if (oprerator.Count != 0 && Predecessor(oprerator.Peek(), c)) //If find an operator
                        {
                            arrival = oprerator.Pop();
                            while (Predecessor(arrival, c))
                            {
                                temp += arrival;
                                AddItemToList(ref temp);
                                if (oprerator.Count == 0)
                                {
                                    arrival = '0';
                                    break;
                                }
                                if (oprerator.Count != 0 && Predecessor(oprerator.Peek(), c))
                                {
                                    arrival = oprerator.Pop();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            oprerator.Push(c);
                        }
                        else
                        {
                            oprerator.Push(c); //If Stack is empty or the operator has precedence 
                        }
                    }
                }
            }

            AddItemToList(ref temp);

            while (oprerator.Count > 0)
            {
                arrival = oprerator.Pop();
                temp += arrival;
                AddItemToList(ref temp);
            }

            foreach (Item item in _listPostfixItems)
            {
                _postfix += item.Name;
            }

            return _postfix;
        }

        private bool Predecessor(char firstOperator, char secondOperator)
        {
            Dictionary<char, ushort> precedence = new Dictionary<char, ushort>()
        {
            {'(', 1},
            {'+', 2},
            {'-', 2},
            {'*', 3},
            {'/', 3},
            {'%', 3},
        };
            return precedence[firstOperator] >= precedence[secondOperator];
        }

        private double Compute()
        {
            Stack<double> stackOperands = new Stack<double>();
            double result = 0;
            for (int p = 0; p < _listPostfixItems.Count; p++)
            {
                if (_listPostfixItems[p].Type == Types.Number)
                {
                    stackOperands.Push(_listPostfixItems[p].Value);
                }
                else
                {
                    if (stackOperands.Count >= 2)
                    {
                        double op2 = stackOperands.Pop();
                        double op1 = stackOperands.Pop();

                        // Do operation
                        if (_listPostfixItems[p].Name == "*")
                        {
                            stackOperands.Push(op1 * op2);
                        }
                        else if (_listPostfixItems[p].Name == "/")
                        {
                            stackOperands.Push(op1 / op2);
                        }
                        else if (_listPostfixItems[p].Name == "%")
                        {
                            stackOperands.Push(op1 % op2);
                        }
                        else if (_listPostfixItems[p].Name == "+")
                        {
                            stackOperands.Push(op1 + op2);
                        }
                        else if (_listPostfixItems[p].Name == "-")
                        {
                            stackOperands.Push(op1 - op2);
                        }
                    }
                    else if (stackOperands.Count == 1 && _listPostfixItems[p].Name == "-")
                    {
                        double op = stackOperands.Pop();
                        stackOperands.Push(op * (-1));
                    }
                }
            }

            if (stackOperands.Count == 1)
            {
                result = stackOperands.Pop();
            }

            return result;
        }

        private void AddItemToList(ref string name)
        {
            if (name == string.Empty) return;
            Item n = new Item();
            try
            {
                n.Value = double.Parse(name);
                n.Name = "{" + name + "}";
                n.Type = Types.Number;
            }
            catch
            {
                n.Name = name;
                if (_operators.Contains(n.Name))
                {
                    n.Type = Types.Operator;
                }
                else
                {
                    n.Type = Types.Variable;
                }
            }
            _listPostfixItems.Add(n);
            name = string.Empty;
        }

        private bool IsValid(string input)
        {
            Regex operators = new Regex(@"[\-+*/%]", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            if (string.IsNullOrEmpty(input))
            {
                return (false);
            }


            int countOfOpenParentheses = 0, countOfCloseParentheses = 0;


            foreach (char c in input)
            {
                if (c == '(')
                {
                    countOfOpenParentheses++;
                }
                else if (c == ')')
                {
                    countOfCloseParentheses++;
                }
            }

            if (countOfOpenParentheses != countOfCloseParentheses)
            {
                return false;
            }

            string tempString = operators.Replace(input, ".");

            if (tempString.EndsWith("."))
            {
                return false;
            }

            string[] contains = new string[] { "(.)", "()", "..", ".)" };

            foreach (string s in contains)
            {
                if (tempString.Contains(s))
                {
                    return false;
                }
            }

            operators = new Regex(@"[().]", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            tempString = operators.Replace(tempString, string.Empty);

            foreach (char c in tempString)
            {
                if (!char.IsNumber(c))
                {
                    return false;
                }
            }

            if (input.Contains("."))
            {
                return false;
            }

            if (input.StartsWith("*") || input.StartsWith("/") || input.StartsWith("%")
                || input.StartsWith("+") || input.StartsWith("-"))
            {
                return false;
            }

            contains = new string[] { "(%", "(/", "(*", "(+", "(-" };
            foreach (string s in contains)
            {
                if (input.Contains(s))
                {
                    return false;
                }
            }

            int begin = 0, end = 0;
            foreach (char c in input)
            {
                if (c == '(')
                {
                    begin++;
                }
                if (c == ')')
                {
                    end++;
                }
                if (end > begin)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
