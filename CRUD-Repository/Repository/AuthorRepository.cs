using CRUD_Repository.Data;
using CRUD_Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Repository.Repository
{
	public class AuthorRepository : IAuthorRepository
	{
		private readonly BookContext context;
		public AuthorRepository(BookContext context)
		{
			this.context = context;
		}
		public async Task<IEnumerable<Author>> GetAll()
		{
			return await context.Authors
								.ToListAsync();
		}
		public async Task<Author> GetById(int id)
		{
			return await context.Authors.FindAsync(id);
		}
		public async Task<Author> Insert(Author entity)
		{
			await context.Authors.AddAsync(entity);
			return entity;

		}

		public async Task Delete(int id)
		{
			var book = await context.Authors.FindAsync(id);
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
