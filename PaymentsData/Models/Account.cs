﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentsWeb.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public DateTime DateCreated { get; set; }

        public decimal Balance { get; set; }

        public string UserId { get; set; }

        public User User { get; set; } 
    }
}