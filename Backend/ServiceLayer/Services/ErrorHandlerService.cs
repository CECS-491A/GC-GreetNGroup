
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

        public void IncrementErrorOccurrenceCount(string errorMessage)
        {
            errorCount++;
            if(IsErrorCounterAtMax() == true)
            {
                ContactSystemAdmin(errorMessage);
            }
        }

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
