using CodePusle.API.Data;
using CodePusle.API.Models.Domain;
using CodePusle.API.Repositories.Inteface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CodePusle.API.Repositories.Implementation
{
	public class ImageRepository : IImageRespository
	{
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly ApplicationDbContext dbContext;

		public ImageRepository(IWebHostEnvironment webHostEnvironment,
			IHttpContextAccessor httpContextAccessor,
			ApplicationDbContext dbContext)
        {
			this.webHostEnvironment = webHostEnvironment;
			this.httpContextAccessor = httpContextAccessor;
			this.dbContext = dbContext;
		}

		public async Task<IEnumerable<BlogImage>> GetAllAsync()
		{
			return await dbContext.BlogImages.ToListAsync();
		}

		public async Task<BlogImage> Upload(IFormFile file, BlogImage image)
		{
			// 1-Upload the Image to API/Image

			var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath,
			"Images", $"{image.FileName}{image.FileExtension}");

			using var stream = new FileStream(localFilePath, FileMode.Create);
			await file.CopyToAsync(stream);

			// 2-Update the database

			// https://codepulse.com/images/image.jpg


			var httpRequest = httpContextAccessor.HttpContext.Request;

			var urlFilePath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{image.FileName}{image.FileExtension}";

			image.Url = urlFilePath;

			await dbContext.BlogImages.AddAsync(image);
			await dbContext.SaveChangesAsync();

			return image;
		}


	}
}
