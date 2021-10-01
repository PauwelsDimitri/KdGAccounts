using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public class Types
    {
        public IEnumerable<string> GetTypes()
        {
            return new List<string>
            {
                "bool","datetime","int","string"
            };
        }
    }
}
