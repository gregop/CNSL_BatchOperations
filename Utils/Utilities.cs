using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using BatchModel;
using System.Globalization;

namespace Utils
{

    public static class Utilities
    {

        public static Dictionary<string, object> ReadBatchFile(string fileName) 
        {

            // Batch file need to handle the transaction operations
            Dictionary<string, string> batchOptions = new Dictionary<string, string>();
            batchOptions.Add("AUTH", "Authorization");
            batchOptions.Add("CAPT", "Capture");
            batchOptions.Add("AUTHC", "Authorization and capture");
            batchOptions.Add("REF", "Refund");
            batchOptions.Add("REV", "Reversal");

            
            try
            {

                Console.WriteLine("--LOG--\t Opening file from {0:G}.", fileName);
                using (StreamReader reader = new StreamReader(fileName))
                {
                    Console.WriteLine("--LOG--\t START Reading {0:G}\t\t[OK]", fileName);
                    string fileContent = reader.ReadToEnd();
                    string[] contentPerLineArray = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    

                    if (contentPerLineArray.Length >= 3)
                    {
                        Console.WriteLine("--LOG--\t END Reading {0:G}\t\t[OK]", fileName);
                        return new Dictionary<string, object>
                            {
                                {"Head", contentPerLineArray[0] },
                                {"Body", contentPerLineArray.Skip(1)
                                            .Take(contentPerLineArray.Length-2).ToArray() },
                                {"EOF", contentPerLineArray[^1] }
                            };
                    }
                    else
                    {
                        throw new Exception("Please check your Batch again");
                    }

                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("--ERROR--\t Something went wrong\t{0:G}", ex.Message);
                return new Dictionary<string, object> 
                {
                    { "Exception", ex.Message } 
                };
            }

        }


        /* Returns the number (count) of
         * Pipes "|" in a string 
         */
        public static int CountPipes(string batchLine)
        {

            return batchLine.Count(t => t == '|');
        }

        /* Split the input string to substring 
         * usind the Pipe "|" as a delimeter
         * return a string array of the splitted strings 
         */
        public static string[] SplitBatchLine(string line)
        {
            string delimeter = "|";
            return line.Split(delimeter, StringSplitOptions.None);
        }


        /* Private method to get user input
         * and handle non-accepted cases
         */
        public static string? GetUserInput(string message, string[] acceptedValues)
        {
            string? userInput;
            Console.Write(message);
            userInput = Console.ReadLine();


            switch (userInput)
            {
                case string s when acceptedValues.Contains(s):
                    return s;

                default:
                    Console.Write("Invalid input. Please try again.");
                    return GetUserInput(message, acceptedValues);
            }
        }


        public static bool IsNumericAndValidLength(object value, params int[] validLenghts)
        {
            bool isNumeric = Int64.TryParse(value.ToString(), out Int64 number);

            if (isNumeric && number > 0)
            {
                return validLenghts.Contains(number.ToString().Length);
            }
            else
            {
                return false;
            }

        }

        public static bool IsValidExpDate(object value, params int[] validLenghts)
        {
            bool isNumeric = value.ToString().All(char.IsDigit);

            if (isNumeric && validLenghts.Contains(value.ToString().Length))
            {
                string monthString = value.ToString().Substring(0, 2);

                return Int32.Parse(monthString) >= 1 && 
                    Int32.Parse(monthString) <= 12 &&
                    Int32.Parse(value.ToString()) > 0;
            }
            else
            {
                return false;
            }

        }

        public static bool IsValidPurchaseAmt(object value)
        {
            bool isNumeric = value.ToString().All(char.IsDigit);

            return isNumeric && value.ToString().Length == 12;

            
        }

        public static bool IsValidOrderId(object value, params string[] restrictedValues)
        {
            bool containsRestrictedValue = restrictedValues.Any(restrictedValue => value.ToString().Contains(restrictedValue));

            return !containsRestrictedValue;
        }


        public static bool IsValidAuthCode(object value)
        {
            return value.ToString().All(char.IsLetterOrDigit);
        }
    }
}