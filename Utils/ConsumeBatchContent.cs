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
            Console.WriteLine("BatchSystemId: {0}", batchModel.BatchSystemId);
        }

        private void ConsumeBatchBody(string[] body)
        {
            Console.WriteLine("ConsumeBatchBody");
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
