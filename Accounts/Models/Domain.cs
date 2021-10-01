using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public class Domain
    {
        public IEnumerable<string> GetDomains()
        {
            return new List<string>
            {
                "ADMIN","STUDENT"
            };
        }
    }
}
