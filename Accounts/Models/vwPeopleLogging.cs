using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Models
{
    public partial class vwPeopleLogging
    {
        public string UniqueID { get; set; }
        public string DisplayName { get; set; }
        public string App { get; set; }
        public string Event { get; set; }
        public System.DateTime Logdate { get; set; }
    }
}
