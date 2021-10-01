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

namespace Accounts.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RightsGroupGroupsController : Controller
    {
        private readonly KDGIDENTITYContext _context;
        private Log _log;

        public RightsGroupGroupsController(KDGIDENTITYContext context)
        {
            _context = context;
            _log = new Log();
        }

        // GET: RightsGroupGroups
        public IActionResult Index()
        {
            return RedirectToAction("Index", "RightsGroups");
        }

        // GET: RightsGroupGroups/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //nieuwe groep
            Kdggroup group = new Kdggroup();
            group.RGId = (int)id;

            //rechtengroep details
            var rightsGroup = from r in _context.KdgrightsGroup.Where(g => g.Id == id).Include(k => k.AccountTypeNavigation)
                              select r;

            //lijst met properties
            var rightsGoupProperties = await _context.KdgrightsGroupPropertie
                .Include(k => k.KdgRightsGroupNavigation)
                .Where(m => m.KdgRightsGroupNavigation.Id == id)
                .ToListAsync();

            //lijst met groepen
            var kdgGroup = from g in _context.Kdggroup.Where(g => g.RGId == id)
                           select g;

            //lijst met domeinen
            Domain dom = new Domain();

            if (rightsGroup == null)
            {
                return NotFound();
            }
            ViewData["Domain"] = new SelectList(dom.GetDomains());
            ViewData["Group"] = kdgGroup;
            ViewData["RightsGroup"] = rightsGroup.FirstOrDefault();
            ViewData["RightsGroupProperties"] = rightsGoupProperties;
            return View(group);
        }

        // POST: RightsGroupGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RGId,AccountName,DisplayName,Domain")] Kdggroup kdggroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kdggroup);
                await _context.SaveChangesAsync();

                var log = kdggroup.Domain + "\\" + kdggroup.AccountName + " (" + kdggroup.DisplayName + ")";
                _log.AddConfig("Account", "RightsGroupGroups " + log + " aangemaakt door " + User.Identity.Name, _context);

                return RedirectToAction("Create", "RightsGroupGroups", new { id = kdggroup.RGId });
            }

            //rechtengroep details
            var rightsGroup = from r in _context.KdgrightsGroup.Where(g => g.Id == kdggroup.RGId).Include(k => k.AccountTypeNavigation)
                              select r;

            //lijst met properties
            var rightsGoupProperties = await _context.KdgrightsGroupPropertie
                .Include(k => k.KdgRightsGroupNavigation)
                .Where(m => m.KdgRightsGroupNavigation.Id == kdggroup.RGId)
                .ToListAsync();

            //lijst met groepen
            var kdgGroup = from g in _context.Kdggroup.Where(g => g.RGId == kdggroup.RGId)
                           select g;

            //lijst met domeinen
            Domain dom = new Domain();

            ViewData["Domain"] = new SelectList(dom.GetDomains());
            ViewData["Group"] = kdgGroup;
            ViewData["RightsGroup"] = rightsGroup.FirstOrDefault();
            ViewData["RightsGroupProperties"] = rightsGoupProperties;
            return View(kdggroup);
        }

        // GET: RightsGroupGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdggroup = await _context.Kdggroup.FindAsync(id);
            if (kdggroup == null)
            {
                return NotFound();
            }

            //rechtengroep details
            var rightsGroup = from r in _context.KdgrightsGroup.Where(g => g.Id == kdggroup.RGId).Include(k => k.AccountTypeNavigation)
                              select r;

            //lijst met properties
            var rightsGoupProperties = await _context.KdgrightsGroupPropertie
                .Include(k => k.KdgRightsGroupNavigation)
                .Where(m => m.KdgRightsGroupNavigation.Id == kdggroup.RGId)
                .ToListAsync();

            //lijst met groepen
            var kdgGroup = from g in _context.Kdggroup.Where(g => g.RGId == kdggroup.RGId)
                           select g;

            //lijst met domeinen
            Domain dom = new Domain();

            ViewData["Domain"] = new SelectList(dom.GetDomains());
            ViewData["Group"] = kdgGroup;
            ViewData["RightsGroup"] = rightsGroup.FirstOrDefault();
            ViewData["RightsGroupProperties"] = rightsGoupProperties;
            return View(kdggroup);
        }

        // POST: RightsGroupGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? Id, string AccountName)
        {
            if (Id == null)
            {
                return NotFound();
            }
            //AD _AD = new AD();
            //ADGroup KDGMedwerkers = _AD.GetADGroup(AccountName, "ADMIN");
            Kdggroup oldgroup = await _context.Kdggroup.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == Id);
            var groupToUpdate = await _context.Kdggroup.SingleOrDefaultAsync(p => p.Id == Id);
            if (await TryUpdateModelAsync<Kdggroup>(
                groupToUpdate, "", g => g.AccountName, g => g.DisplayName, g => g.Domain))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    string editby = " door " + User.Identity.Name;
                    _log.EditRightsGroupGroup(oldgroup,groupToUpdate,editby,_context);
                    return RedirectToAction("Create", "RightsGroupGroups", new { id = oldgroup.RGId });

                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Onmogelijk om deze wijzigingen op te slaan.");
                }
            }
            //rechtengroep details
            var rightsGroup = from r in _context.KdgrightsGroup.Where(g => g.Id == oldgroup.RGId).Include(k => k.AccountTypeNavigation)
                              select r;

            //lijst met properties
            var rightsGoupProperties = await _context.KdgrightsGroupPropertie
                .Include(k => k.KdgRightsGroupNavigation)
                .Where(m => m.KdgRightsGroupNavigation.Id == oldgroup.RGId)
                .ToListAsync();

            //lijst met groepen
            var kdgGroup = from g in _context.Kdggroup.Where(g => g.RGId == oldgroup.RGId)
                           select g;

            //lijst met domeinen
            Domain dom = new Domain();

            ViewData["Domain"] = new SelectList(dom.GetDomains());
            ViewData["Group"] = kdgGroup;
            ViewData["RightsGroup"] = rightsGroup.FirstOrDefault();
            ViewData["RightsGroupProperties"] = rightsGoupProperties;
            return View(groupToUpdate);

        }

        // GET: RightsGroupGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdggroup = await _context.Kdggroup
                .Include(k => k.KdgRightsGroupNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kdggroup == null)
            {
                return NotFound();
            }

            return View(kdggroup);
        }

        // POST: RightsGroupGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kdggroup = await _context.Kdggroup.FindAsync(id);
            _context.Kdggroup.Remove(kdggroup);
            var RGId = kdggroup.RGId;
            var log = kdggroup.Domain + "\\" + kdggroup.AccountName + " (" + kdggroup.DisplayName + ")";
            await _context.SaveChangesAsync();
            _log.AddConfig("Account", "RightsGroupGroups " + log + " verwijderd door " + User.Identity.Name, _context);

            return RedirectToAction("Create", "RightsGroupGroups", new { id = RGId });
        }

        private bool KdggroupExists(int id)
        {
            return _context.Kdggroup.Any(e => e.Id == id);
        }
    }
}
