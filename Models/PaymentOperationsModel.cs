using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentOperationsModels
{
    // implement a class to retrieve credentials, and set to classes below


    public class OpAuthorization
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CardNumber { get; set; }
        public string OrderNumber { get; set; }
        public string Amount { get; set; }
        public bool BindingNotNeeded { get; set; }
        public bool PreAuth = true;
        public string Pan { get; set; }
        public string Cvc { get; set; }
        public string Expiry { get; set; }
        public string DynamicCallbackUrl { get; set; }

    }

    public class OpAuthCapture
    {

    }

    public class OpCapture
    {

    }

    public class OpRefund
    {

    }

    public class OpReversal
    {

    }
}
