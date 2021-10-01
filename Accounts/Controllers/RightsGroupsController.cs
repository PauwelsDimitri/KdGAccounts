using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Accounts.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Accounts.Controllers
{
    
    public class RightsGroupsController : Controller
    {
        private readonly KDGIDENTITYContext _context;

        public RightsGroupsController(KDGIDENTITYContext context)
        {
            _context = context;
        }

        // GET: RightsGroups
        public async Task<IActionResult> Index()
        {
            var rightsgroup = from r in _context.KdgrightsGroup.Include(k => k.AccountTypeNavigation)
                              select r;

            return View(await rightsgroup.ToListAsync());
        }

        // GET: RightsGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdgrightsGroup = await _context.KdgrightsGroup
                .Include(k => k.AccountTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kdgrightsGroup == null)
            {
                return NotFound();
            }

            return View(kdgrightsGroup);
        }

		// GET: RightsGroups/Create
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
        {
            var accounttypes = from a in _context.AccountType.Where(p => p.CreateAllowed == true || p.Id == 0)
                               select a;
            ViewData["AccountType"] = new SelectList(accounttypes, "Id", "Name", 0);
            return View();
        }

		// POST: RightsGroups/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,MaxEndDate,AccountType")] KdgrightsGroup kdgrightsGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(kdgrightsGroup);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Onmogelijk om deze rechtengroup aan te maken.");
            }
            return View(kdgrightsGroup);
        }

		// GET: RightsGroups/Edit/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdgrightsGroup = await _context.KdgrightsGroup.FindAsync(id);
            if (kdgrightsGroup == null)
            {
                return NotFound();
            }
            var accounttypes = from a in _context.AccountType.Where(p => p.CreateAllowed == true || p.Id == 0)
                               select a;
            ViewData["AccountType"] = new SelectList(accounttypes, "Id", "Name", kdgrightsGroup.AccountType);
            return View(kdgrightsGroup);
        }

		// POST: RightsGroups/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MaxEndDate,AccountType")] KdgrightsGroup kdgrightsGroup)
        {
            if (id != kdgrightsGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kdgrightsGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KdgrightsGroupExists(kdgrightsGroup.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kdgrightsGroup);
        }

		// GET: RightsGroups/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdgrightsGroup = await _context.KdgrightsGroup
                .Include(k => k.AccountTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kdgrightsGroup == null)
            {
                return NotFound();
            }

            return View(kdgrightsGroup);
        }

        // POST: RightsGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kdgrightsGroup = await _context.KdgrightsGroup.FindAsync(id);
            ViewData["Error"] = "";
            try
            {
                _context.KdgrightsGroup.Remove(kdgrightsGroup);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ViewData["Error"]="Deze rechtengroep kan niet verwijderd worden. Is deze nog gekoppeld aan één of meerder accounts?";
                return View(kdgrightsGroup);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool KdgrightsGroupExists(int id)
        {
            return _context.KdgrightsGroup.Any(e => e.Id == id);
        }

		public IActionResult GetDDLGroups(int? accounttype)
		{
			var rightsgroups = from r in _context.KdgrightsGroup.Where(p => p.Id == 0)
											select r;

			if (accounttype == null)
			{
				return Json(rightsgroups);
			}

			rightsgroups = from r in _context.KdgrightsGroup.Where(p => p.Id == 0 || p.AccountType == accounttype)
							   select r;

			return Json(rightsgroups);
		}

		public IActionResult GetRGDetails(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var kdgrightsGroup =  _context.KdgrightsGroup
				.Include(k => k.AccountTypeNavigation)
				.Include(a => a.Kdggroup)
				.Include(p => p.Kdgrightsgrouppropertie)
				.Where(m => m.Id == id);

			if (kdgrightsGroup == null)
			{
				return NotFound();
			}

			return Json(kdgrightsGroup, new JsonSerializerSettings
			{
				Formatting = Formatting.None,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				PreserveReferencesHandling = PreserveReferencesHandling.None,
				TypeNameHandling = TypeNameHandling.None,
			});
		}
	}
}
