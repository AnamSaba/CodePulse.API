using CodePusle.API.Data;
using CodePusle.API.Models.Domain;
using CodePusle.API.Repositories.Inteface;
using Microsoft.EntityFrameworkCore;

namespace CodePusle.API.Repositories.Implementation
{
	public class BlogPostRespository : IBlogPostRepository
	{
		private readonly ApplicationDbContext dbContext;

		public BlogPostRespository(ApplicationDbContext dbContext)
        {
			this.dbContext = dbContext;
		}
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
		{
			await dbContext.BlogPosts.AddAsync(blogPost);
			await dbContext.SaveChangesAsync();

			return blogPost;
		}

		public async Task<BlogPost?> DeleteAsync(Guid id)
		{
			var existingBlogPost = await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);

			if(existingBlogPost != null) 
			{
				dbContext.BlogPosts.Remove(existingBlogPost);
				await dbContext.SaveChangesAsync();

			}

			return existingBlogPost;
		}

		public async Task<IEnumerable<BlogPost>> GetAllAsync()
		{
			return await dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();

		}

		public async Task<BlogPost?> GetByIdAsync(Guid id)
		{
			return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
		{
			return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
		}

		public async Task<BlogPost?> UpdateAsync(Guid id, BlogPost blogPost)
		{
			var existingBlogPost = await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);

			if(existingBlogPost == null)
			{
				return null;
			}

			existingBlogPost.Title = blogPost.Title;
			existingBlogPost.ShortDescription = blogPost.ShortDescription;
			existingBlogPost.UrlHandle = blogPost.UrlHandle;
			existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
			existingBlogPost.Author	= blogPost.Author;
			existingBlogPost.PublishedDate = blogPost.PublishedDate;
			existingBlogPost.Author = blogPost.Author;
			existingBlogPost.IsVisible = blogPost.IsVisible;
			existingBlogPost.Categories = blogPost.Categories;

			await dbContext.SaveChangesAsync();

			return existingBlogPost;
		}
	}
}
