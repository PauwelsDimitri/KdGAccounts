using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Models
{
    public partial class AccountType
    {
        public AccountType()
        {
            Kdgaccount = new HashSet<Kdgaccount>();
        }

        public int Id { get; set; }

        [Display(Name = "Accounttype")]
        public string Name { get; set; }
        [Display(Name = "E-mail extention")]
        public string EmailExt { get; set; }
        [Display(Name = "Account extention")]
        public string AccountExt { get; set; }
        [Display(Name = "Verwijderen na (maanden)")]
        public int DeleteAfter { get; set; }
        [Display(Name = "Aanmaken toegestaan")]
        public bool CreateAllowed { get; set; }

        public ICollection<Kdgaccount> Kdgaccount { get; set; }
        public ICollection<KdgrightsGroup> KdgrightsGroup { get; set; }
        public ICollection<AccountTypeRole> AccountTypeRole { get; set; }
    }
}
