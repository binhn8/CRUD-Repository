using CRUD_Repository.Models;

namespace CRUD_Repository.Repository
{
	public interface IBookRepository
	{
		Task<IEnumerable<Book>> GetAll();
		Task<Book> GetById(int id);
		Task<Book> Insert(Book entity);
		Task Delete(int id);
		Task Save();
	}
}
