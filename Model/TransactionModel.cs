using System;

namespace Model
{
    public class TransactionModel
    {
        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public decimal? TransactionFee { get; set; }

        public string TransactionType { get; set; }

        public string Status { get; set; }

        public string Note { get; set; }
    }
}
