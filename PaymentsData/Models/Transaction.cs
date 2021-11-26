using System;

namespace PaymentsWeb.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }

        public decimal TransactionFee { get; set; }

        public string Note { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime TransactionDate { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}