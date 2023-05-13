using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba2;
using Microsoft.Data.SqlClient;
using Microsoft.CodeAnalysis.Elfie.Model.Structures;

namespace Laba2.Controllers
{
    public class StadiaController : Controller
    {
        private readonly BasaDanuxLaba2Context _context;

        public StadiaController(BasaDanuxLaba2Context context)
        {
            _context = context;
        }

        // GET: Stadia
        public async Task<IActionResult> Index()
        {
              return _context.Stadiums != null ? 
                          View(await _context.Stadiums.ToListAsync()) :
                          Problem("Entity set 'BasaDanuxLaba2Context.Stadiums'  is null.");
        }

        // GET: Stadia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stadiums == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadiums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // GET: Stadia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stadia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Adress,Name,Capacity,MaxCapacity")] Stadium stadium)
        {
            string stadiumName = stadium.Name;
            //using (var ctx = new BasaDanuxLaba2Context())
            //{
            //    //List<Division> divisions = ctx. Database
                var divisions = _context.Divisions
            //                    //.SqlQuery<Division>($"select Divisions.ID from Stadium\r\n  Join Matches on Stadium.ID = Matches.stadium\r\n  Join Divisions on Divisions.ID = Matches.division\r\n  where Stadium.name = {stadiumName}").ToListAsync();
            //                    //.SqlQuery<int>($"select Divisions.ID from Stadium\r\n  Join Matches on Stadium.ID = Matches.stadium\r\n  Join Divisions on Divisions.ID = Matches.division\r\n  where Stadium.name = {stadiumName}").ToListAsync();
                                .FromSql($"select Divisions.*from Stadiums\r\n  Join Matches on Stadiums.ID = Matches.StadiumID\r\n  Join Divisions on Divisions.ID = Matches.divisionID\r\n  where Stadiums.name = {stadiumName}").ToList();
            //    //.FirstOrDefault();
            //}

            //string stadiumName = stadium.Name;
            //using (var ctx = new BasaDanuxLaba2Context())
            //{
                //List<Division> divisions = ctx. Database
              //  var divisions = _context.Stadiums
                                //.SqlQuery<Division>($"select Divisions.ID from Stadium\r\n  Join Matches on Stadium.ID = Matches.stadium\r\n  Join Divisions on Divisions.ID = Matches.division\r\n  where Stadium.name = {stadiumName}").ToListAsync();
                                //.SqlQuery<int>($"select Divisions.ID from Stadium\r\n  Join Matches on Stadium.ID = Matches.stadium\r\n  Join Divisions on Divisions.ID = Matches.division\r\n  where Stadium.name = {stadiumName}").ToListAsync();
                //                .FromSql($"select Stadium.* from Stadium\r\n  Join Matches on Stadium.ID = Matches.stadium\r\n  Join Divisions on Divisions.ID = Matches.division\r\n  where Stadium.name = {stadiumName}").ToList();
                //.FirstOrDefault();    
            //}


            //if (ModelState.IsValid)
            //{
            //    _context.Add(stadium);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            return View(stadium);
        }

        // GET: Stadia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stadiums == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadiums.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }
            return View(stadium);
        }

        // POST: Stadia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adress,Name,Capacity,MaxCapacity")] Stadium stadium)
        {
            if (id != stadium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stadium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StadiumExists(stadium.Id))
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
            return View(stadium);
        }

        // GET: Stadia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stadiums == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadiums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // POST: Stadia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stadiums == null)
            {
                return Problem("Entity set 'BasaDanuxLaba2Context.Stadiums'  is null.");
            }
            var stadium = await _context.Stadiums.FindAsync(id);
            if (stadium != null)
            {
                _context.Stadiums.Remove(stadium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StadiumExists(int id)
        {
          return (_context.Stadiums?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
