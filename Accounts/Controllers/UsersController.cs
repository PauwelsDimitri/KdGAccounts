using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Accounts.Models;

namespace Accounts.Controllers
{
    public class UsersController : Controller
    {
        private readonly KDGIDENTITYContext _context;

        public UsersController(KDGIDENTITYContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAccountExt(int? Id)
        {
            if (Id == null)
            {
                return BadRequest();
            }
            AccountType AType = _context.AccountType.Find(Id);
            if (AType == null)
            {
                return NotFound();
            }
            if (AType.AccountExt == null)
                AType.AccountExt = "";
            if (AType.EmailExt == null)
                AType.EmailExt = "";
            return Json(AType);
        }

        public IActionResult GetADGroup(string accountname, string domain)
        {
            if (accountname == null || domain == null)
            {
                return BadRequest();
            }
            AD _ad = new AD();
            bool exist = _ad.ADGroupExist(accountname, domain);
            ADGroup aDGroup = new ADGroup("", "", "");
            if (exist)
            {
                aDGroup = _ad.GetADGroup(accountname, domain);
            }

            return Json(aDGroup);

        }

        public IActionResult GetADGroups(string term, string domain)
        {
            if (term == null)
            {
                return BadRequest();
            }

            AD _ad = new AD();
            return Json(_ad.GetADGroups(term, domain, 20).Select(a => a.accountname));
        }


        public IActionResult VerifyAccount(string accountname, int? accounttype, int? employeeid)
        {
            if (accountname == null || accounttype == null)
            {
                return BadRequest();
            }
            Users auser = new Users(_context);
            bool exist = auser.AccountExists(accountname);
            string accountext = auser.GetAccountExt(accounttype);
            if (employeeid != null)
            {
                if (auser.IsUserAccount(accountname, employeeid))
                {
                    return Json(data: true);
                }
            }


            if (!(string.IsNullOrEmpty(accountext) || accountname.Contains(accountext)))
            {
                return Json(data: $"Account moet {accountext} bevatten.");
            }

            if (exist)
            {
                return Json(data: $"Er bestaat reeds een account {accountname}.");
            }

            return Json(data: true);
        }

        public IActionResult VerifyADGroup(string accountname, string domain)
        {
            if (accountname == null || domain == null)
            {
                return BadRequest();
            }
            AD _ad = new AD();
            bool exist = _ad.ADGroupExist(accountname, domain);

            if (!exist)
            {
                return Json(data: $"{accountname} bestaat niet in het {domain} domein.");
            }
            return Json(data: true);
        }

        public IActionResult VerifyEmail(string email, string domain, int? employeeid, int? accounttype)
        {
            if (email == null || domain == null || accounttype == null)
            {
                return BadRequest();
            }
            Users auser = new Users(_context);
            bool exist = auser.EmailExists(email);
            string emailext = auser.GetEmailExt(accounttype);
            string atdomain = emailext + "@kdg.be";

            if (employeeid != null)
            {
                if (auser.IsUserEmail(email, employeeid))
                {
                    return Json(data: true);
                }
            }

            if (domain == "STUDENT")
            {
                atdomain = emailext + "@student.kdg.be";
            }

            if (!email.EndsWith(atdomain))
            {
                return Json(data: $"E-mailadres moet eindigen op {atdomain}.");
            }

            if (exist)
            {
                return Json(data: $"Er bestaat reeds een e-mailadres {email}.");
            }

            return Json(data: true);

        }

        public IActionResult VerifyPassword(string initialpassword, string firstname, string lastname)
        {
            if (string.IsNullOrEmpty(initialpassword))
                return Json(data: $"Paswoord moet een waarde bevatten.");

            if (initialpassword.Length < 8)
            {
                return Json(data: $"Paswoord moet minstens 8 karakters lang zijn.");
            }
            var hasUpper = 0;
            var hasLower = 0;
            var hasNumber = 0;
            var hasNonAlpha = 0;
            if (initialpassword.Any(char.IsUpper)) { hasUpper = 1; }
            if (initialpassword.Any(char.IsLower)) { hasLower = 1; }
            if (initialpassword.Any(char.IsNumber)) { hasNumber = 1; }
            if (!initialpassword.All(char.IsLetterOrDigit)) { hasNonAlpha = 1; }

            if (hasUpper + hasLower + hasNumber + hasNonAlpha < 3)
            {
                return Json(data: $"Paswoord moet minstens 3 verschillende type karakters bevatten.");
            }

            initialpassword = initialpassword.ToLower();


            if (!string.IsNullOrEmpty(firstname))
            {
                if (initialpassword.Contains(firstname.ToLower()))
                    return Json(data: $"Paswoord mag geen deel van de voornaam en/of achternaam bevatten");
            }

            if (!string.IsNullOrEmpty(lastname))
            {
                if (initialpassword.Contains(lastname.ToLower()))
                    return Json(data: $"Paswoord mag geen deel van de voornaam en/of achternaam bevatten");
            }

            return Json(data: true);
        }
    }
}