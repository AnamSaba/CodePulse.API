using CodePusle.API.Data;
using CodePusle.API.Models.Domain;
using CodePusle.API.Models.DTO;
using CodePusle.API.Repositories.Inteface;

namespace CodePusle.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoriesRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            category.Id = Guid.NewGuid();
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;
        }
    }
}
