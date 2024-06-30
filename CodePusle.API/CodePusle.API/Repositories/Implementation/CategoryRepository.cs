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
		public async Task<IEnumerable<Category>> GetAllAsync(string? query = null,
            string? sortBy = null, string? sortDirection = null,
			int? pageNumber = 1, int? pageSize = 100)
		{
            var catergories =  dbContext.Categories.AsQueryable();


            // Filtering

            if(!string.IsNullOrWhiteSpace(query))
            {
                catergories = catergories.Where(x => x.Name.Contains(query));
            }

            // Sorting

            if(!string.IsNullOrWhiteSpace(sortBy))
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;
                    catergories = isAsc ? catergories.OrderBy(x => x.Name) : catergories.OrderByDescending(x => x.Name);
                }
				if (sortBy.Equals("URL", StringComparison.OrdinalIgnoreCase))
				{
					var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
						? true : false;
					catergories = isAsc ? catergories.OrderBy(x => x.UrlHandle) : catergories.OrderByDescending(x => x.UrlHandle);
				}
			}

			// Pagination

			// PageNumber 1 pageSize 5 - skip 0, take 5
			// PageNumber 2 pageSize 5 - skip 5, take 5
			// PageNumber 3 pageSize 5 - skip 10, take 5

			var skipResults = (pageNumber - 1) * pageSize;

            catergories = catergories.Skip(skipResults ?? 0).Take(pageSize ?? 100);


            return await catergories.ToListAsync();
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

		public async Task<int> GetTotalCountAsync()
		{
			return await dbContext.Categories.CountAsync();
		}
	}
}
