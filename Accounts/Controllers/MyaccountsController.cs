using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Accounts.Models;
using Accounts.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace Accounts.Controllers
{
    [Authorize]
    public class MyaccountsController : Controller
    {
        private readonly KDGIDENTITYContext _context;
        private readonly KDGVIEWSContext _vcontext;
        private Log _log;

        public MyaccountsController(KDGIDENTITYContext context, KDGVIEWSContext vcontext)
        {
            _context = context;
            _vcontext = vcontext;
            _log = new Log();
        }

        // GET: Myaccounts
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page, int? pagesize)
        {
            if (sortOrder != null)
            {
                HttpContext.Session.SetString("sortOrder", sortOrder);
            }
            if (HttpContext.Session.GetString("sortOrder") != null && sortOrder == null)
            {
                sortOrder = HttpContext.Session.GetString("sortOrder");
            }
            if (HttpContext.Session.GetString("currentFilter") != null && currentFilter == null)
            {
                currentFilter = HttpContext.Session.GetString("currentFilter");
            }
            if (HttpContext.Session.GetString("searchString") != null && searchString == null)
            {
                searchString = HttpContext.Session.GetString("currentFilter");
            }
            if (HttpContext.Session.GetInt32("page") != null && page == null)
            {
                page = HttpContext.Session.GetInt32("page");
            }
            if (HttpContext.Session.GetInt32("pagesize") != null && page == null)
            {
                page = HttpContext.Session.GetInt32("pagesize");
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["PageSize"] = pagesize;
            ViewData["DisplaynameSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("displayname_asc") ? "displayname_desc" : "displayname_asc";
            ViewData["DomainSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("domain_asc") ? "domain_desc" : "domain_asc";
            ViewData["ActiveSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("active_asc") ? "active_desc" : "active_asc";
            ViewData["StartDateSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("startdate_asc") ? "startdate_desc" : "startdate_asc";
            ViewData["EndDateSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("enddate_asc") ? "enddate_desc" : "enddate_asc";
            ViewData["AanvragerSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("aanvrager_asc") ? "aanvrager_desc" : "aanvrager_asc";
            ViewData["CreateDateSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("createdate_asc") ? "createdate_desc" : "createdate_asc";
            ViewData["AccountTypeSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("accounttype_asc") ? "accounttype_desc" : "accounttype_asc";

            var kdgaccount = from k in _context.Kdgaccount.Include(k => k.AccountTypeNavigation).Include(k => k.RightsGroupNavigation)
                             where k.Aanvrager.Contains(User.Identity.Name)
                             select k;

            if (searchString != null)
            {
                page = 1;
                pagesize = 25;
                HttpContext.Session.SetString("currentFilter", searchString);
            }
            else
            {
                searchString = currentFilter;
            }
            int pageSize = (pagesize ?? 25);
            int pageNumber = (page ?? 1);

            ViewData["CurrentFilter"] = searchString;
            HttpContext.Session.SetInt32("page", pageNumber);
            HttpContext.Session.SetInt32("pagesize", pageSize);

            if (!String.IsNullOrEmpty(searchString))
            {
                kdgaccount = kdgaccount.Where(s => s.FirstName.Contains(searchString)
                        || s.LastName.Contains(searchString)
                        || s.DisplayName.Contains(searchString)
                        || s.Ticketnr.Contains(searchString)
                        || s.Email.Contains(searchString)
                        || s.Aanvrager.Contains(searchString)
                        || s.AccountTypeNavigation.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "displayname_desc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.DisplayName);
                    break;
                case "displayname_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.DisplayName);
                    break;
                case "domain_desc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.Domain);
                    break;
                case "domain_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.Domain);
                    break;
                case "active_desc":
                    kdgaccount = kdgaccount.OrderBy(a => a.Active);
                    break;
                case "active_asc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.Active);
                    break;
                case "startdate_desc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.StartDate);
                    break;
                case "startdate_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.StartDate);
                    break;
                case "enddate_desc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.EndDate);
                    break;
                case "enddate_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.EndDate);
                    break;
                case "createdate_desc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.CreateDate);
                    break;
                case "createdate_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.CreateDate);
                    break;
                case "accounttype_desc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.AccountTypeNavigation.Name);
                    break;
                case "accounttype_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.AccountTypeNavigation.Name);
                    break;
                default:
                    kdgaccount = kdgaccount.OrderByDescending(a => a.EmployeeId);
                    break;
            }
            
            return View(await PaginatedList<Kdgaccount>.CreateAsync(kdgaccount.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Myaccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdgaccount = await _context.Kdgaccount
                .Include(k => k.AccountTypeNavigation)
                .Include(k => k.RightsGroupNavigation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id && m.Aanvrager.Contains(User.Identity.Name));
            if (kdgaccount == null)
            {
                return NotFound();
            }
            AD aD = new AD();
            var aDUser = aD.GetADUser(kdgaccount.AccountName, kdgaccount.Domain);
            var LastLogon = "never";
            var AccountExpires = "never";
            DateTime date = new DateTime(1900, 1, 1, 0, 0, 0);
            if (aDUser.lastLogonTimestamp > date)
            {
                LastLogon = aDUser.lastLogonTimestamp.ToString();
            }
            if (aDUser.expiredate > date)
            {
                AccountExpires = aDUser.expiredate.ToString();
            }

            ViewData["LastLogon"] = LastLogon;
            ViewData["AccountExpires"] = AccountExpires;
            return View(kdgaccount);
        }
        // GET: Accounts/Log/KDG900000
        public ActionResult Log(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var uniqueid = "KDG" + id;
            var peoplelog = _vcontext.vwPeopleLogging.Where(p => p.UniqueID == uniqueid).OrderByDescending(p => p.Logdate);

            if (peoplelog == null)
            {
                return NotFound();
            }

            var account = _context.Kdgaccount.Where(a => a.EmployeeId == id).FirstOrDefault();
            ViewData["eid"] = id;
            ViewData["DisplayName"] = account.DisplayName;
            return View(peoplelog);
        }

        // GET: Myaccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdgaccount = await _context.Kdgaccount.FirstOrDefaultAsync(m => m.EmployeeId == id && m.Aanvrager.Contains(User.Identity.Name));

            if (kdgaccount == null)
            {
                return NotFound();
            }

            return View(kdgaccount);
        }

        // POST: Myaccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>EditPost(int? EmployeeId, DateTime EndDate)
        {
            if (EmployeeId == null)
            {
                return NotFound();
            }
            var weeks = _context.Kdgaccount.AsNoTracking().Include(k => k.RightsGroupNavigation)
                .Where(e => e.EmployeeId == EmployeeId).FirstOrDefault().RightsGroupNavigation.MaxEndDate;
            if (EndDate > DateTime.Now.AddDays(weeks * 7))
            {
                ModelState.AddModelError("EndDate", "Einddatum mag max. " + weeks + " week(en) in de toekomst liggen!");

            }
            Kdgaccount oldaccount = await _context.Kdgaccount.AsNoTracking()
                .FirstOrDefaultAsync(a => a.EmployeeId == EmployeeId);
            
            var accountToUpdate = await _context.Kdgaccount.SingleOrDefaultAsync(a => a.EmployeeId == EmployeeId);
            
            if (await TryUpdateModelAsync<Kdgaccount>(
                accountToUpdate,"",
                a => a.Active, a => a.HideFromAddressLists, a => a.EndDate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    string editby = " door " + User.Identity.Name;
                    _log.EditAccount(oldaccount, accountToUpdate, editby, _context);
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Onmogelijk om de wijzigingen op te slaan.");
                }
            }

            return View(accountToUpdate);
        }

        private bool KdgaccountExists(int id)
        {
            return _context.Kdgaccount.Any(e => e.EmployeeId == id);
        }
    }
}
