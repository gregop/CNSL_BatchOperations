using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BatchModel
{

    public class BatchFile
    {

        /* only get allowed on BatchSystemId.
         * This will set the BatchSystemId, by reading the total
         * number of batches processed, via a file, and add 1
        */
        public static int BatchSystemId { get { return SetBatchSystemId(); } }

        [Range(3, 3, ErrorMessage = "Head of Batch is out of format.")]
        public int HeadPipes { get; set; }

        public string? Mdet { get; set; }

        [ValidateOutlet("outlet")]
        public string? Outlet {get; set;}

        [Compare("404040", ErrorMessage = "Invalid Acquirer Id")]
        public string? AcquirerId {get; set;}

        public string? BatchId { get; set; }

        public string ?Body {get; set;}

        public string? Eof { get; set; }


        private protected static int SetBatchSystemId(){
            return 1;
        }

    }

    internal class Auth : BatchFile
    {
        public int BatchLinked  { get { return SetBatchSystemId(); }}

        [Range(6, 6, ErrorMessage = "AUTH line out of format")]
        public int AuthPipes { get; set; }

        [ValidateCardNum("CardNum")]
        public string CardNum {get; set;}
        public int Currency {get; set;}
        public string ExpDate {get; set;}
        public string Cvv {get; set;}
        public string PurchaseAmt {get; set;}
        public string? OrderId {get; set;}

    }

    internal class AuthCapture : BatchFile
    {
        public int BatchLinked  { get { return SetBatchSystemId(); }}

        [Range(10, 10, ErrorMessage = "AUTHC line out of format")]
        public int AuthCPipes { get; set; }

        [ValidateCardNum("CardNum")]
        public string ?CardNum {get; set;}
        public int ?Currency {get; set;}
        public string ?ExpDate {get; set;}
        public int Cvv {get; set;}
        public Int64 PurchaseAmt {get; set;}
        public Int64 PurchaseAmtCapture {get; set;}
        public string? OrderId {get; set;}

    }

    internal class Capture : BatchFile
    {
        public int BatchLinked  { get { return SetBatchSystemId(); }}

        [Range(9, 9, ErrorMessage = "AUTHC line out of format")]
        public int CaptPipes { get; set; }
        public int Currency {get; set;}
        public Int64 PurchaseAmt {get; set;}
        public Int64 PurchaseAmtCapture {get; set;}
        public string? OrderId {get; set;}
        public string? AuthCode {get; set;}

    }
    
}