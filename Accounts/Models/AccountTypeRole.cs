using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public class AccountTypeRole
    {
            public int Id { get; set; }
        [Display(Name = "Accounttype")]
        public int AccountType { get; set; }
        [Display(Name = "Rol")]
        public string Role { get; set; }

            public AccountType AccountTypeNavigation { get; set; }
    }
}
