using CodePusle.API.Models.Domain;
using CodePusle.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CodePusle.API.Repositories.Inteface
{
    public interface ICategoriesRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> UpdateAsync(Guid id, Category category);
        Task<Category?> DeleteAsync(Guid id);
    }
}
