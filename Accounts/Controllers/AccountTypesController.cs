using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Accounts.Custom;
using Accounts.Models;
using Microsoft.AspNetCore.Authorization;


namespace Accounts.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountTypesController : Controller
    {
        private readonly KDGIDENTITYContext _context;
        private Log _log;

        public AccountTypesController(KDGIDENTITYContext context)
        {
            _context = context;
            _log = new Log();
        }

        // GET: AccountTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountType.OrderBy(a => a.Name).ToListAsync());
        }

        // GET: AccountTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccountTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,EmailExt,AccountExt,DeleteAfter,CreateAllowed")] AccountType accountType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(accountType);
                    await _context.SaveChangesAsync();
                    _log.AddConfig("Account", "Aacounttype " + accountType.Name + " aangemaakt door " + User.Identity.Name, _context);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Onmogelijk om dit accounttype aan te maken.");
            }
            return View(accountType);
        }

        // GET: AccountTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountType = await _context.AccountType.FindAsync(id);
            if (accountType == null)
            {
                return NotFound();
            }
            return View(accountType);
        }

        // POST: AccountTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EmailExt,AccountExt,DeleteAfter,CreateAllowed")] AccountType accountType)
        {
            if (id != accountType.Id)
            {
                return NotFound();
            }
            AccountType oldaccounttype = await _context.AccountType.AsNoTracking().FirstOrDefaultAsync(m => m.Id == accountType.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountTypeExists(accountType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                string editby = " door " + User.Identity.Name;
                _log.EditAccountType(oldaccounttype, accountType, editby, _context);
                return RedirectToAction(nameof(Index));
            }
            return View(accountType);
        }


        private bool AccountTypeExists(int id)
        {
            return _context.AccountType.Any(e => e.Id == id);
        }
    }
}
