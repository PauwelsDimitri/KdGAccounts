using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public partial class ADGroup
    {
        public string accountname { get; set; }
        public string cn { get; set; }
        public string name { get; set; }



        public ADGroup(string cn,string Name, string SamAccountName)
        {
            this.accountname = SamAccountName;
            this.cn = cn;
            this.name = Name;
        }
    }
}
