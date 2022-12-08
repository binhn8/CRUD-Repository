using CRUD_Repository.Data;
using CRUD_Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Repository.Repository
{
	public class BookRepository : IRepository<Book,int>
	{
		private readonly BookContext context;
		public BookRepository(BookContext context)
		{
			this.context = context;
		}
		public async Task<IEnumerable<Book>> GetAll()
		{
			return await context.Books
								.Include(b => b.Author)
								.Include(b => b.Category)
								.ToListAsync();
		}
		public async Task<Book> GetById(int id)
		{
			return await context.Books.FindAsync(id);
		}
		public async Task<Book> Insert(Book entity)
		{
			await context.Books.AddAsync(entity);
			return entity;

		}

		public async Task Delete(int id)
		{
			var book = await context.Books.FindAsync(id);
			if (book != null)
			{
				context.Remove(book);
			}
		}

		public async Task Save()
		{
			await context.SaveChangesAsync();
		}
	}
}
