using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public partial class Logging
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string App { get; set; }

        [StringLength(200)]
        public string Event { get; set; }

        public DateTime Logdate { get; set; }
    }
}
