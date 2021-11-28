namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Transaction
    {
        public Transaction()
        {
            TransactionDate = DateTime.UtcNow;
            DateCreated = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal? TransactionFee { get; set; }

        [StringLength(50)]
        public string Note { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime TransactionDate { get; set; }

        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
