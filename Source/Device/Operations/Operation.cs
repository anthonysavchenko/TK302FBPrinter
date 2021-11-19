using System.Collections.Generic;

namespace TK302FBPrinter.Device.Operations
{
    public class Operation : IOperation
    {
        private List<string> _errorsDescriptions = new List<string>();

        protected void AddErrorDescription(string errorDescription)
        {
            _errorsDescriptions.Add(errorDescription);
        }

        public string ErrorDescriptions
        {
            get
            {
                if (_errorsDescriptions.Count == 0)
                {
                    return string.Empty;
                }

                if (_errorsDescriptions.Count == 1)
                {
                    return $"{_errorsDescriptions[0]}";
                }

                string errorDescription = $"Errors quantity: {_errorsDescriptions.Count}.";

                for (var i = 0; i < _errorsDescriptions.Count; i++)
                {
                    errorDescription += $"\r\n{i + 1}. {_errorsDescriptions[i]}";
                }

                return errorDescription;
            }
        }
    }
}
