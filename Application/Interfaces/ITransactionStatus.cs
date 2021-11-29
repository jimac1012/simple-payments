namespace Application.Interfaces
{
    public interface ITransactionStatus
    {
        bool IsSuccess { get; }

        string Message { get; }
    }
}
