namespace Model
{
    public class AccountModel
    {
        public int Id { get; set; }

        public string AccountName { get; set; }

        public string Type { get; set; }

        public decimal Balance { get; set; }

        public int UserId { get; set; }
    }
}
