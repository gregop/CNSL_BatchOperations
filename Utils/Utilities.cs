using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using BatchModel;

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



        private static int CountPipes(string batchLine)
        {

            return batchLine.Count(t => t == '|');
        }

    }
}