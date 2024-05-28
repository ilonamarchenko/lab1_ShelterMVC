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
    public class MedicalCardsController : Controller
    {
        private readonly DbshelterContext _context;
        private object animal;

        public MedicalCardsController(DbshelterContext context)
        {
            _context = context;
        }

        // GET: MedicalCards
        public async Task<IActionResult> Index(int? id, string? name)
        {
            var dbshelterContext = _context.MedicalCards.Include(c => c.Animal);

            if (id == null)
            {
                return View(await dbshelterContext.ToListAsync());
            }


            ViewBag.AnimalId = id;
            ViewBag.Name = name;
            var medicalCardByAnimal = _context.MedicalCards.Where(b => b.AnimalId == id).Include(b => b.Animal);

            return View(await medicalCardByAnimal.ToListAsync());
        }


        // GET: MedicalCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalCard = await _context.MedicalCards
                .Include(m => m.Animal)
                .FirstOrDefaultAsync(m => m.MedicalId == id);
            if (medicalCard == null)
            {
                return NotFound();
            }

            return View(medicalCard);
        }

        // GET: MedicalCards/Create
        public IActionResult Create(int animalId, string name)
        {
            if (animalId != null && animalId != 0) // if animal id determined
            {
                var animal = _context.Animals.Where(c => c.AnimalId == animalId).FirstOrDefault();
                ViewBag.AnimalId = animalId;
                ViewBag.Name = name;
            }
            else
            {
                ViewData["AnimalId"] = new SelectList(_context.Animals, "Id", "Name");
            }

            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int animalId, [Bind("MedicalId,AnimalId,DateOfCreation,Description")] MedicalCard medicalCard)
        {
            if (_context.MedicalCards.Any(mc => mc.AnimalId == animalId))
            {
                ModelState.AddModelError("", "Медична картка вже існує для цієї тварини.");
                return View(medicalCard);
            }
            //Animal animal = _context.Animals.Include(c => c.Shelter).FirstOrDefault(c => c.ShelterId == Shelter.AnimalId);
            medicalCard.AnimalId = animalId;
            Animal animal = _context.Animals.FirstOrDefault(c => c.AnimalId == animalId);
            if (animal == null)
            {
                // Throwing a runtime exception with a custom error message
                throw new InvalidOperationException($"No medical card found with ID {animalId}.");
            }
            medicalCard.Animal = animal;
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                _context.Add(medicalCard);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "MedicalCards", new { id = animalId, name = _context.Animals.Where(c => c.AnimalId == animalId).FirstOrDefault().Name });
            }
            //ViewData["ShelterId"] = new SelectList(_context.Shelters, "ShelterId", "Address", animal.ShelterId);
            //return View(animal);

            return RedirectToAction("Index", "MedicalCards", new { id = animalId, name = _context.Animals.Where(c => c.AnimalId == animalId).FirstOrDefault().Name });
        }

        // GET: MedicalCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalCard = await _context.MedicalCards.FindAsync(id);
            if (medicalCard == null)
            {
                return NotFound();
            }
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", medicalCard.AnimalId);
            return View(medicalCard);
        }

        // POST: MedicalCards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicalId,AnimalId,DateOfCreation,Description")] MedicalCard medicalCard)
        {
            if (id != medicalCard.MedicalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalCardExists(medicalCard.MedicalId))
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
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "Name", medicalCard.AnimalId);
            return View(medicalCard);
        }

        private bool MedicalCardExists(int id)
        {
            return _context.MedicalCards.Any(e => e.MedicalId == id);
        }


        // GET: MedicalCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalCard = await _context.MedicalCards
                .Include(m => m.Animal)
                .FirstOrDefaultAsync(m => m.MedicalId == id);
            if (medicalCard == null)
            {
                return NotFound();
            }

            return View(medicalCard);
        }

        // POST: MedicalCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalCard = await _context.MedicalCards.FindAsync(id);
            if (medicalCard != null)
            {
                _context.MedicalCards.Remove(medicalCard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
