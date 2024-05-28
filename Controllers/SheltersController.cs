using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShelterDomain.Model;
using ShelterInfastructure;

namespace ShelterInfrastructure.Controllers
{
    public class SheltersController : Controller
    {
        private readonly DbshelterContext _context;
        //private string name;

        public SheltersController(DbshelterContext context)
        {
            _context = context;
        }

        // GET: Shelters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shelters.ToListAsync());
        }

        // GET: Shelters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelter = await _context.Shelters
                .FirstOrDefaultAsync(m => m.ShelterId == id);
            if (shelter == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Animals", new { id = shelter.ShelterId, name = shelter.ShelterName });
        }


        // GET: Shelters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shelters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShelterId,ShelterName,Address,Contact,Id")] Shelter shelter)
        {
            if (ModelState.IsValid)
            {
                // Перевірка унікальності номера телефону
                var shelters = await _context.Shelters.ToListAsync();
                if (shelters.Any(s => s.Contact == shelter.Contact))
                {
                    ModelState.AddModelError("Contact", "Цей номер телефону вже використовується.");
                    return View(shelter);
                }

                _context.Add(shelter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shelter);
        }

        // GET: Shelters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelter = await _context.Shelters.FindAsync(id);
            if (shelter == null)
            {
                return NotFound();
            }
            return View(shelter);
        }

        // POST: Shelters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShelterId,ShelterName,Address,Contact,Id")] Shelter shelter)
        {
            if (id != shelter.ShelterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shelter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShelterExists(shelter.ShelterId))
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
            return View(shelter);
        }

        // GET: Shelters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelter = await _context.Shelters
                .FirstOrDefaultAsync(m => m.ShelterId == id);
            if (shelter == null)
            {
                return NotFound();
            }

            return View(shelter);
        }

        // POST: Shelters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelter = await _context.Shelters.FindAsync(id);
            if (shelter == null)
            {
                return NotFound();
            }

            var animalsToDelete = await _context.Animals.Where(a => a.ShelterId == id).ToListAsync();
            foreach (var animal in animalsToDelete)
            {
                var medicalCardsToDelete = await _context.MedicalCards.Where(mc => mc.AnimalId == animal.AnimalId).ToListAsync();
                _context.MedicalCards.RemoveRange(medicalCardsToDelete);
            }

            _context.Animals.RemoveRange(animalsToDelete);
            _context.Shelters.Remove(shelter);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ShelterExists(int id)
        {
            return _context.Shelters.Any(e => e.ShelterId == id);
        }

    }
}