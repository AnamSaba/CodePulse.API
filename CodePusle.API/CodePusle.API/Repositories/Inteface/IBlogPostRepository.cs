using CodePusle.API.Models.Domain;

namespace CodePusle.API.Repositories.Inteface
{
	public interface IBlogPostRepository
	{
		Task<BlogPost> CreateAsync(BlogPost blogPost);
		Task<IEnumerable<BlogPost>> GetAllAsync();
		Task<BlogPost?> GetByIdAsync(Guid id);
		Task<BlogPost?> UpdateAsync(Guid id, BlogPost blogPost);
		Task<BlogPost?> DeleteAsync(Guid id);
		Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);
	}
}
