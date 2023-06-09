﻿using Utils;
using System.IO;
using CNSL_BatchOperations.Utils.ConsumeBatch;

namespace CNSL_BatchOperations
{
    internal class Program
    {
 
        static void Main(string[] args)
        {

            string[] acceptedValues = { "y", "n" };
            string? userInput = Utilities.GetUserInput("Proceed with batch file? (y/n) : ", acceptedValues);

            if (userInput == "y")
            {
                string file_name = "validBatchFile.txt";

                /* run this when on Debug mode.
                 * Directory.GetCurrentDirectory() returns the current 
                 * working directory, which by default is the bin\Debug 
                 * when running the application from within an IDE.
                 */

                string directory_path = Path.Combine(Directory
                    .GetParent(AppDomain.CurrentDomain.BaseDirectory)
                    .Parent
                    .Parent
                    .Parent
                    .FullName, "BatchFiles");


                string file_path = Path.Combine(directory_path, file_name);
                Console.WriteLine(file_path);


                Dictionary<string, object> batch_content = Utilities.ReadBatchFile(file_path);

                if (batch_content["Body"] is string[] body)
                {
                    ConsumeBatchContent consume = new ConsumeBatchContent(
                        batch_content["Head"].ToString(),
                        body,
                        batch_content["EOF"].ToString());

                    

                    if (!consume.IsBatchConsumed())
                    {
                        List<string> errorMessages = consume.GetErrorMessages();
                        foreach (string error in errorMessages)
                        {
                            Console.WriteLine(error);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Errors");
                        List<object> paymentOperations = consume.GetOperations();

                        foreach (object operation in paymentOperations)
                        {
                            Console.WriteLine(operation);

                        }

                    }
                    
                }


                
            }
            else
            {
                Environment.Exit(0);
            }
            

        }

    }
}