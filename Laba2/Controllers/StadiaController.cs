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
using Laba2.ViewModels;
using ClosedXML.Excel;

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
            ViewData["Name"] = new SelectList(_context.Stadiums, "Name", "Name");
            string stadiumName = "%";
            var divisions = _context.Divisions
                    .FromSql($"select Divisions.*from Stadiums\r\n  Join Matches on Stadiums.ID = Matches.StadiumID\r\n  Join Divisions on Divisions.ID = Matches.divisionID\r\n  where Stadiums.name like {stadiumName}").ToList();
            var viewModel = new DivisioStadiumViwModel
            {
                divisions = divisions
            };
            return View(viewModel);
        }

        // POST: Stadia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Adress,Name,Capacity,MaxCapacity")] Stadium stadium)
        {
            string stadiumName = stadium.Name;
            var divisions = _context.Divisions
                                .FromSql($"select Divisions.*from Stadiums\r\n  Join Matches on Stadiums.ID = Matches.StadiumID\r\n  Join Divisions on Divisions.ID = Matches.divisionID\r\n  where Stadiums.name like {stadiumName}").ToList();
            //ViewData["DivisionsResult"] = new SelectList(divisions);
            var viewModel = new DivisioStadiumViwModel
            {
                divisions = divisions
            };
            ViewData["Name"] = new SelectList(_context.Stadiums, "Name", "Name");

            using (XLWorkbook workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Managers");
                worksheet.Cell("A1").Value = "Ім'я Дівізіону";
                worksheet.Cell("B1").Value = "Левел";

                worksheet.Row(1).Style.Font.Bold = true;
                int j = 1;
                int i = 2;
                foreach (var c in divisions)
                {
                    worksheet.Cell(i, j).Value = c.Name;
                    worksheet.Cell(i, j+1).Value = c.Level;
                    //j++;
                    i++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        // Змініть назву файла відповідно до тематики Вашого проєкту
                        FileDownloadName = $"library{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };

                }
            }
        }

        // GET: Stadia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Name"] = new SelectList(_context.Stadiums, "Name", "Name");
            string stadiumName = "%";
            var divisions = _context.Stadiums
                    .FromSql($"select * from Stadiums").ToList();
            var data = _context.Matches
                    .FromSql($"select * from Matches").ToList();
            var viewModel = new DivisioStadiumViwModel
            {
                matches = data,
                stadiums = divisions
            };
            return View(viewModel);
        }

        // POST: Stadia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adress,Name,Capacity,MaxCapacity")] Stadium stadium)
        {
            ViewData["Name"] = new SelectList(_context.Stadiums, "Name", "Name");
            string stadiumName = stadium.Name;
            var divisions = _context.Stadiums
                    .FromSql($"SELECT  s.*, m.Date\r\nFROM stadiums s\r\nJOIN matches m ON s.id = m.stadiumid\r\nWHERE s.name != {stadiumName} and exists (\r\n    SELECT m2.Date\r\n    FROM stadiums s2\r\n    JOIN matches m2 ON s2.id = m2.stadiumid\r\n    WHERE s2.name = {stadiumName} and m.Date = m2.Date  \r\n);\r\n").ToList();
            var date= _context.Matches
                    .FromSql($"SELECT  m.*\r\nFROM stadiums s\r\nJOIN matches m ON s.id = m.stadiumid\r\nWHERE s.name != {stadiumName} and exists (\r\n    SELECT m2.Date\r\n    FROM stadiums s2\r\n    JOIN matches m2 ON s2.id = m2.stadiumid\r\n    WHERE s2.name = {stadiumName} and m.Date = m2.Date  \r\n);\r\n").ToList();
            var viewModel = new DivisioStadiumViwModel
            {
                matches = date,
                stadiums = divisions
            };
            return View(viewModel);
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
