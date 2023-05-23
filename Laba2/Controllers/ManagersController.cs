using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba2;
using Laba2.ViewModels;

namespace Laba2.Controllers
{
    public class ManagersController : Controller
    {
        private readonly BasaDanuxLaba2Context _context;

        public ManagersController(BasaDanuxLaba2Context context)
        {
            _context = context;
        }

        // GET: Managers
        public async Task<IActionResult> Index()
        {
              return _context.Managers != null ? 
                          View(await _context.Managers.ToListAsync()) :
                          Problem("Entity set 'BasaDanuxLaba2Context.Managers'  is null.");
        }

        // GET: Managers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }


        public IActionResult CreateQ()
        {
            return View();
        }

        // POST: Divisions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQ([Bind("Id,DivisoinOrLeague,Level,Name")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manager);
        }

        // GET: Managers/Create
        public IActionResult Create()
        {
            int count = 0;
            var managersResult = _context.Managers
                                .FromSql($"\t\tselect Managers.*, COUNT(t.ID) AS Club\r\n\tfrom Managers \r\n  Join Clubs c on Managers.ID = c.managerID\r\n  Join Teams t on c.ID = t.ClubID\t\r\n  GROUP BY Managers.ID , Managers.Name\r\n  HAVING COUNT(DISTINCT t.ID) != {count};").ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                managers = managersResult
            };
            return View(viewModel);
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Manager manager)
        {
            int count = manager.Id;
            var managersResult = _context.Managers
                                .FromSql($"\t\tselect Managers.*, COUNT(t.ID) AS Club\r\n\tfrom Managers \r\n  Join Clubs c on Managers.ID = c.managerID\r\n  Join Teams t on c.ID = t.ClubID\t\r\n  GROUP BY Managers.ID , Managers.Name\r\n  HAVING COUNT(DISTINCT t.ID) >= {count};").ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                managers = managersResult
            };
            return View(viewModel);
        }

        // GET: Managers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Manager manager)
        {
            if (id != manager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.Id))
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
            return View(manager);
        }

        // GET: Managers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Managers == null)
            {
                return Problem("Entity set 'BasaDanuxLaba2Context.Managers'  is null.");
            }
            var manager = await _context.Managers.FindAsync(id);
            if (manager != null)
            {
                _context.Managers.Remove(manager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManagerExists(int id)
        {
          return (_context.Managers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
