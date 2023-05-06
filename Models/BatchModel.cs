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
        public int BatchSystemId { get { return SetBatchSystemId(); } }
        public string? Mdet { get; set; }

        [ValidateOutlet(ErrorMessage = "{0} field validation failed.")]
        public string? Outlet {get; set;}

        [ValidateAcquirer(ErrorMessage = "{0} field validation failed.")]
        public int Acquirer {get; set;}
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

        [ValidateCardNum(ErrorMessage = "{0} field validation failed.")]
        public Int64 CardNum {get; set;}
        public int Currency {get; set;}
        public int ExpDate {get; set;}
        public int Cvv {get; set;}
        public Int64 PurchaseAmt {get; set;}
        public string? OrderId {get; set;}

    }

    internal class AuthCapture : BatchFile
    {
        public int BatchLinked  { get { return SetBatchSystemId(); }}

        [ValidateCardNum(ErrorMessage = "{0} field validation failed.")]
        public Int64 CardNum {get; set;}
        public int Currency {get; set;}
        public int ExpDate {get; set;}
        public int Cvv {get; set;}
        public Int64 PurchaseAmt {get; set;}
        public Int64 PurchaseAmtCapture {get; set;}
        public string? OrderId {get; set;}

    }

    internal class Capture : BatchFile
    {
        public int BatchLinked  { get { return SetBatchSystemId(); }}
        public int Currency {get; set;}
        public Int64 PurchaseAmt {get; set;}
        public Int64 PurchaseAmtCapture {get; set;}
        public string? OrderId {get; set;}
        public string? AuthCode {get; set;}

    }
    
}