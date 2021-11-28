namespace Application
{
    public class TransactionStatus
    {
        public TransactionStatus()
        {
            IsSuccess = false;
        }

        public bool IsSuccess { get; private set; }

        public string Message { get; private set; }

        public void UpdateMessage(string message)
        {
            Message = message;
        }

        public void TransactionSuccess()
        {
            IsSuccess = true;
        }
    }
}
