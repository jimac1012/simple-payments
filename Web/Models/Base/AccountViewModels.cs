namespace Web.Models.Base
{
    public class AccountCreationBindingModel
    {
        public int UserId { get; set; }

        public string AccountName { get; set; }
    }

    public class GetUserAccountList
    {
        public int UserId { get; set; }
    }

    public class AccountTransaction
    {
        public int UserId { get; set; }

        public int AccountId { get; set; }

        public decimal Amount { get; set; }
    }
}