using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
