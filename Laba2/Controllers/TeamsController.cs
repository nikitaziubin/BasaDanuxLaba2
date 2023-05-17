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
                                .Include(P => P.Stadium).ToList();
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
                                     .Include(p => p.Matches).ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                participates = a,
                stadiums = teamResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        public async Task<IActionResult> Query7(int? id)
        {
            ViewData["Name"] = new SelectList(_context.Teams, "Name", "Name");

            var TeamsResult = _context.Teams
                                     .FromSql($"SELECT s.*\r\nFROM Teams s")
                                     .ToList();
            var Result = _context.Matches
                                     .FromSql($"SELECT s.*\r\nFROM Matches s")
                                     .ToList();
            var Result2 = _context.Participates
                                     .FromSql($"SELECT s.*\r\nFROM Participate s")
                                     .ToList();
            var Result3 = _context.Stadiums
                                     .FromSql($"SELECT s.*\r\nFROM Stadiums s")
                                     .ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                stadiums = Result3,
                participates = Result2,
                matches = Result,
                teams = TeamsResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        // POST: Teams/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query7(Team team)
        {
            string name = team.Name;
            var teamResult = _context.Teams
                                     .FromSql($"SELECT DISTINCT t.*\r\nFROM Teams t\r\nJOIN participate p ON t.id = p.teamid\r\nJOIN matches m ON p.matchid = m.id\r\nJOIN stadiums s ON m.stadiumid = s.id\r\nWHERE t.name != {name}\r\n  AND NOT EXISTS (\r\n    (SELECT s2.name\r\n    FROM Teams t2\r\n    JOIN participate p2 ON t2.id = p2.teamid\r\n    JOIN matches m2 ON p2.matchid = m2.id\r\n    JOIN stadiums s2 ON m2.stadiumid = s2.id\r\n    WHERE t2.name = {name})\r\n    EXCEPT\r\n    (SELECT s2.name\r\n    FROM Teams t2\r\n    JOIN participate p2 ON t2.id = p2.teamid\r\n    JOIN matches m2 ON p2.matchid = m2.id\r\n    JOIN stadiums s2 ON m2.stadiumid = s2.id\r\n    WHERE t.ID = t2.ID))\r\n  AND NOT EXISTS (\r\n    (SELECT s2.name\r\n    FROM Teams t2\r\n    JOIN participate p2 ON t2.id = p2.teamid\r\n    JOIN matches m2 ON p2.matchid = m2.id\r\n    JOIN stadiums s2 ON m2.stadiumid = s2.id\r\n    WHERE t.ID = t2.ID)\r\n    EXCEPT\r\n    (SELECT s2.name\r\n    FROM Teams t2\r\n    JOIN participate p2 ON t2.id = p2.teamid\r\n    JOIN matches m2 ON p2.matchid = m2.id\r\n    JOIN stadiums s2 ON m2.stadiumid = s2.id\r\n    WHERE t2.name = {name}));\r\n")
                                     .ToList();
            var Result3 = _context.Stadiums
                                     .FromSql($"SELECT s.*\r\nFROM Stadiums s")
                                     .ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
               // stadiums = Result3,
                teams = teamResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        public async Task<IActionResult> Query7_1(int? id)
        {
            ViewData["Name"] = new SelectList(_context.Teams, "Name", "Name");

            var TeamsResult = _context.Teams
                                     .FromSql($"SELECT s.*\r\nFROM Teams s")
                                     .ToList();
            var Result = _context.Matches
                                     .FromSql($"SELECT s.*\r\nFROM Matches s")
                                     .ToList();
            var Result2 = _context.Participates
                                     .FromSql($"SELECT s.*\r\nFROM Participate s")
                                     .ToList();
            var Result3 = _context.Stadiums
                                     .FromSql($"SELECT s.*\r\nFROM Stadiums s")
                                     .ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                stadiums = Result3,
                participates = Result2,
                matches = Result,
                teams = TeamsResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        // POST: Teams/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query7_1(Team team)
        {   
            string name = team.Name;
            var teamResult = _context.Teams
                                     .FromSql($"\tSELECT DISTINCT t.*\r\nFROM Teams t\r\nJOIN participate p ON t.id = p.teamid\r\nJOIN matches m ON p.matchid = m.id\r\nJOIN stadiums s ON m.stadiumid = s.id\r\nWHERE t.name != {name}\r\n  AND NOT EXISTS (\r\n    (SELECT s2.name\r\n    FROM Teams t2\r\n    JOIN participate p2 ON t2.id = p2.teamid\r\n    JOIN matches m2 ON p2.matchid = m2.id\r\n    JOIN stadiums s2 ON m2.stadiumid = s2.id\r\n    WHERE t2.name = {name})\r\n    EXCEPT\r\n    (SELECT s2.name\r\n    FROM Teams t2\r\n    JOIN participate p2 ON t2.id = p2.teamid\r\n    JOIN matches m2 ON p2.matchid = m2.id\r\n    JOIN stadiums s2 ON m2.stadiumid = s2.id\r\n    WHERE t.ID = t2.ID))")
                                     .ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                teams = teamResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }


        public async Task<IActionResult> Query8(int? id)
        {
            ViewData["Name"] = new SelectList(_context.Teams, "Name", "Name");

            var TeamsResult = _context.Teams
                                     .FromSql($"SELECT s.*\r\nFROM Teams s")
                                     .ToList();
            var Result = _context.Matches
                                     .FromSql($"SELECT s.*\r\nFROM Matches s")
                                     .ToList();
            var Result2 = _context.Participates
                                     .FromSql($"SELECT s.*\r\nFROM Participate s")
                                     .ToList();
            var Result3 = _context.Divisions
                                     .FromSql($"SELECT s.*\r\nFROM Divisions s")
                                     .ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                divisions = Result3,
                participates = Result2,
                matches = Result,
                teams = TeamsResult
            };
            ViewData["Teams"] = new SelectList(_context.Teams, "Name", "Name");
            return View(viewModel);
        }

        // POST: Teams/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query8(Team team)
        {
            string name = team.Name;
            var teamResult = _context.Teams
                                     .FromSql($"SELECT t.*\r\nFROM Teams t\r\nJOIN participate p ON t.id = p.teamid\r\nJOIN Matches m ON p.matchid = m.ID\r\nJOIN divisions d ON m.DivisionID = d.ID\r\nWHERE t.name != {name} and d.id in (\r\n SELECT distinct d2.ID\r\n    FROM divisions d2\r\n    JOIN Matches m2 ON d2.ID = m2.DivisionID\r\n    JOIN participate p2 ON m2.ID = p2.matchid\r\n    JOIN Teams t2 ON p2.teamid = t2.id\r\n    WHERE t2.name = {name}\r\n);")
                                     .ToList();
            var Result = _context.Matches
                                     .FromSql($"SELECT s.*\r\nFROM Matches s")
                                     .ToList();
            var Result2 = _context.Participates
                                     .FromSql($"SELECT s.*\r\nFROM Participate s")
                                     .ToList();
            var Result3 = _context.Divisions
                                     .FromSql($"SELECT s.*\r\nFROM Divisions s")
                                     .ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                divisions = Result3,
                participates = Result2,
                matches = Result,
                teams = teamResult
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

