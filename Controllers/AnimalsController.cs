using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterDomain.Model;
using ShelterInfastructure;

namespace ShelterInfrastructure.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly DbshelterContext _context;
        private object shelter;

        public AnimalsController(DbshelterContext context)
        {
            _context = context;
        }

        // GET: Animals
        public async Task<IActionResult> Index(int? id, string? name)
        {
            var dbshelterContext = _context.Animals.Include(c => c.Shelter);

            if (id == null){
                return View(await dbshelterContext.ToListAsync());
            }


            ViewBag.ShelterId = id;
            ViewBag.ShelterName = name;
            var animalByShelter = _context.Animals.Where(b => b.ShelterId == id).Include(b => b.Shelter);

            return View(await animalByShelter.ToListAsync());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.Shelter)
                .FirstOrDefaultAsync(m => m.AnimalId == id);
            if (animal == null)
            {
                return NotFound();
            }

            //return View(animal);
            return RedirectToAction("Index", "MedicalCards", new { id = animal.AnimalId, name = animal.Name }) ;
        }

        // GET: Animals/Create
        public IActionResult Create(int shelterId)
        {
            if (shelterId != null && shelterId != 0) // if client id determined
            {
                var shelter = _context.Shelters.Where(c => c.ShelterId == shelterId).FirstOrDefault();
                ViewBag.ShelterId = shelterId;
                ViewBag.ShelterName = shelter.ShelterName; 
            }
            else
            {
                ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name");
            }

            return View();
        }


        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int shelterId, [Bind("AnimalId,ShelterId,Name,DateOfBirth,Gender,SpecialNeeds")] Animal animal)
        {
            animal.ShelterId = shelterId;
            Shelter shelter = _context.Shelters.FirstOrDefault(c => c.ShelterId == shelterId);
            if (shelter == null)
            {
                throw new InvalidOperationException($"No shelter found with ID {shelterId}.");
            }
            animal.Shelter = shelter;
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                if (animal.DateOfBirth > DateTime.Today) // перевірка на те, чи дата народження не пізніше за сьогоднішню дату
                {
                    ModelState.AddModelError("DateOfBirth", "Дата народження не може бути пізніше за сьогоднішню дату.");
                    return Redirect("/Animals/Create?shelterId="+shelterId);
                }

                _context.Add(animal);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Animals", new { id = shelterId, name = _context.Shelters.Where(c => c.ShelterId == shelterId).FirstOrDefault().ShelterName });
            }

            //return View(animal);
            return RedirectToAction("Index", "Animals", new { id = shelterId, name = _context.Shelters.Where(c => c.ShelterId == shelterId).FirstOrDefault().ShelterName });

        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            //ViewData["ShelterId"] = new SelectList(_context.Shelters, "ShelterId", "Address", animal.ShelterId);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnimalId,ShelterId,Name,DateOfBirth,Gender,SpecialNeeds")] Animal animal)
        {
            if (id != animal.AnimalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //if (animal.DateOfBirth > DateTime.Today) // перевірка на те, чи дата народження не пізніше за сьогоднішню дату
                //{
                //    ModelState.AddModelError("DateOfBirth", "Дата народження не може бути пізніше за сьогоднішню дату.");
                //    return View(animal);
                //}

                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.AnimalId))
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
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "ShelterId", "Address", animal.ShelterId);
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                //.Include(a => a.Shelter)
                .FirstOrDefaultAsync(m => m.AnimalId == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.AnimalId == id);
        }
    }
}