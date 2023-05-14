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
    public class TeamsController : Controller
    {
        private readonly BasaDanuxLaba2Context _context;

        public TeamsController(BasaDanuxLaba2Context context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            var basaDanuxLaba2Context = _context.Teams.Include(t => t.Club);
            return View(await basaDanuxLaba2Context.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Club)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            var matchesResult = _context.Matches
                                .FromSql($"SELECT m.*\r\nFROM Matches m\r\nINNER JOIN Participate p ON m.ID = p.MatchID\r\nINNER JOIN Teams t ON p.TeamID = t.ID\r\nWHERE p.red_cards != -1 ")
                                .Include(P=>P.Stadium).ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                matches = matchesResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ClubId")] Team team)
        {
            string count = team.Name;
            var matchesResult = _context.Matches
                                .FromSql($"SELECT m.*\r\nFROM Matches m\r\nINNER JOIN Participate p ON m.ID = p.MatchID\r\nINNER JOIN Teams t ON p.TeamID = t.ID\r\nWHERE p.red_cards = 0 and t.name = {count}")
                                .Include(P => P.Stadium).ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            var viewModel = new DivisioStadiumViwModel
            {
                matches = matchesResult
            };
            return View(viewModel);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var teamResult = _context.Teams
                                .FromSql($"SELECT t.Name ,t.ID, t.ClubID\r\nFROM Teams t\r\nINNER JOIN Participate p ON t.ID = p.TeamID\r\nINNER JOIN Matches m ON p.MatchID = m.ID\r\nINNER JOIN Divisions d ON m.DivisionID = d.ID\r\nGROUP BY t.Name, t.ClubID, t.ID\r\nHAVING COUNT(*) >= 2;")
                                .ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                teams = teamResult
            };
            //ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ClubId")] Team team)
        {
            int num = team.Id;
            var teamResult = _context.Teams
                                 .FromSql($"SELECT t.Name ,t.ID, t.ClubID\r\nFROM Teams t\r\nINNER JOIN Participate p ON t.ID = p.TeamID\r\nINNER JOIN Matches m ON p.MatchID = m.ID\r\nINNER JOIN Divisions d ON m.DivisionID = d.ID\r\nGROUP BY t.Name, t.ClubID, t.ID\r\nHAVING COUNT(*) >= {num};")
                                 .ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                teams = teamResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var stadiumResult = _context.Stadiums
                                     .FromSql($"SELECT s.*\r\nFROM Stadiums s")
                                     .ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                stadiums = stadiumResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Team team)
        {
            var a = _context.Participates.ToList();
            string count = team.Name;

            var teamResult = _context.Stadiums
                                     .FromSql($"SELECT s.*\r\nFROM Stadiums s\r\nINNER JOIN Matches m ON s.ID = m.StadiumID\r\nINNER JOIN Participate p ON m.ID = p.MatchID\r\nINNER JOIN Teams t ON p.TeamID = t.ID\r\nWHERE t.Name = {count} AND p.yellow_cards > 0")
                                     .Include(p=>p.Matches).ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                participates = a,
                stadiums = teamResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        private bool TeamExists(int id)
        {
          return (_context.Teams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
//SELECT DISTINCT t.name
//FROM Teams t
//JOIN participate p ON t.id = p.teamid
//JOIN matches m ON p.matchid = m.id
//JOIN stadiums s ON m.stadiumid = s.id
//WHERE t.name != 'Arsenal 2'
//  AND NOT EXISTS (
//		(SELECT s2.name
//		FROM Teams t2
//		JOIN participate p2 ON t2.id = p2.teamid
//		JOIN matches m2 ON p2.matchid = m2.id
//		JOIN stadiums s2 ON m2.stadiumid = s2.id
//		WHERE t2.name = 'Arsenal 2')
//    EXCEPT
//        (SELECT s2.name

//        FROM Teams t2

//        JOIN participate p2 ON t2.id = p2.teamid

//        JOIN matches m2 ON p2.matchid = m2.id

//        JOIN stadiums s2 ON m2.stadiumid = s2.id

//        WHERE t.ID = t2.ID))
//  AND NOT EXISTS (
//		(SELECT s2.name
//		FROM Teams t2
//		JOIN participate p2 ON t2.id = p2.teamid
//		JOIN matches m2 ON p2.matchid = m2.id
//		JOIN stadiums s2 ON m2.stadiumid = s2.id
//		WHERE t.ID = t2.ID)
//    EXCEPT
//        (SELECT s2.name

//        FROM Teams t2

//        JOIN participate p2 ON t2.id = p2.teamid

//        JOIN matches m2 ON p2.matchid = m2.id

//        JOIN stadiums s2 ON m2.stadiumid = s2.id

//        WHERE t2.name = 'Arsenal 2'));