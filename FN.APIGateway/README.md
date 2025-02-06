using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FN.APIGateway
{
    public class README : Controller
    {
        // GET: README
        public ActionResult Index()
        {
            return View();
        }

        // GET: README/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: README/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: README/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: README/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: README/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: README/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: README/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
