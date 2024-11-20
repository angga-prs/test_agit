using Microsoft.AspNetCore.Mvc;
using test_agit.Data;
using test_agit.Models;

namespace test_agit.Controllers
{
    public class Task2Controller : Controller
    {
        private readonly DatabaseContext _context;

        public Task2Controller(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var plans = _context.tProductionPlans.ToList();
            return View(plans);
        }

        [HttpPost]
        public IActionResult Calculate(string plans)
        {
            if (string.IsNullOrEmpty(plans))
            {
                ViewBag.Error = "Input tidak boleh kosong.";
                return View("Index", _context.tProductionPlans.ToList());
            }

            try
            {
                var initialPlans = plans.Split(',')
                    .Select(int.Parse)
                    .ToList();

                if (initialPlans.Count != 7)
                {
                    ViewBag.Error = "Harap masukkan 7 angka yang dipisahkan koma.";
                    return View("Index", _context.tProductionPlans.ToList());
                }

                int total = initialPlans.Where(x => x > 0).Sum();
                int countNonZero = initialPlans.Count(x => x > 0);
                int average = countNonZero > 0 ? total / countNonZero : 0;
                int remainder = total % countNonZero;

                var adjustedPlans = initialPlans
                    .Select(x => x > 0 ? average : 0)
                    .ToList();

                var sortedIndexes = initialPlans
                    .Select((value, index) => new { value, index })
                    .Where(x => x.value > 0)
                    .OrderByDescending(x => x.value)
                    .Select(x => x.index)
                    .ToList();

                
                for (int i = 0; i < remainder; i++)
                {
                    adjustedPlans[sortedIndexes[i]]++;
                }
                
                Console.WriteLine("Initial Plans: " + string.Join(",", initialPlans));
                Console.WriteLine("Adjusted Plans: " + string.Join(",", adjustedPlans));
                Console.WriteLine("Sorted Indexes: " + string.Join(",", sortedIndexes));

                var productionPlan = new ProductionPlan
                {
                    initial_plan = string.Join(",", initialPlans),
                    adjusted_plan = string.Join(",", adjustedPlans)
                };

                _context.tProductionPlans.Add(productionPlan);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Terjadi kesalahan: {ex.Message}";
                return View("Index", _context.tProductionPlans.ToList());
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var plan = _context.tProductionPlans.FirstOrDefault(x => x.id == id);
            if (plan != null)
            {
                _context.tProductionPlans.Remove(plan);
                _context.SaveChanges();
                ViewBag.message = "Data berhasil dihapus.";
                ViewBag.status = 1;
            }
            else
            {
                ViewBag.status = 0;
                ViewBag.message = "Data tidak ditemukan.";
            }

            return View("Index", _context.tProductionPlans.ToList());
        }
    }
}