using CodePusle.API.Data;
using CodePusle.API.Models.Domain;
using CodePusle.API.Models.DTO;
using CodePusle.API.Repositories.Inteface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
		public async Task<IEnumerable<Category>> GetAllAsync()
		{
            return await dbContext.Categories.ToListAsync();
		}

		public async Task<Category?> GetByIdAsync(Guid id)
		{
			return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Category?> UpdateAsync(Guid id, Category category)
		{
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = category.Name;
            existingCategory.UrlHandle = category.UrlHandle;

            await dbContext.SaveChangesAsync();

            return existingCategory;
		}

		public async Task<Category?> DeleteAsync(Guid id)
		{
			var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (existingCategory == null)
            {
                return null;
            }

            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return existingCategory;

		}


	}
}
