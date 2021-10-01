using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Models
{

    public partial class Kdgaccount
    {
        public int EmployeeId { get; set; }

        [StringLength(50)]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Achternaam")]
        [Required(ErrorMessage = "Achternaam is verplicht.")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Weergavenaam")]
        [Required(ErrorMessage = "Weergavenaam is verplicht.")]
        public string DisplayName { get; set; }

        [StringLength(10)]
        [Display(Name = "Dienst")]
        public string Department { get; set; }

        [StringLength(20)]
        [Display(Name = "Domein")]
        public string Domain { get; set; }

        [Display(Name = "Accounttype")]
        public int AccountType { get; set; }

        [Display(Name = "Rechtengroep")]
        public int RightsGroup { get; set; }

        [StringLength(50)]
        [Display(Name = "Account")]
        [Remote(action: "VerifyAccount", controller: "Users", AdditionalFields = "AccountType, EmployeeId")]
        public string AccountName { get; set; }

        [StringLength(50)]
        [Display(Name = "E-mail")]
        [RegularExpression(@"\b[a-zA-Z0-9._%+-]+@(student.|)kdg.be\b", ErrorMessage = "E-mail is ongeldig!")]
        [Remote(action: "VerifyEmail", controller: "Users", AdditionalFields = "AccountType, Domain, EmployeeId")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Privé e-mail")]
        [RegularExpression(@"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}\b", ErrorMessage = "Privé e-mail is ongeldig!")]
        public string PrivateMail { get; set; }

        [StringLength(50)]
        [Display(Name = "Telefoon")]
        public string Telephone { get; set; }

        [StringLength(50)]
        [Display(Name = "Office locatie")]
        public string Office { get; set; }

        [StringLength(50)]
        [Display(Name = "Mifareserial")]
        public string MifareSerial { get; set; }

        [StringLength(50)]
        [Display(Name = "Initiëel paswoord")]
        [Remote(action: "VerifyPassword", controller: "Users", AdditionalFields = "FirstName, LastName")]
        public string InitialPassword { get; set; }

        [Display(Name = "Faciliteiten")]
        public long? Facilities { get; set; }

        [Display(Name = "Onzichtbaar in adresboek")]
        public bool HideFromAddressLists { get; set; }

        [Display(Name = "Actief")]
        [UIHint("Bool")]
        public bool Active { get; set; }

        [Display(Name = "Startdatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Startdatum is verplicht.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Einddatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Einddatum is verplicht.")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Ticketnr.")]
        [RegularExpression(@"(^[I][-][1-9][0-9]{5}|nieuw|extern)$", ErrorMessage = "Ticket nummer is ongeldig! (I-xxxxxx, nieuw of extern)")]
        [Required(ErrorMessage = "Tickectnummer (I-xxxxxx) is verplicht.")]
        public string Ticketnr { get; set; }

        [Display(Name = "Aanvrager e-mail")]
        [Required(ErrorMessage = "Aanvrager is verplicht.")]
        //[RegularExpression(@"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}\b", ErrorMessage = "Aanvrager e-mail is ongeldig!")]
        [RegularExpression(@"^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$", ErrorMessage = "Aanvrager e-mail is ongeldig!")]
        public string Aanvrager { get; set; }

        [DataType(DataType.MultilineText)]
        public string Opmerking { get; set; }

        [Display(Name = "Aanmaakdatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Aanmaakdatum is verplicht.")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Aangemaakt door")]
        [Required(ErrorMessage = "Aangemaakt door is verplicht.")]
        public string CreateBy { get; set; }

        public AccountType AccountTypeNavigation { get; set; }
        public KdgrightsGroup RightsGroupNavigation { get; set; }
    }
}
