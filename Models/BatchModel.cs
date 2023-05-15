using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CNSL_BatchOperations.Models.Validations;
using Utils;

namespace BatchModel
{

    public class BatchFile
    {

        /* only get allowed on BatchSystemId.
         * This will set the BatchSystemId, by reading the total
         * number of batches processed, via a file, and add 1
        */
        public int BatchSystemId { get { return SetBatchSystemId(); } }

        //[Range(3, 3, ErrorMessage = "Head of Batch is out of format.")]
        public int HeadPipes { get; set; }

        public string? Mdet { get; set; }

        //[ValidateOutlet("outlet")]
        public string? Outlet {get; set;}

        //[Compare("402971", ErrorMessage = "Invalid Acquirer Id")]
        public string? AcquirerId {get; set;}

        public string? BatchId { get; set; }

        public string ?Body {get; set;}

        //[Compare("EOF", ErrorMessage = "End of File out of format")]
        public string? Eof { get; set; }

        public ValidationHelper Validate()
        {
            ValidationHelper validation = new ValidationHelper();

            if (HeadPipes != 3 ||
                Mdet != "MDET" ||
                AcquirerId != "402971") 
            { 
                validation.AddError("Batch head format invalid"); 
            };

            if (!IsValidOutlet(Outlet))
            {
                validation.AddError("Outlet number out of format");
            }

            if(BatchId == "" || BatchId.Length > 20) 
            {
                validation.AddError("Batch Id out of format");
            }
            
            if(Eof != "EOF")
            {
                validation.AddError("End of line is missing or invalid");
            }


            return validation;
            
        }

        private bool IsValidOutlet(string outlet)
        {
            return outlet.All(char.IsDigit)
               && outlet.ToString().Length == 10;
        }

        private protected static int SetBatchSystemId(){
            return 1;
        }

    }

    public class Auth
    {
        public int BatchSystemId { get; set; }

        public string BatchIdLined { get; set; }

        //[Range(6, 6, ErrorMessage = "AUTH line out of format")]
        public int AuthPipes { get; set; }

        //[ValidateCardNum("CardNum")]
        public string CardNum {get; set;}
        public string Currency {get; set;}
        public string ExpDate {get; set;}
        public string Cvv {get; set;}
        public string PurchaseAmt {get; set;}
        public string OrderId {get; set;}

        public ValidationHelper ValidateAuthLine(int lineNumber)
        {
            ValidationHelper validation = new ValidationHelper();

            if (AuthPipes != 6)
            {
                validation.AddError($"Batch Line {lineNumber} format invalid");
                return validation;
            };

            if(! Utilities.IsNumericAndValidLength(CardNum, 15, 16))
            {
                validation.AddError($"Batch Line {lineNumber} Card format invalid");
            }

            if (!Utilities.IsNumericAndValidLength(Currency, 3))
            {
                validation.AddError($"Batch Line {lineNumber} Currency format invalid");
            }

            if (!Utilities.IsValidExpDate(ExpDate, 4) )
            {
                validation.AddError($"Batch Line {lineNumber} Card Expiry Date format invalid");
            }

            if (!Utilities.IsNumericAndValidLength(Cvv, 3, 4))
            {
                validation.AddError($"Batch Line {lineNumber} CVV format invalid");
            }

            if (!Utilities.IsValidPurchaseAmt(PurchaseAmt))
            {
                validation.AddError($"Batch Line {lineNumber} Purchase Amount format invalid");
            }

            if (!Utilities.IsValidOrderId(OrderId, "\"", "'", "=", "/"))
            {
                validation.AddError($"Batch Line {lineNumber} Order Id format invalid. Remove all chars like: \", ', =, / ");
            }

            return validation;

        }

    }

    public class AuthCapture
    {
        public int BatchSystemId { get; set; }
        public string BatchIdLined { get; set; }
        //[Range(10, 10, ErrorMessage = "AUTHC line out of format")]
        public int AuthCPipes { get; set; }

        //[ValidateCardNum("CardNum")]
        public string CardNum {get; set;}
        public string Currency {get; set;}
        public string ExpDate {get; set;}
        public string Cvv {get; set;}
        public string PurchaseAmt {get; set;}
        public string PurchaseAmtCapture {get; set;}
        public string OrderId {get; set;}


        public ValidationHelper ValidateAuthC(int lineNumber)
        {
            ValidationHelper validation = new ValidationHelper();

            if (AuthCPipes != 10)
            {
                validation.AddError($"Batch Line {lineNumber} format invalid");
                return validation;
            };

            if (!Utilities.IsNumericAndValidLength(CardNum, 15, 16))
            {
                validation.AddError($"Batch Line {lineNumber} Card format invalid");
            }

            if (!Utilities.IsNumericAndValidLength(Currency, 3))
            {
                validation.AddError($"Batch Line {lineNumber} Currency format invalid");
            }

            if (!Utilities.IsValidExpDate(ExpDate, 4))
            {
                validation.AddError($"Batch Line {lineNumber} Card Expiry Date format invalid");
            }

            if (!Utilities.IsNumericAndValidLength(Cvv, 3, 4))
            {
                validation.AddError($"Batch Line {lineNumber} CVV format invalid");
            }

            if (!Utilities.IsValidPurchaseAmt(PurchaseAmt))
            {
                validation.AddError($"Batch Line {lineNumber} Purchase Amount format invalid");
            }

            if (!Utilities.IsValidPurchaseAmt(PurchaseAmtCapture))
            {
                validation.AddError($"Batch Line {lineNumber} Purchase Amount format invalid");
            }

            if (!Utilities.IsValidOrderId(OrderId, "\"", "'", "=", "/"))
            {
                validation.AddError($"Batch Line {lineNumber} Order Id format invalid. Remove all chars like: \", ', =, / ");
            }

            return validation;

            
        }

    }

