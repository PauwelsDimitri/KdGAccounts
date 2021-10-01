using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public class Department
    {
        public IEnumerable<string> GetDepartments()
        {
            return new List<string>
            {
               "AD","COMM","HR","HSA","HSD","INFRA","LEO","MIT","MNT","OC","SLA","STUVO","WEG","WET","WOG"
            };
        }
    }
}
