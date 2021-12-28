using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsterWebApp.Data;
using WebsterWebApp.Models;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CandidateListController : Controller
    {
        private readonly DatabaseContext _context;

        public CandidateListController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/CandidateList
        public async Task<IActionResult> Index()
        {
            return View(await _context.CandidateLists.ToListAsync());
        }

        // GET: Admin/CandidateList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateList = await _context.CandidateLists
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidateList == null)
            {
                return NotFound();
            }

            return View(candidateList);
        }

        // GET: Admin/CandidateList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CandidateList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,Exam,Email")] CandidateList candidateList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidateList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(candidateList);
        }

        // GET: Admin/CandidateList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateList = await _context.CandidateLists.FindAsync(id);
            if (candidateList == null)
            {
                return NotFound();
            }
            return View(candidateList);
        }

        // POST: Admin/CandidateList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,Exam,Email")] CandidateList candidateList)
        {
            if (id != candidateList.CandidateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidateList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateListExists(candidateList.CandidateId))
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
            return View(candidateList);
        }

        // GET: Admin/CandidateList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateList = await _context.CandidateLists
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidateList == null)
            {
                return NotFound();
            }

            return View(candidateList);
        }

        // POST: Admin/CandidateList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidateList = await _context.CandidateLists.FindAsync(id);
            _context.CandidateLists.Remove(candidateList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateListExists(int id)
        {
            return _context.CandidateLists.Any(e => e.CandidateId == id);
        }
    }
}
