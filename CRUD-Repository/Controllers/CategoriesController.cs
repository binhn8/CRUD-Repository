using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Repository.Data;
using CRUD_Repository.Models;
using CRUD_Repository.Repository;

namespace CRUD_Repository.Controllers
{
	public class CategoriesController : Controller
    {
        private readonly IRepository<Category, int> categoryRepository;
        public CategoriesController(IRepository<Category, int> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

		public async Task<IActionResult> Index()
		{
			var categories = await categoryRepository.GetAll();
			return View(categories);
		}

		public async Task<IActionResult> Details(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var category = await categoryRepository.GetById((int)id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                await categoryRepository.Insert(category);
                await categoryRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await categoryRepository.GetById((int)id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var categoryObject = await categoryRepository.GetById(id);
                    categoryObject.Name = category.Name;
                    categoryObject.Description = category.Description;
                    await categoryRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await categoryRepository.GetById((int)id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await categoryRepository.Delete(id);
            await categoryRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return categoryRepository.GetById(id) != null;
        }
    }
}
