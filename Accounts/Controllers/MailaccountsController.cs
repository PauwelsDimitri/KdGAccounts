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
using Microsoft.AspNetCore.Http;

namespace Accounts.Controllers
{
    [Authorize(Roles = "Mailaccount, Account")]
    public class MailaccountsController : Controller
    {
        private readonly KDGIDENTITYContext _context;
        private readonly KDGVIEWSContext _vcontext;
        private Log _log;

        public MailaccountsController(KDGIDENTITYContext context, KDGVIEWSContext vcontext)
        {
            _context = context;
            _vcontext = vcontext;
            _log = new Log();
        }

        // GET: Mailaccounts
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
            ViewData["RightsGroupSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("rightsgroup_asc") ? "rightsgroup_desc" : "rightsgroup_asc";


            var kdgaccount = from k in _context.Kdgaccount.Include(k => k.AccountTypeNavigation).Include(k => k.RightsGroupNavigation)
                             where k.AccountType == 10
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
                        || s.AccountTypeNavigation.Name.Contains(searchString)
                        || s.RightsGroupNavigation.Name.Contains(searchString));
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
                    kdgaccount = kdgaccount.OrderByDescending(a => a.Active);
                    break;
                case "active_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.Active);
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
                case "aanvrager_desc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.Aanvrager);
                    break;
                case "aanvrager_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.Aanvrager);
                    break;
                case "createdate_desc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.CreateDate);
                    break;
                case "createdate_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.CreateDate);
                    break;
                case "rightsgroup_desc":
                    kdgaccount = kdgaccount.OrderByDescending(a => a.RightsGroupNavigation.Name);
                    break;
                case "rightsgroup_asc":
                    kdgaccount = kdgaccount.OrderBy(a => a.RightsGroupNavigation.Name);
                    break;
                default:
                    kdgaccount = kdgaccount.OrderByDescending(a => a.EmployeeId);
                    break;
            }
            
            return View(await PaginatedList<Kdgaccount>.CreateAsync(kdgaccount.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Mailaccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdgaccount = await _context.Kdgaccount
                .Include(k => k.AccountTypeNavigation)
                .Include(k => k.RightsGroupNavigation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id && m.AccountType == 10);
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

        // GET: Mailaccounts/Create
        public IActionResult Create()
        {
            Kdgaccount kdgaccount = new Kdgaccount();

            Department dep = new Department();
            Domain dom = new Domain();
            Users users = new Users(_context);
            var accounttypes = from a in _context.AccountType.Where(p => p.Id == 10)
                               select a;
            var rightsgroup = from r in _context.KdgrightsGroup.Where(p => p.AccountType == 10 || p.AccountType == 0)
                              select r;

            kdgaccount.AccountType = 10;
            kdgaccount.Facilities = 625;
            kdgaccount.InitialPassword = users.GenerateRandomPasswordGenerate();
            kdgaccount.StartDate = DateTime.Today;
            kdgaccount.EndDate = DateTime.Today.AddYears(5);
            kdgaccount.Ticketnr = "I-xxxxxx";
            kdgaccount.CreateDate = DateTime.Today;
            kdgaccount.CreateBy = User.Identity.Name;

            ViewData["Department"] = new SelectList(dep.GetDepartments());
            ViewData["Domain"] = new SelectList(dom.GetDomains());
            ViewData["AccountType"] = new SelectList(accounttypes, "Id", "Name", 10);
            ViewData["RightsGroup"] = new SelectList(rightsgroup, "Id", "Name");
            ViewData.Model = kdgaccount;
            return View();
        }

        // POST: Mailaccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DisplayName,Department,Domain,AccountName,Email,InitialPassword,HideFromAddressLists,Active,StartDate,EndDate,Ticketnr,Aanvrager,Opmerking,CreateDate,CreateBy")] Kdgaccount kdgaccount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    kdgaccount.AccountType = 10;
                    //kdgaccount.RightsGroup = 0;
                    kdgaccount.Facilities = 625;


                    _context.Add(kdgaccount);
                    await _context.SaveChangesAsync();
                    string createby = " door " + User.Identity.Name;
                    _log.Add("KDG" + kdgaccount.EmployeeId, "Accounts", "Mailaccount aangemaakt in db." + createby, _context);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Onmogelijk om deze mailaccount aan te maken.");
            }
            
            Department dep = new Department();
            Domain dom = new Domain();

            var accounttypes = from a in _context.AccountType.Where(p => p.Id == 10)
                               select a;

            var rightsgroup = from r in _context.KdgrightsGroup.Where(p => p.AccountType == 10 || p.AccountType == 0)
                              select r;

            ViewData["Department"] = new SelectList(dep.GetDepartments());
            ViewData["Domain"] = new SelectList(dom.GetDomains());
            ViewData["AccountType"] = new SelectList(accounttypes, "Id", "Name", kdgaccount.AccountType);
            ViewData["RightsGroup"] = new SelectList(rightsgroup, "Id", "Name", kdgaccount.RightsGroup);
            return View(kdgaccount);
        }

