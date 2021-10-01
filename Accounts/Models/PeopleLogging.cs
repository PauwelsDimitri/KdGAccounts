using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Models
{
    public partial class PeopleLogging
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string UniqueID { get; set; }

        [StringLength(20)]
        public string App { get; set; }

        [StringLength(200)]
        public string Event { get; set; }

        public DateTime Logdate { get; set; }

    }
}
