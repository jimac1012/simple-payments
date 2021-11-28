using System;

namespace Model
{
    public class TransactionModel
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public decimal? TransactionFee { get; set; }

        public string Status { get; set; }

        public string Note { get; set; }
    }
}