        // GET: Mailaccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdgaccount = await _context.Kdgaccount.FindAsync(id);
            if (kdgaccount == null || kdgaccount.AccountType != 10)
            {
                return NotFound();
            }
            Department dep = new Department();
            Domain dom = new Domain();

            var accounttypes = from a in _context.AccountType.Where(p => p.Id == 10)
                               select a;
            var rightsgroup = from r in _context.KdgrightsGroup.Where(p => p.AccountType == 10 || p.AccountType == 0)
                              select r;

            ViewData["Department"] = new SelectList(dep.GetDepartments());
            ViewData["Domain"] = new SelectList(dom.GetDomains());
            ViewData["AccountType"] = new SelectList(accounttypes, "Id", "Name", kdgaccount.AccountType);
            ViewData["RightsGroup"] = new SelectList(rightsgroup, "Id", "Name", kdgaccount.RightsGroup);
            return View(kdgaccount);
        }

        // POST: Mailaccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? EmployeeId)
        {
            if (EmployeeId == null)
            {
                return NotFound();
            }
            Kdgaccount oldaccount = await _context.Kdgaccount.AsNoTracking()
                .FirstOrDefaultAsync(a => a.EmployeeId == EmployeeId);
            var accountToUpdate = await _context.Kdgaccount.SingleOrDefaultAsync(a => a.EmployeeId == EmployeeId);
            if (await TryUpdateModelAsync<Kdgaccount>(
                accountToUpdate, "",
                a => a.FirstName, a => a.LastName, a => a.DisplayName, a => a.Department, a => a.Domain, 
                a => a.AccountName, a => a.Email, a => a.HideFromAddressLists, a => a.Active, 
                a => a.StartDate, a => a.EndDate, a => a.Ticketnr, a => a.Aanvrager, a => a.Opmerking, a => a.RightsGroup))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    string editby = " door " + User.Identity.Name;
                    _log.EditAccount(oldaccount, accountToUpdate, editby, _context);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Onmogelijk om deze wijzigingen op te slaan.");
                }
            }
            Department dep = new Department();
            Domain dom = new Domain();

            var accounttypes = from a in _context.AccountType.Where(p => p.Id == 10)
                               select a;
            var rightsgroup = from r in _context.KdgrightsGroup.Where(p => p.AccountType == 10 || p.AccountType == 0)
                              select r;

            ViewData["Department"] = new SelectList(dep.GetDepartments());
            ViewData["Domain"] = new SelectList(dom.GetDomains());
            ViewData["AccountType"] = new SelectList(accounttypes, "Id", "Name", accountToUpdate.AccountType);
            ViewData["RightsGroup"] = new SelectList(rightsgroup, "Id", "Name", accountToUpdate.RightsGroup);
            return View(accountToUpdate);
        }


        private bool KdgaccountExists(int id)
        {
            return _context.Kdgaccount.Any(e => e.EmployeeId == id);
        }
    }
}
