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

        //public static Dictionary<string, string> readBatchFile(string fileName) 
        public static string readBatchFile(string fileName)
        {

            // Batch file need to handle the transaction operations
            Dictionary<string, string> batchOptions = new Dictionary<string, string>();
            batchOptions.Add("AUTH", "Authorization");
            batchOptions.Add("CAPT", "Capture");
            batchOptions.Add("AUTHC", "Authorization and capture");
            batchOptions.Add("REF", "Refund");
            batchOptions.Add("REV", "Reversal");

            //Console.WriteLine(batchFile.BatchSystemId.ToString());
            try
            {

                Console.WriteLine("--LOG--\t Opening file from {0:G}.", fileName);
                using (StreamReader reader = new StreamReader(fileName))
                {
                    Console.WriteLine("--LOG--\t Reading {0:G}\t\t[OK]", fileName);
                    string fileContent = reader.ReadToEnd();

                    string[] contentPerLineArray = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);


                    // if (isValidBatch(fileContent, batchFile)){

                    //     //write function to readperline and build requests as json
                    //     return true;
                    // } 
                    // else {
                    //     return false;
                    // }  

                    return fileContent;
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("--ERROR--\t Something went wrong\t{0:G}", ex.Message);
                return ex.Message;
            }

        }


        private static bool isValidBatch(string batchContent, BatchFile batchFile)
        {

            Console.WriteLine("--LOG--\t Validating batch file...");

            // SEPERATE ALL LINES USING LINE BREAKS AND STORE THEM IN A DICTIONARY
            string[] contentPerLineArray = batchContent.Split(new[]
                { Environment.NewLine }, StringSplitOptions.None);

            /* VALIDATION - PHASE 1 - START
            *   add all validation functions to a list and check the list validity,
            *   at the end of the IsValid Batch function
            */
            // 1. CHECK FIRST and LAST line
            String[] firstAndLastLines =
                        new string[2] {contentPerLineArray[0],
                         contentPerLineArray[contentPerLineArray.Length -1]};

            if (CheckFirstAndLastLine(firstAndLastLines, batchFile))
            {
                return true;
            }
            else
            {
                return false;
            };

            /* VALIDATION - PHASE 2 - START*/



            //return true;

        }


        private static bool CheckFirstAndLastLine(string[] lines, BatchFile batchFile)
        {

            bool check_MED, check_EOF;
            try
            {

                //CHECK MDET
                if (lines[0] != null)
                {
                    if (lines[0].Substring(0, 4) == "MDET" && countPipes(lines[0]) == 3)
                    {
                        Console.WriteLine("--LOG--\t Batch Merchant Details line Format\t\t[OK]");
                        check_MED = true;
                    }
                    else
                    {
                        Console.WriteLine("--LOG--\t Batch First Line Out of Format\t\t[NOK] <--");
                        check_MED = false;
                    }
                }
                else
                {
                    Console.WriteLine("--LOG--\t Batch First Line Out of Format\t\t[NOK]");
                    check_MED = false;
                }

                // CHECK EOF
                if (lines[1] != null && lines[1].Length == 3 && lines[1] == "EOF")
                {

                    string[] contentFirstLine = lines[0].Split(new[] { "|" }, StringSplitOptions.None);
                    if (contentFirstLine[2] == "402971")
                    {
                        batchFile.Acquirer = 402971;
                    }
                    else
                    {
                        Console.WriteLine("--LOG--\t Line 1: AqcuirerId Format\t\t[NOK]");
                        check_EOF = false;
                        //return false;
                    }

                    int AcqId;
                    bool success = int.TryParse(contentFirstLine[2], out AcqId);
                    batchFile.Acquirer = AcqId;
                    batchFile.Outlet = contentFirstLine[1];
                    batchFile.BatchId = contentFirstLine[3];

                    Console.WriteLine("--LOG--\t Batch EOF line Format\t\t[OK]");
                    check_EOF = true;
                    //return true;

                }
                else
                {
                    Console.WriteLine("--LOG--\t Batch Last Line Out of Format\t\t[NOK]");
                    check_EOF = false;
                    //return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("--ERROR--\t Something went wrong\t{0:G}", ex.Message);
                return false;
            }

            return check_MED && check_EOF;

        }

        private static int countPipes(string batchLine)
        {

            return batchLine.Count(t => t == '|');
        }

    }
}