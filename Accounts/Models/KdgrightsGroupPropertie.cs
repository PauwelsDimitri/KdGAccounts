using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public class KdgrightsGroupPropertie
    {
        public int Id { get; set; }
        public int RGId {get; set;}

        [Display(Name = "Veldnaam")]
        [StringLength(50)]
        [MaxLength(50)]
        [Required(ErrorMessage = "Veldnaam is verplicht.")]
        public string Field { get; set; }

        [Display(Name = "Type")]
        [StringLength(10)]
        [Required(ErrorMessage = "Type is verplicht.")]
        public string Type { get; set; }

        [Display(Name = "Data")]
        [StringLength(50)]
        [MaxLength(50)]
        [Required(ErrorMessage = "Data is verplicht.")]
        public string Value { get; set; }

        public KdgrightsGroup KdgRightsGroupNavigation { get; set; }

    }
}
