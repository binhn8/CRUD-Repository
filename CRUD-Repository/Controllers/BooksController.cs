using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Repository.Data;
using CRUD_Repository.Models;
using CRUD_Repository.Repository;

namespace CRUD_Repository.Controllers
{
	public class BooksController : Controller
    {
        private readonly BookContext context;
        private readonly IBookRepository bookRepository;
        public BooksController(BookContext context, IBookRepository bookRepository)
        {
            this.context = context;
            this.bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index()
        {
            var books = await bookRepository.GetAll();
            return View(books);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || context.Books == null)
            {
                return NotFound();
            }
            var book = await bookRepository.GetById((int)id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public IActionResult Create()
        {
            PopulateAuthorsList();
            PopulateCategoriesList();
            return View();
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Published,AuthorId,CategoryId")] Book book)
        {
            if (ModelState.IsValid)
            {
                await bookRepository.Insert(book);
                await bookRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await bookRepository.GetById((int)id);
            if (book == null)
            {
                return NotFound();
            }
			PopulateAuthorsList();
			PopulateCategoriesList();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Published,AuthorId,CategoryId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var bookObject = await bookRepository.GetById(id);
                    bookObject.Author = book.Author;
                    bookObject.AuthorId = book.AuthorId;
                    bookObject.Category = book.Category;
                    bookObject.CategoryId = book.CategoryId;
                    bookObject.Published = book.Published;
                    bookObject.Title = book.Title;
                    await bookRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await bookRepository.GetById((int)id);
            if (book == null)
            {
                return NotFound();
            }
            PopulateAuthorsList();
            PopulateCategoriesList();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await bookRepository.Delete(id);
            await bookRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return bookRepository.GetById(id) != null;
        }

        private void PopulateCategoriesList()
        {
            var categories = context.Categories.ToList();
            var categoryObject = new Category { Id = 0, Description = "-- Select --", Name = "-- Select --" };
            categories.Insert(0, categoryObject);
            ViewBag.Categories = categories;
        }

        private void PopulateAuthorsList()
        {
            var authors = context.Authors.ToList();
            var authorObject = new Author { Id = 0, FirstName = "-- Select --" , LastName = ""};
            authors.Insert(0, authorObject);
            ViewBag.Authors = authors;
        }

    }
}
