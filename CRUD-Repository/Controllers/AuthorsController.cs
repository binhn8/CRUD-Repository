using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Repository.Data;
using CRUD_Repository.Models;
using CRUD_Repository.Repository;

namespace CRUD_Repository.Controllers
{
	public class AuthorsController : Controller
    {
        private readonly IRepository<Author, int> authorRepository;

        public AuthorsController(IRepository<Author, int> authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await authorRepository.GetAll();
            return View(authors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await authorRepository.GetById((int)id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName")] Author author)
        {
            if (ModelState.IsValid)
            {
                await authorRepository.Insert(author);
                await authorRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var author = await authorRepository.GetById((int)id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var authorObject = await authorRepository.GetById(id);
                    authorObject.FirstName = author.FirstName;
                    authorObject.LastName = author.LastName;
                    await authorRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await authorRepository.GetById((int)id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await authorRepository.Delete(id); 
            await authorRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return authorRepository.GetById(id) != null;
        }
    }
}
