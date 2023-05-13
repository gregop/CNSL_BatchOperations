﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNSL_BatchOperations.Models;
using Utils;
using BatchModel;

namespace CNSL_BatchOperations.Utils.ConsumeBatch
{
    public class ConsumeBatchContent
    {
        private List<string> _errorMessages;

        public ConsumeBatchContent(string head, string[] body, string eof)
        {
            _errorMessages = new List<string>();
            BatchFile batchModel = new BatchFile();

            try
            {
                ConsumeBatchHeadandEof(head, eof, batchModel);
                ConsumeBatchBody(body);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            
        }

        private void ConsumeBatchHeadandEof(string head, string eof, BatchFile batchModel)
        {

            int head_Pipes = Utilities.CountPipes(head);

            string[]? head_values = Utilities.SplitBatchLine(head);

            if (head_values.Length != head_Pipes +1)
            {
                _errorMessages.Add("Invalid Batch Head");
                return;
            }
            else
            {
                // Assign batch file model values
                Console.WriteLine("BatchSystemId: {0}", batchModel.BatchSystemId);
                batchModel.HeadPipes = head_Pipes;
                batchModel.Eof = eof;
                batchModel.Mdet = head_values[0];
                batchModel.Outlet = head_values[1];
                batchModel.AcquirerId = head_values[2];
                batchModel.BatchId = head_values[3];

                List<string> validationErrors = batchModel.Validate().GetErrors();

                if (validationErrors.Count == 0)
                {
                    Console.WriteLine("No Errors");
                }
                else
                {
                    foreach (string error in validationErrors)
                    {
                        Console.WriteLine(error);
                    }
                }
                
            }

            
        }

        private void ConsumeBatchBody(string[] body)
        {
            /* Do not proceed with Body validations if
             * batch head is not valid
             */
            if (_errorMessages.Count > 0)
            {
                return;
            }
            else
            {
                //do what you have to do
            }
        }


        public bool BatchConsumed()
        {
            return _errorMessages.Count == 0;
        }

        public List<string> GetErrorMessages()
        {
            return _errorMessages;
        }

    }
}
