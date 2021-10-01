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
    [Authorize(Roles = "Admin")]
    public class AccountTypeRolesController : Controller
    {
        private readonly KDGIDENTITYContext _context;
        private Log _log;

        public AccountTypeRolesController(KDGIDENTITYContext context)
        {
            _context = context;
            _log = new Log();
        }

        // GET: AccountTypeRoles
        public async Task<IActionResult> Index(string sortOrder, int? page, int? pagesize)
        {
            if (sortOrder != null)
            {
                HttpContext.Session.SetString("sortOrder", sortOrder);
            }
            if (HttpContext.Session.GetString("sortOrder") != null && sortOrder == null)
            {
                sortOrder = HttpContext.Session.GetString("sortOrder");
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
            ViewData["AccountTypeSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("accounttype_asc") ? "accounttype_desc" : "accounttype_asc";
            ViewData["RoleSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("role_asc") ? "role_desc" : "role_asc";

            var accounttyperoles = from ar in _context.AccountTypeRole.Include(a => a.AccountTypeNavigation)
                                   select ar;
            int pageSize = (pagesize ?? 25);
            int pageNumber = (page ?? 1);

            HttpContext.Session.SetInt32("page", pageNumber);
            HttpContext.Session.SetInt32("pagesize", pageSize);

            switch (sortOrder)
            {
                case "accounttype_desc":
                    accounttyperoles = accounttyperoles.OrderByDescending(a => a.AccountTypeNavigation.Name);
                    break;
                case "accounttype_asc":
                    accounttyperoles = accounttyperoles.OrderBy(a => a.AccountTypeNavigation.Name);
                    break;
                case "role_desc":
                    accounttyperoles = accounttyperoles.OrderByDescending(a => a.Role);
                    break;
                case "role_asc":
                    accounttyperoles = accounttyperoles.OrderBy(a => a.Role);
                    break;
                default:
                    accounttyperoles = accounttyperoles.OrderByDescending(a => a.AccountTypeNavigation.Name);
                    break;
            }

            return View(await PaginatedList<AccountTypeRole>.CreateAsync(accounttyperoles.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: AccountTypeRoles/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //nieuwe role
            AccountTypeRole accounttyperole = new AccountTypeRole();
            accounttyperole.AccountType = (int)id;

            //accounttype details
            var accounttype = from a in _context.AccountType.Where(i => i.Id == id)
                              select a;

            //lijst met roles
            var roles = from r in _context.AccountTypeRole.Where(i => i.AccountType == id)
                        select r;

            if (accounttype == null)
            {
                return NotFound();
            }

            ViewData["AccountType"] = accounttype.FirstOrDefault();
            ViewData["Roles"] = roles;
            return View(accounttyperole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountType,Role")] AccountTypeRole accounttyperole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accounttyperole);
                await _context.SaveChangesAsync();
                var accounttypename = (from a in _context.AccountType.Where(p => p.Id == accounttyperole.AccountType)
                                       select a).FirstOrDefault().Name;
                var log = "Role: " + accounttyperole.Role + " AccountType: " + accounttypename;
                _log.AddConfig("Account", "AccountTypeRoles " + log + " aangemaakt door " + User.Identity.Name, _context);

                return RedirectToAction("Create", "AccountTypeRoles", new { id = accounttyperole.AccountType });
            }

            //accounttype details
            var accounttype = from a in _context.AccountType.Where(i => i.Id == accounttyperole.AccountType)
                              select a;

            //lijst met roles
            var roles = from r in _context.AccountTypeRole.Where(i => i.AccountType == accounttyperole.AccountType)
                        select r;

            if (accounttype == null)
            {
                return NotFound();
            }

            ViewData["AccountType"] = accounttype.FirstOrDefault();
            ViewData["Roles"] = roles;
            return View(accounttyperole);
        }
        // GET: AccountTypeRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounttyperole = await _context.AccountTypeRole.FindAsync(id);
            if (accounttyperole == null)
            {
                return NotFound();
            }
            //accounttype details
            var accounttype = from a in _context.AccountType.Where(i => i.Id == id)
                              select a;

            //lijst met roles
            var roles = from r in _context.AccountTypeRole.Where(i => i.AccountType == id)
                        select r;

            if (accounttype == null)
            {
                return NotFound();
            }

            ViewData["AccountType"] = accounttype.FirstOrDefault();
            ViewData["Roles"] = roles;
            return View(accounttyperole);
        }

        // POST: AccountTypeRoles/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            AccountTypeRole oldrole = await _context.AccountTypeRole.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == Id);
            var roletoupdate = await _context.AccountTypeRole.SingleOrDefaultAsync(p => p.Id == Id);
            if (await TryUpdateModelAsync<AccountTypeRole>(
                roletoupdate, "", g => g.Role))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    string editby = " door " + User.Identity.Name;
                    _log.EditAccountTypeRole(oldrole, roletoupdate, editby, _context);
                    return RedirectToAction("Create", "AccountTypeRoles", new { id = oldrole.AccountType });

                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Onmogelijk om deze wijzigingen op te slaan.");
                }
            }
            //accounttype details
            var accounttype = from a in _context.AccountType.Where(i => i.Id == oldrole.AccountType)
                              select a;

            //lijst met roles
            var roles = from r in _context.AccountTypeRole.Where(i => i.AccountType == oldrole.AccountType)
                        select r;

            if (accounttype == null)
            {
                return NotFound();
            }

            ViewData["AccountType"] = accounttype.FirstOrDefault();
            ViewData["Roles"] = roles;
            return View(roletoupdate);

        }

        // GET: AccountTypeRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounttyperole = await _context.AccountTypeRole
                .Include(k => k.AccountTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounttyperole == null)
            {
                return NotFound();
            }
            return View(accounttyperole);
        }

        // POST: AccountTypeRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accounttyperole = await _context.AccountTypeRole.FindAsync(id);
            var accounttypename = (from a in _context.AccountType.Where(p => p.Id == accounttyperole.AccountType)
                                   select a).FirstOrDefault().Name;

            _context.AccountTypeRole.Remove(accounttyperole);
            var accounttype = accounttyperole.AccountType;
            var log = "Role: " + accounttyperole.Role + " AccountType: " + accounttypename;
            await _context.SaveChangesAsync();
            _log.AddConfig("Account", "AccountTypeRoles " + log + " verwijderd door " + User.Identity.Name, _context);
            
            return RedirectToAction("Create", "AccountTypeRoles", new { id = accounttyperole.AccountType });

        }

        // GET: AccountTypeRoles/Delete/5
        public async Task<IActionResult> DeleteFromIndex(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounttyperole = await _context.AccountTypeRole
                .Include(k => k.AccountTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounttyperole == null)
            {
                return NotFound();
            }
            return View(accounttyperole);
        }

        // POST: AccountTypeRoles/Delete/5
        [HttpPost, ActionName("DeleteFromIndex")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedFromIndex(int id)
        {
            var accounttyperole = await _context.AccountTypeRole.FindAsync(id);
            var accounttypename = (from a in _context.AccountType.Where(p => p.Id == accounttyperole.AccountType)
                                   select a).FirstOrDefault().Name;

            _context.AccountTypeRole.Remove(accounttyperole);
            var accounttype = accounttyperole.AccountType;
            var log = "Role: " + accounttyperole.Role + " AccountType: " + accounttypename;
            await _context.SaveChangesAsync();
            _log.AddConfig("Account", "AccountTypeRoles " + log + " verwijderd door " + User.Identity.Name, _context);

            return RedirectToAction("Index", "AccountTypeRoles");

        }

    }
}