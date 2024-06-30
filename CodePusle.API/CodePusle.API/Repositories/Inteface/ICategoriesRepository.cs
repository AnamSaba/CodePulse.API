using CodePusle.API.Models.Domain;
using CodePusle.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CodePusle.API.Repositories.Inteface
{
    public interface ICategoriesRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync(string? query = null, string? sortBy = null, string? sortDirection= null,
                                                int? pageNumber = 1, int? pageSize = 100);
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> UpdateAsync(Guid id, Category category);
        Task<Category?> DeleteAsync(Guid id);
        Task<int> GetTotalCountAsync();
    }
}
