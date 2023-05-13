using System;
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
                batchModel.HeadPipes = head_Pipes;
                batchModel.Eof = eof;

                Console.WriteLine("BatchSystemId: {0}", batchModel.BatchSystemId);

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
