using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual_Project
{
    public static class Helper
    {
        public static int Convert4BitStringToInt(string _4Bit)
        {
            if (_4Bit.Length != 4)
                return -1;
            int output = 0;
            int position = 3;
            foreach (char c in _4Bit)
            {
                if (c == '1')
                {
                    output += (int)Math.Pow(2, position);
                }
                position--;
            }
            return output;
        }

        public static string GenerateXbitString(int X)
        {
            string output = "";
            for (int i = 0; i < X; i++)
            {
                output += ApplicationSettings.random.Next() % 2 == 0 ? "0" : "1";
            }

            return output;
        }

        public static int[] DecodeString(string stringToDecode)
        {
            if (stringToDecode.Length % 4 != 0)
                return null;

            int _4BitValue;
            int[] output = new int[stringToDecode.Length / 4];

            for (int i = 0; i < stringToDecode.Length / 4; i++)
            {
                _4BitValue = Convert4BitStringToInt(stringToDecode.Substring(4 * i, 4));
                output[i] = _4BitValue;
            }

            return output;
        }

        public static string PrintExpression(string expresssion)
        {
            int[] expressionArray = DecodeString(expresssion);

            return PrintExpression(expressionArray);
        }

        public static string PrintExpression(int[] expression)
        {
            string output = "";

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] < 10)
                {
                    output += expression[i];
                }
                else
                {
                    switch(expression[i])
                    {
                        case 10:
                            output += "+";
                            break;
                        case 11:
                            output += "-";
                            break;
                        case 12:
                            output += "*";
                            break;
                        case 13:
                            output += "/";
                            break;
                        default:
                            output += "N/A";
                            break;
                    }
                }
                output += " ";
            }
            return output;
        }

        public static float CalculateExpresssion(int[] expression)
        {
            float value = 0;
            bool searchForNumber = true;
            bool searchForOperator = false;
            bool isFirst = true;
            int operatorFound = -1;
            int numberFound = -1;
            for (int i = 0; i < expression.Length; i ++)
            {
                if(searchForNumber && expression[i] < 10)
                {
                    numberFound = expression[i];
                    searchForNumber = !searchForNumber;
                    searchForOperator = !searchForOperator;

                } else if(searchForOperator && expression[i] >= 10 && expression[i] <= 13)
                {
                    operatorFound = expression[i];
                    searchForNumber = !searchForNumber;
                    searchForOperator = !searchForOperator;
                }

                if(isFirst && numberFound != -1)
                {
                    value = numberFound;
                    numberFound = -1;
                    isFirst = false;
                }

                if(operatorFound != - 1 && numberFound != -1)
                {
                    switch(operatorFound)
                    {
                        case 10:
                            value += numberFound;
                            break;
                        case 11:
                            value -= numberFound;
                            break;
                        case 12:
                            value *= numberFound;
                            break;
                        case 13://check for 0
                            if(numberFound != 0)
                            {
                                value /= numberFound;
                            }
                            break;
                    }
                    operatorFound = -1;
                    numberFound = -1;
                }
            }
            return value;
        }

        public static float AssignFitness(string bits, float target_value)
        {
            int[] expressionArray = DecodeString(bits);
            float result = CalculateExpresssion(expressionArray);

            if (result == target_value)

                return 100000.0f;

            else

                return 1 / (float)Math.Abs((double)(target_value - result));
        }

        public static string Mutate(string bits)
        {
            string output = "";

            for (int i = 0; i < bits.Length; i++)
            {
                if (ApplicationSettings.random.NextDouble() < ApplicationSettings.MUTATION_RATE)
                {
                    if (bits[i] == '1')

                        output += "0";

                    else

                        output += "1";
                }
                else
                {
                    output += bits[i];
                }
            }

            return output;
        }

        public static void Crossover(ref string offspring0, ref string offspring1)
        {
            if(ApplicationSettings.random.NextDouble() < ApplicationSettings.CROSSOVER_RATE)
            {
                int crossoverPivot = (int)(ApplicationSettings.random.NextDouble() * offspring0.Length);

                string offspring0Temp = offspring0.Substring(0, crossoverPivot) + offspring1.Substring(crossoverPivot);
                string offspring1Temp = offspring1.Substring(0, crossoverPivot) + offspring0.Substring(crossoverPivot);

                offspring0 = offspring0Temp;
                offspring1 = offspring1Temp;
            }
        }

        public static string Roulette(float total_fitness, chromo_typ[] Population)
        {
            //generate a random number between 0 & total fitness count
            float Slice = (float)(ApplicationSettings.random.NextDouble() * total_fitness);

            //go through the chromosones adding up the fitness so far
            float FitnessSoFar = 0.0f;

            for (int i = 0; i < Population.Length; i++)
            {
                FitnessSoFar += Population[i].fitness;

                //if the fitness so far > random number return the chromo at this point
                if (FitnessSoFar >= Slice)

                    return Population[i].bits;
            }

            return "";
        }
    }
}
