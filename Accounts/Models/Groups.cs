using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public class Groups
    {
        private readonly KDGIDENTITYContext _context;

        public Groups(KDGIDENTITYContext context)
        {
            _context = context;
        }

        public int GetRightsGroupMaxEndDate(int groupid)
        {
            var RGroup = _context.KdgrightsGroup.Where(g => g.Id == groupid);
            return RGroup.FirstOrDefault().MaxEndDate;
        }

    }
}
