using AutoMapper;
using CodePusle.API.Models.Domain;
using CodePusle.API.Models.DTO;
using CodePusle.API.Repositories.Inteface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePusle.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IImageRespository imageRespository;

		public ImagesController(IImageRespository imageRespository)
        {
			this.imageRespository = imageRespository;
		}

        //{apiBaseUrl}/api/images
        [HttpPost]

		public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
			[FromForm] string fileName, [FromForm] string title)
		{
			ValidateFileUpload(file);


			if (ModelState.IsValid)
			{
				//File Upload
				var blogImage = new BlogImage
				{
					FileExtension = Path.GetExtension(file.FileName.ToLower()),
					FileName = fileName,
					Title = title,
					DateCreated = DateTime.Now
				};

				var blogPost = await imageRespository.Upload(file,blogImage);

				// Convert Domain model to Dto
				var blogImageDto = new BlogImageDto
				{
					Id = blogImage.Id,
					Title = blogImage.Title,
					DateCreated = blogImage.DateCreated,
					FileExtension = blogImage.FileExtension,
					FileName = blogImage.FileName,
					Url = blogImage.Url
				};

				return Ok(blogImageDto);

			}

			return BadRequest(ModelState);
		}

		[HttpGet]

		public async Task<IActionResult> GetAllImages()
		{
			var blogImages = await imageRespository.GetAllAsync();

			var response = new List<BlogImageDto>();

			foreach(var blogImage in blogImages)
			{
				response.Add(new BlogImageDto
				{
					Id = blogImage.Id,
					Title = blogImage.Title,
					DateCreated = blogImage.DateCreated,
					FileExtension = blogImage.FileExtension,
					FileName = blogImage.FileName,
					Url = blogImage.Url
				});
			}

			return Ok(response);
		}

		#region Private region

		private void ValidateFileUpload(IFormFile file)
		{
			var allowedExtenions = new string[] { "jpg", "jpeg", "png" };

			if(allowedExtenions.Contains(Path.GetExtension(file.FileName).ToLower()))
			{
				ModelState.AddModelError("file", "Unsupported file format.");
			}

			if(file.Length > 10485760)
			{
				ModelState.AddModelError("file", "File size cannot be morw than 10MB.");
			}
		}

		#endregion

	}
}
