using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNSL_BatchOperations.Models.Validations
{
    public class ValidationHelper
    {
        private List<string> _validationErrors;

        public ValidationHelper()
        {
            _validationErrors = new List<string>();
        }

        protected bool IsValid()
        {
            return _validationErrors.Count == 0;
        }

        public void AddError(string errorMessage)
        {
            _validationErrors.Add(errorMessage);
        }

        public List<string> GetErrors()
        {
            return _validationErrors;
        }
    }
}
