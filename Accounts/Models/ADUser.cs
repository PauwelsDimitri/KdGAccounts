using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public partial class ADUser
    {
        public string name { get; set; }
        public string accountname { get; set; }
        public string description { get; set; }
        public DateTime lastLogonTimestamp { get; set; }
        public DateTime expiredate { get; set; }
        public bool expired { get; set; }


        public ADUser(string cn, string SamAccountName, string Description, DateTime LastLogonTimestamp, DateTime AccountExpires, bool Expired)
        {
            this.name = cn;
            this.accountname = SamAccountName;
            this.description = Description;
            this.lastLogonTimestamp = LastLogonTimestamp;
            this.expiredate = AccountExpires;
            this.expired = Expired;
        }
    }
}
