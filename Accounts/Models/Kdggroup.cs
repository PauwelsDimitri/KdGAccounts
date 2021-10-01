using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Models
{
    public partial class Kdggroup
    {
        public int Id { get; set; }
        public int RGId { get; set; }

        [Display(Name = "Account")]
        [StringLength(50)]
        [MaxLength(50)]
        [Remote(action: "VerifyADGroup", controller: "Users", AdditionalFields = "Domain")]
        public string AccountName { get; set; }

        [Display(Name = "Weergavenaam")]
        [StringLength(50)]
        [MaxLength(50)]
        [Required(ErrorMessage = "Weergavenaam is verplicht.")]
        public string DisplayName { get; set; }

        [Display(Name = "Domein")]
        [StringLength(10)]
        [Required(ErrorMessage = "Domein is verplicht.")]
        public string Domain { get; set; }

        public KdgrightsGroup KdgRightsGroupNavigation { get; set; }
    }
}
