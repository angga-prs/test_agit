using Microsoft.AspNetCore.Mvc;

namespace test_agit.Controllers
{
    public class Task1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(string plans)
        {
            if (string.IsNullOrEmpty(plans))
            {
                ViewBag.error = "Input tidak boleh kosong.";
                return View("Index");
            }

            try
            {
                var initialPlans = plans.Split(',')
                    .Select(int.Parse)
                    .ToList();

                ViewBag.initialPlans = initialPlans;

                if (initialPlans.Count != 5)
                {
                    ViewBag.Error = "Harap masukkan 5 angka yang dipisahkan koma.";
                    return View("Index");
                }

                int total = initialPlans.Sum();
                int average = total / 5;
                int remainder = total % 5;

                var adjustedPlans = Enumerable.Repeat(average, 5).ToList();

                var sortedIndexes = initialPlans
                    .Select((value, index) => new { value, index })
                    .OrderByDescending(x => x.value)
                    .Select(x => x.index)
                    .ToList();

                for (int i = 0; i < remainder; i++)
                {
                    adjustedPlans[sortedIndexes[i]]++;
                }

                ViewBag.adjustedPlans = adjustedPlans;

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Terjadi kesalahan: {ex.Message}";
                return View("Index");
            }
        }

    }
}
