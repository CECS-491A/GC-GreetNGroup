using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class ErrorHandlerService : IErrorHandlerService
    {
        private int errorCount;

        public ErrorHandlerService()
        {
            errorCount = 0;
        }

        /// <summary>
        /// Method ContactSystemAdmin informs the system admin that there is an issue
        /// in the backend that must be taken care of. The error counter is reset to 0
        /// once the system admin has been notified of the issue.
        /// </summary>
        public void ContactSystemAdmin(string errorMessage)
        {
            //TODO: Contact system admin
            errorCount = 0;
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
                ContactSystemAdmin(errorMessage);
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
