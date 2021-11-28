namespace Web.Models.Base
{
    public class GetAccountTransactions
    {
        public int UserId { get; set; }

        public int AccountId { get; set; }
    }

    public class TransactionViewModel
    {
        public int UserId { get; set; }

        public int AccountId { get; set; }

        public decimal Amount { get; set; }

        public decimal TransactionFee { get; set; }

        public string Note { get; set; }
    }
}