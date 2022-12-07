using CRUD_Repository.Models;

namespace CRUD_Repository.Repository
{
	public interface IAuthorRepository
	{
		Task<IEnumerable<Author>> GetAll();
		Task<Author> GetById(int id);
		Task<Author> Insert(Author entity);
		Task Delete(int id);
		Task Save();
	}
}
