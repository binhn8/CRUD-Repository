using CRUD_Repository.Data;
using CRUD_Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Repository.Repository
{
	public class CategoryRepository : IRepository<Category, int>
	{
		private readonly BookContext context;
		public CategoryRepository(BookContext context)
		{
			this.context = context;
		}
		public async Task<IEnumerable<Category>> GetAll()
		{
			return await context.Categories
								.ToListAsync();
		}
		public async Task<Category> GetById(int id)
		{
			return await context.Categories.FindAsync(id);
		}
		public async Task<Category> Insert(Category entity)
		{
			await context.Categories.AddAsync(entity);
			return entity;

		}

		public async Task Delete(int id)
		{
			var category = await context.Categories.FindAsync(id);
			if (category != null)
			{
				context.Remove(category);
			}
		}

		public async Task Save()
		{
			await context.SaveChangesAsync();
		}
	}
}
