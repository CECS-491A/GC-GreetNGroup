namespace Gucci.ServiceLayer.Interface
{
    public interface IErrorHandlerService
    {
        int GetErrorOcurrenceCount();
        void IncrementErrorOccurrenceCount(string errorMessage);
        void ResetErrorCount(string errorMessage);
        string ContactSystemAdmin();
        bool IsErrorCounterAtMax();
    }
}
