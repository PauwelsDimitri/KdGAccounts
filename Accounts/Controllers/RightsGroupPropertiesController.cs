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
    public class RightsGroupPropertiesController : Controller
    {
        private readonly KDGIDENTITYContext _context;
        private Log _log;

        public RightsGroupPropertiesController(KDGIDENTITYContext context)
        {
            _context = context;
            _log = new Log();
        }

        // GET: RightsGroupProperties
        public IActionResult Index()
        {
            return RedirectToAction("Index", "RightsGroups");
        }

        // GET: RightsGroupProperties/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //nieuw propertie
            KdgrightsGroupPropertie kdgrightsGroupPropertie = new KdgrightsGroupPropertie();
            kdgrightsGroupPropertie.RGId = (int)id;

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

            //Lijst met types
            Types types = new Types();

            if (rightsGroup == null)
            {
                return NotFound();
            }
            ViewData["Group"] = kdgGroup;
            ViewData["RightsGroup"] = rightsGroup.FirstOrDefault();
            ViewData["RightsGroupProperties"] = rightsGoupProperties;
            ViewData["Types"] = new SelectList(types.GetTypes());
            return View(kdgrightsGroupPropertie);
        }

        // POST: RightsGroupProperties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RGId,Field,Type,Value")] KdgrightsGroupPropertie kdgrightsGroupPropertie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kdgrightsGroupPropertie);
                await _context.SaveChangesAsync();

                var log = kdgrightsGroupPropertie.Field + "(" + kdgrightsGroupPropertie.Type + ") => " + kdgrightsGroupPropertie.Value;
                _log.AddConfig("Account", "RightsGroupProperty " + log + " aangemaakt door " + User.Identity.Name, _context);

                return RedirectToAction("Create", "RightsGroupProperties", new { id = kdgrightsGroupPropertie.RGId});
            }
            //rechtengroep details
            var rightsGroup = from r in _context.KdgrightsGroup.Where(g => g.Id == kdgrightsGroupPropertie.RGId).Include(k => k.AccountTypeNavigation)
                              select r;

            //lijst met properties
            var rightsGoupProperties = await _context.KdgrightsGroupPropertie
                .Include(k => k.KdgRightsGroupNavigation)
                .Where(m => m.KdgRightsGroupNavigation.Id == kdgrightsGroupPropertie.RGId)
                .ToListAsync();

            //lijst met groepen
            var kdgGroup = from g in _context.Kdggroup.Where(g => g.RGId == kdgrightsGroupPropertie.RGId)
                           select g;

            //Lijst met types
            Types types = new Types();

            ViewData["Group"] = kdgGroup;
            ViewData["RightsGroup"] = rightsGroup.FirstOrDefault();
            ViewData["RightsGroupProperties"] = rightsGoupProperties;
            ViewData["Types"] = new SelectList(types.GetTypes());
            return View(kdgrightsGroupPropertie);
        }

        // GET: RightsGroupProperties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdgrightsGroupPropertie = await _context.KdgrightsGroupPropertie.FindAsync(id);
            if (kdgrightsGroupPropertie == null)
            {
                return NotFound();
            }

            int RGId = kdgrightsGroupPropertie.RGId;

            //rechtengroep details
            var rightsGroup = from r in _context.KdgrightsGroup.Where(g => g.Id == RGId).Include(k => k.AccountTypeNavigation)
                              select r;

            //lijst met properties
            var rightsGoupProperties = await _context.KdgrightsGroupPropertie
                .Include(k => k.KdgRightsGroupNavigation)
                .Where(m => m.KdgRightsGroupNavigation.Id == RGId)
                .ToListAsync();

            //lijst met groepen
            var kdgGroup = from g in _context.Kdggroup.Where(g => g.RGId == RGId)
                           select g;

            //Lijst met types
            Types types = new Types();

            ViewData["Group"] = kdgGroup;
            ViewData["RightsGroup"] = rightsGroup.FirstOrDefault();
            ViewData["RightsGroupProperties"] = rightsGoupProperties;
            ViewData["Types"] = new SelectList(types.GetTypes());
            return View(kdgrightsGroupPropertie);
        }

        // POST: RightsGroupProperties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            KdgrightsGroupPropertie oldpropertie = await _context.KdgrightsGroupPropertie.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == Id);
            var propertieToUpdate = await _context.KdgrightsGroupPropertie.SingleOrDefaultAsync(p => p.Id == Id);
            if (await TryUpdateModelAsync<KdgrightsGroupPropertie>(
                propertieToUpdate, "",a => a.Field, a => a.Type, a => a.Value))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    string editby = " door " + User.Identity.Name;
                    _log.EditPropertie(oldpropertie, propertieToUpdate, editby, _context);
                    return RedirectToAction("Create", "RightsGroupProperties", new { id = oldpropertie.RGId });
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Onmogelijk om deze wijzigingen op te slaan.");
                }
            }
            //rechtengroep details
            var rightsGroup = from r in _context.KdgrightsGroup.Where(g => g.Id == oldpropertie.RGId).Include(k => k.AccountTypeNavigation)
                              select r;

            //lijst met properties
            var rightsGoupProperties = await _context.KdgrightsGroupPropertie
                .Include(k => k.KdgRightsGroupNavigation)
                .Where(m => m.KdgRightsGroupNavigation.Id == oldpropertie.RGId)
                .ToListAsync();

            //lijst met groepen
            var kdgGroup = from g in _context.Kdggroup.Where(g => g.RGId == oldpropertie.RGId)
                           select g;

            //Lijst met types
            Types types = new Types();
            ViewData["Group"] = kdgGroup;
            ViewData["RightsGroup"] = rightsGroup.FirstOrDefault();
            ViewData["RightsGroupProperties"] = rightsGoupProperties;
            ViewData["Types"] = new SelectList(types.GetTypes());
            return View(propertieToUpdate);
        }


        // GET: RightsGroupProperties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kdgrightsGroupPropertie = await _context.KdgrightsGroupPropertie
                .Include(k => k.KdgRightsGroupNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kdgrightsGroupPropertie == null)
            {
                return NotFound();
            }

            return View(kdgrightsGroupPropertie);
        }

        // POST: RightsGroupProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kdgrightsGroupPropertie = await _context.KdgrightsGroupPropertie.FindAsync(id);
            _context.KdgrightsGroupPropertie.Remove(kdgrightsGroupPropertie);
            var RGId = kdgrightsGroupPropertie.RGId;
            var log = kdgrightsGroupPropertie.Field + "(" + kdgrightsGroupPropertie.Type + ") => " + kdgrightsGroupPropertie.Value;
            await _context.SaveChangesAsync();
            _log.AddConfig("Account", "RightsGroupProperty " + log + " verwijderd door " + User.Identity.Name, _context);
            return RedirectToAction("Create", "RightsGroupProperties", new { id = RGId });
        }

        private bool KdgrightsGroupPropertieExists(int id)
        {
            return _context.KdgrightsGroupPropertie.Any(e => e.Id == id);
        }
    }
}
