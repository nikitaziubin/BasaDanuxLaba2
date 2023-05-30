using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba2;

namespace Laba2.Controllers
{
    public class ParticipatesController : Controller
    {
        private readonly BasaDanuxLaba2Context _context;

        public ParticipatesController(BasaDanuxLaba2Context context)
        {
            _context = context;
        }

        // GET: Participates
        public async Task<IActionResult> Index()
        {
            var basaDanuxLaba2Context = _context.Participates.Include(p => p.Match).Include(p => p.Team);
            return View(await basaDanuxLaba2Context.ToListAsync());
        }

        // GET: Participates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Participates == null)
            {
                return NotFound();
            }

            var participate = await _context.Participates
                .Include(p => p.Match)
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participate == null)
            {
                return NotFound();
            }

            return View(participate);
        }

        // GET: Participates/Create
        public IActionResult Create()
        {
            ViewData["MatchId"] = new SelectList(_context.Matches, "Id", "Id");
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id");
            return View();
        }

        // POST: Participates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamId,MatchId,ScoredGoals,BallOwnershipTime,TeamRole,RedCards,YellowCards,ShotsOnTarget,Shots,Passes,Offsides")] Participate participate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MatchId"] = new SelectList(_context.Matches, "Id", "Id", participate.MatchId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", participate.TeamId);
            return View(participate);
        }

        // GET: Participates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Participates == null)
            {
                return NotFound();
            }

            var participate = await _context.Participates.FindAsync(id);
            if (participate == null)
            {
                return NotFound();
            }
            ViewData["MatchId"] = new SelectList(_context.Matches, "Id", "Id", participate.MatchId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", participate.TeamId);
            return View(participate);
        }

        // POST: Participates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamId,MatchId,ScoredGoals,BallOwnershipTime,TeamRole,RedCards,YellowCards,ShotsOnTarget,Shots,Passes,Offsides")] Participate participate)
        {
            if (id != participate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipateExists(participate.Id))
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
            ViewData["MatchId"] = new SelectList(_context.Matches, "Id", "Id", participate.MatchId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", participate.TeamId);
            return View(participate);
        }

        // GET: Participates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Participates == null)
            {
                return NotFound();
            }

            var participate = await _context.Participates
                .Include(p => p.Match)
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participate == null)
            {
                return NotFound();
            }

            return View(participate);
        }

        // POST: Participates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Participates == null)
            {
                return Problem("Entity set 'BasaDanuxLaba2Context.Participates'  is null.");
            }
            var participate = await _context.Participates.FindAsync(id);
            if (participate != null)
            {
                _context.Participates.Remove(participate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipateExists(int id)
        {
          return (_context.Participates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
