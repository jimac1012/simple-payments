using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TransactionalModel
    {
        public int UserId { get; set; }

        public int AccountId { get; set; }

        public decimal Amount { get; set; }

        public decimal TransactionFee { get; set; }

        public string Note { get; set; }
    }
}
