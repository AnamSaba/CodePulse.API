using CodePusle.API.Models.Domain;

namespace CodePusle.API.Repositories.Inteface
{
	public interface IImageRespository
	{
		Task<BlogImage> Upload(IFormFile file, BlogImage image);
		Task<IEnumerable<BlogImage>> GetAllAsync();
	}
}
