﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiracleToDoApp.Data;
using MiracleToDoApp.Models;

namespace MiracleToDoApp.Controllers
{
    public class StepsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StepsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Steps
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        private async Task<List<Step>> GetMySteps()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser? currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);

            List<Step> mySteps = await _context.steps.Where(x => x.User == currentUser).ToListAsync();

            int completeCount = 0;
            foreach(Step step in mySteps)
            {
                if(step.isDone) completeCount++;
            }
            ViewBag.Percent = Math.Round(100f * ((float)completeCount / (float)mySteps.Count()));

            return mySteps;
        }
        public async Task<IActionResult> BuildStepsTable()
        {
            
            return _context.steps != null ? PartialView("_StepsTable", await GetMySteps()) : Problem("Entity set 'ApplicationDbContext.steps'  is null.");
        }

        // GET: Steps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.steps == null)
            {
                return NotFound();
            }

            var step = await _context.steps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (step == null)
            {
                return NotFound();
            }

            return View(step);
        }

        // GET: Steps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Steps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,isDone")] Step step)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                IdentityUser? currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
                _context.Add(step);
                step.User = currentUser;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(step);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AJAXCreate([Bind("Id,Description")] Step step)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                IdentityUser? currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
                _context.Add(step);
                step.User = currentUser;
                step.isDone = false;
                await _context.SaveChangesAsync();
            }
            return _context.steps != null ? PartialView("_StepsTable", await GetMySteps()) : Problem("Entity set 'ApplicationDbContext.steps'  is null.");
        }

        // GET: Steps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.steps == null)
            {
                return NotFound();
            }

            var step = await _context.steps.FindAsync(id);
            if (step == null)
            {
                return NotFound();
            }

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser? currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);

            if (step.User != currentUser)
            {
                return BadRequest();
            }
            return View(step);
        }

        // POST: Steps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,isDone")] Step step)
        {
            if (id != step.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(step);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StepExists(step.Id))
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
            return View(step);
        }

        [HttpPost]
        public async Task<IActionResult> AJAXEdit(int? id, bool value)
        {
            if (id == null || _context.steps == null)
            {
                return NotFound();
            }

            var step = await _context.steps.FindAsync(id);
            if (step == null)
            {
                return NotFound();
            }
            else
            {
                step.isDone = value;
                _context.Update(step);
                await _context.SaveChangesAsync();

            }
            return _context.steps != null ? PartialView("_StepsTable", await GetMySteps()) : Problem("Entity set 'ApplicationDbContext.steps'  is null.");
        }

        // GET: Steps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.steps == null)
            {
                return NotFound();
            }

            var step = await _context.steps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (step == null)
            {
                return NotFound();
            }

            return View(step);
        }

        // POST: Steps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.steps == null)
            {
                return Problem("Entity set 'ApplicationDbContext.steps'  is null.");
            }
            var step = await _context.steps.FindAsync(id);
            if (step != null)
            {
                _context.steps.Remove(step);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StepExists(int id)
        {
          return (_context.steps?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
