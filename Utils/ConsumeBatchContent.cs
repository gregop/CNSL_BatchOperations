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
        private List<object> _operations;
        private int _batchSystemId;
        private string _batchId;
        private string _outletId;

        private readonly string[] _body;

        public ConsumeBatchContent(string head, string[] body, string eof)
        {
            _errorMessages = new List<string>();
            _body = body;

            BatchFile batchModel = new BatchFile();
            _batchSystemId = batchModel.BatchSystemId;

            try
            {
                ConsumeBatchHeadandEof(head, eof, batchModel);
                //ConsumeBatchBody(body);

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

                _batchId = head_values[3];
                _outletId = head_values[1];

                List<string> validationErrors = batchModel.Validate().GetErrors();

                if (validationErrors.Count == 0)
                {
                    Console.WriteLine("BatchSystemId {0} Head and Eof Consumer without Errors", batchModel.BatchSystemId);
                    ConsumeBatchBody(_body);
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
                for(int i=0; i <= body.Length-1; i++)
                {
                    Console.WriteLine($"Line {i + 1}, {body[i]}");

                    string[] operation = Utilities.SplitBatchLine(body[i]);

                    Console.WriteLine(operation[0]);

                    switch (operation[0])
                    {
                        case ("AUTH"):
                            Authorization(operation, i + 1);
                            break;

                        case ("AUTHC"):
                            AuthorizationCapture(operation, i + 1);
                            break;

                        case ("CAPT"):
                            Capture(operation, i + 1);
                            break;

                        case ("REF"):
                            Refund(operation, i + 1);
                            break;

                        case ("REV"):
                            Reversal(operation, i + 1);
                            break;

                        default:
                            _errorMessages.Add($"Unrecognized operation {operation[0]} on line {i + 1}");
                            break;
                    }

                }
                
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

        private void Authorization(string[] operation, int lineNumber)
        {
            Auth auth = new Auth();

            auth.BatchSystemId = _batchSystemId;
            auth.BatchIdLined = _batchId;
            auth.OutletId = _outletId;
            auth.AuthPipes = operation.Length - 1;
            auth.CardNum = operation[1];
            auth.ExpDate = operation[2];
            auth.Cvv = operation[3];
            auth.PurchaseAmt = operation[4];
            auth.Currency = operation[5];
            auth.OrderId = operation[6];



            List<string> validationMessages = auth.ValidateAuthLine(lineNumber).GetErrors();

            if (validationMessages.Count > 0)
            {
                _errorMessages.AddRange(validationMessages);
                return;
            }
            else
            {
                _operations.Add(auth);
            }
            
        }

        private void AuthorizationCapture(string[] operation, int lineNumber)
        {

            AuthCapture authC = new AuthCapture();

            authC.BatchSystemId = _batchSystemId;
            authC.BatchIdLined = _batchId;
            authC.OutletId = _outletId;
            authC.AuthCPipes = operation.Length - 1;
            authC.CardNum = operation[1];
            authC.CardNum = operation[2];
            authC.ExpDate = operation[3];
            authC.Cvv = operation[4];
            authC.PurchaseAmt = operation[5];
            authC.PurchaseAmtCapture = operation[6];
            authC.Currency = operation[7];
            authC.OrderId = operation[8];


            List<string> validationMessages = authC.ValidateAuthC(lineNumber).GetErrors();

            if (validationMessages.Count > 0)
            {
                _errorMessages.AddRange(validationMessages);
                return;
            }
            else
            {
                _operations.Add(authC);
            }


        }

        private void Capture(string[] operation, int lineNumber)
        {

            Capture capt = new Capture();

            capt.BatchSystemId = _batchSystemId;
            capt.BatchIdLined = _batchId;
            capt.OutletId = _outletId;
            capt.CaptPipes = operation.Length - 1;
            capt.PurchaseAmt = operation[4];
            capt.Currency = operation[5];
            capt.PurchaseAmtCapture = operation[6];
            capt.OrderId = operation[8];
            capt.AuthCode = operation[9];

            List<string> validationMessages = capt.ValidateCapture(lineNumber).GetErrors();

            if (validationMessages.Count > 0)
            {
                _errorMessages.AddRange(validationMessages);
                return;
            }
            else
            {
                _operations.Add(capt);
            }

        }

        private void Refund(string[] operation, int lineNumber)
        {
            Refund refund = new Refund();

            refund.BatchSystemId = _batchSystemId;
            refund.BatchIdLined = _batchId;
            refund.OutletId = _outletId;
            refund.RefPipes = operation.Length - 1; 
            refund.RefundAmt = operation[4];
            refund.Currency = operation[5];
            refund.OrderId = operation[6];
            refund.AuthCode = operation[7];

            List<string> validationMessages = refund.ValidateRefund(lineNumber).GetErrors();

            if (validationMessages.Count > 0)
            {
                _errorMessages.AddRange(validationMessages);
                return;
            }
            else
            {
                _operations.Add(refund);
            }

        }

        private void Reversal(string[] operation, int lineNumber)
        {
            Reversal reversal = new Reversal();

            reversal.BatchSystemId = _batchSystemId;
            reversal.BatchIdLined = _batchId;
            reversal.OutletId = _outletId;
            reversal.RevPipes = operation.Length - 1;
            reversal.PurchaseAmt = operation[4];
            reversal.Currency = operation[5];
            reversal.ReversalAmount = operation[6];
            reversal.OrderId = operation[7];
            reversal.AuthCode = operation[8];

            List<string> validationMessages = reversal.ValidateReversal(lineNumber).GetErrors();

            if (validationMessages.Count > 0)
            {
                _errorMessages.AddRange(validationMessages);
                return;
            }
            else
            {
                _operations.Add(reversal);
            }

        }

    }
}
