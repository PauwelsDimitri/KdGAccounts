using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Models
{
    public partial class KdgrightsGroup
    {
        public KdgrightsGroup()
        {
            Kdgaccount = new HashSet<Kdgaccount>();
            Kdggroup = new HashSet<Kdggroup>();
            Kdgrightsgrouppropertie = new HashSet<KdgrightsGroupPropertie>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Rechtengroep")]
        [Required(ErrorMessage = "Rechtengroep is verplicht.")]
        public string Name { get; set; }
        [Display(Name = "Max. Einddatum( in weken)")]
        public int MaxEndDate { get; set; }

        [Display(Name = "Accounttype")]
        public int AccountType { get; set; }

        public ICollection<Kdgaccount> Kdgaccount { get; set; }
        public ICollection<Kdggroup> Kdggroup { get; set; }
        public ICollection<KdgrightsGroupPropertie> Kdgrightsgrouppropertie { get; set; }

        public AccountType AccountTypeNavigation { get; set; }
    }
}
