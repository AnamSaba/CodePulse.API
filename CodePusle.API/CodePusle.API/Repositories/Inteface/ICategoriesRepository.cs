using CodePusle.API.Models.Domain;
using CodePusle.API.Models.DTO;

namespace CodePusle.API.Repositories.Inteface
{
    public interface ICategoriesRepository
    {
        Task<Category> CreateAsync(Category category);
    }
}