    public class Capture
    {
        public int BatchSystemId { get; set; }
        public string BatchIdLined { get; set; }

        //[Range(9, 9, ErrorMessage = "AUTHC line out of format")]
        public int CaptPipes { get; set; }
        public string Currency {get; set;}
        public string PurchaseAmt {get; set;}
        public string PurchaseAmtCapture {get; set;}
        public string OrderId {get; set;}
        public string AuthCode {get; set;}


        public ValidationHelper ValidateCapture(int lineNumber)
        {
            ValidationHelper validation = new ValidationHelper();

            if (CaptPipes != 9)
            {
                validation.AddError($"Batch Line {lineNumber} format invalid");
                return validation;
            };

            if (!Utilities.IsNumericAndValidLength(Currency, 3))
            {
                validation.AddError($"Batch Line {lineNumber} Currency format invalid");
            }

            if (!Utilities.IsValidPurchaseAmt(PurchaseAmt))
            {
                validation.AddError($"Batch Line {lineNumber} Purchase Amount format invalid");
            }

            if (!Utilities.IsValidPurchaseAmt(PurchaseAmtCapture))
            {
                validation.AddError($"Batch Line {lineNumber} Purchase Amount format invalid");
            }

            if (!Utilities.IsValidOrderId(OrderId, "\"", "'", "=", "/"))
            {
                validation.AddError($"Batch Line {lineNumber} Order Id format invalid. Remove all chars like: \", ', =, / ");
            }

            if (!Utilities.IsValidAuthCode(AuthCode))
            {
                validation.AddError($"Batch Line {lineNumber} AuthCode format invalid");
            }


            return validation;

        }

    }


    public class Refund
    {

        public int BatchSystemId { get; set; }
        public string BatchIdLined { get; set; }
        public int RefPipes { get; set; }
        public string RefundAmt { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public string AuthCode { get; set; }


        public ValidationHelper ValidateRefund(int lineNumber)
        {
            ValidationHelper validation = new ValidationHelper();

            if (RefPipes != 7)
            {
                validation.AddError($"Batch Line {lineNumber} format invalid");
                return validation;
            };

            if (!Utilities.IsNumericAndValidLength(Currency, 3))
            {
                validation.AddError($"Batch Line {lineNumber} Currency format invalid");
            }


            if (!Utilities.IsValidPurchaseAmt(RefundAmt))
            {
                validation.AddError($"Batch Line {lineNumber} Purchase Amount format invalid");
            }

            if (!Utilities.IsValidOrderId(OrderId, "\"", "'", "=", "/"))
            {
                validation.AddError($"Batch Line {lineNumber} Order Id format invalid. Remove all chars like: \", ', =, / ");
            }

            if (!Utilities.IsValidAuthCode(AuthCode))
            {
                validation.AddError($"Batch Line {lineNumber} AuthCode format invalid");
            }


            return validation;
        }

    }

    public class Reversal
    {
        public int BatchSystemId { get;  set;  }
        public string BatchIdLined { get; set; }
        public int RevPipes { get; set; }
        public string PurchaseAmt { get; set; }
        public string Currency { get; set; } 
        public string ReversalAmount { get; set; }
        public string OrderId { get; set; }
        public string AuthCode { get; set; }


        public ValidationHelper ValidateReversal(int lineNumber)
        {
            ValidationHelper validation = new ValidationHelper();

            if (RevPipes != 7)
            {
                validation.AddError($"Batch Line {lineNumber} format invalid");
                return validation;
            };

            if (!Utilities.IsNumericAndValidLength(Currency, 3))
            {
                validation.AddError($"Batch Line {lineNumber} Currency format invalid");
            }

            if (!Utilities.IsValidPurchaseAmt(PurchaseAmt))
            {
                validation.AddError($"Batch Line {lineNumber} Purchase Amount format invalid");
            }


            if (!Utilities.IsValidPurchaseAmt(ReversalAmount))
            {
                validation.AddError($"Batch Line {lineNumber} Purchase Amount format invalid");
            }

            if (!Utilities.IsValidOrderId(OrderId, "\"", "'", "=", "/"))
            {
                validation.AddError($"Batch Line {lineNumber} Order Id format invalid. Remove all chars like: \", ', =, / ");
            }

            if (!Utilities.IsValidAuthCode(AuthCode))
            {
                validation.AddError($"Batch Line {lineNumber} AuthCode format invalid");
            }


            return validation;

        }

    }
    
}