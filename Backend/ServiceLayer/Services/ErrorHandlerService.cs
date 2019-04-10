using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class ErrorHandlerService : IErrorHandlerService
    {
        private int errorCount;
        private string errorMsg;

        public ErrorHandlerService()
        {
            errorCount = 0;
        }

        /// <summary>
        /// Method ResetErrorCount resets the error counter to 0 and stores the error
        /// message which will be sent to the System Admin to fix.
        /// </summary>
        /// <param name="errorMessage">error message in string format</param>
        public void ResetErrorCount(string errorMessage)
        {
            errorMsg = errorMessage;
            errorCount = 0;
        }

        /// <summary>
        /// Method ContactSystemAdmin informs the system admin that there is an issue
        /// in the backend that must be taken care of.
        /// </summary>
        public string ContactSystemAdmin()
        {
            return errorMsg;
        }

        public int GetErrorOcurrenceCount()
        {
            return errorCount;
        }

        /// <summary>
        /// Method IncrementErrorOccurrenceCount increments the error counter when
        /// it is called. Should the error counter reach 100 or more, the error message
        /// is passed to ContactSystemAdmin to let them know an error is occurring at 
        /// this area of the code.
        /// </summary>
        /// <param name="errorMessage">The exception caught as a string</param>
        public void IncrementErrorOccurrenceCount(string errorMessage)
        {
            errorCount++;
            if(IsErrorCounterAtMax() == true)
            {
                ResetErrorCount(errorMessage);
            }
        }

        /// <summary>
        /// Method IsErrorCounterAtMax checks if the error counter has reached a 
        /// value of 100 or more.
        /// </summary>
        /// <returns>Returns true or false if the error counter has reached 100 or more</returns>
        public bool IsErrorCounterAtMax()
        {
            bool isAtMax = false;
            if (errorCount >= 100)
            {
                isAtMax = true;
            }
            return isAtMax;
        }
    }
}
