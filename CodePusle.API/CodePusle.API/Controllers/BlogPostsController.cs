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
	public class BlogPostsController : ControllerBase
	{
		private readonly IMapper mapper;
		private readonly IBlogPostRepository blogPostRepository;
		private readonly ICategoriesRepository categoriesRepository;

		public BlogPostsController(IMapper mapper, 
			IBlogPostRepository blogPostRepository,
			ICategoriesRepository categoriesRepository)
        {
			this.mapper = mapper;
			this.blogPostRepository = blogPostRepository;
			this.categoriesRepository = categoriesRepository;
		}

        // POST: {apiBaseUrl}/api/blogposts

        [HttpPost]

        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto createBlogPostRequestDto)
        {
			var blogPostDomain = mapper.Map<BlogPost>(createBlogPostRequestDto);

			foreach(var item in createBlogPostRequestDto.CategoriesCollection)
			{
				var existingCategories = await categoriesRepository.GetByIdAsync(item);

				if (existingCategories != null)
				{
					blogPostDomain.Categories.Add(existingCategories);
				}
			}

			blogPostDomain = await blogPostRepository.CreateAsync(blogPostDomain);

			var blogPostDto = mapper.Map<BlogPostDto>(blogPostDomain);

			return Ok(blogPostDto);
        }

		[HttpGet]

		public async Task<IActionResult> GetAllBlogPost()
		{
			var blogPosts = await blogPostRepository.GetAllAsync();

			var blogPostDtos = mapper.Map<List<BlogPostDto>>(blogPosts);

			return Ok(blogPostDtos);
		}

		// GET: {apiBaseUrl}/api/blogposts/{id}

		[HttpGet]
		[Route("{id:guid}")]
		public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
		{
			var blogPostDomain = await blogPostRepository.GetByIdAsync(id);

			if(blogPostDomain == null)
			{
				return NotFound();
			}
			var blogPostDtos = mapper.Map<BlogPostDto>(blogPostDomain);

			return Ok(blogPostDtos);
		}

		[HttpPut]
		[Route("{id:guid}")]

		public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, [FromBody] UpdateBlogPostRequestDto requestDto)
		{
			var blogPostDomain = mapper.Map<BlogPost>(requestDto);

			foreach (var item in requestDto.CategoriesCollection)
			{
				var existingCategories = await categoriesRepository.GetByIdAsync(item);

				if (existingCategories != null)
				{
					blogPostDomain.Categories.Add(existingCategories);
				}
			}

			blogPostDomain = await blogPostRepository.UpdateAsync(id, blogPostDomain);

			if(blogPostDomain == null)
			{
				return NotFound();
			}

			var blogPostDto = mapper.Map<BlogPostDto>(blogPostDomain);

			return Ok(blogPostDto);

		}

		[HttpDelete]
		[Route("{id:guid}")]

		public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
		{
			var blogPostDomain = await blogPostRepository.DeleteAsync(id);

			if( blogPostDomain == null)
			{
				return NotFound();
			}

			var blogPostDto = mapper.Map<BlogPostDto>(blogPostDomain);

			return Ok(blogPostDto);
		}


		// GET: {apiBaseUrl}/api/blogposts/{urlHandle}

		[HttpGet]
		[Route("{urlHandle}")]

		public async Task<IActionResult> GetByUrlHandle([FromRoute] string urlHandle)
		{
			var blogPostDomain = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

			if (blogPostDomain == null)
			{
				return NotFound();
			}
			var blogPostDtos = mapper.Map<BlogPostDto>(blogPostDomain);

			return Ok(blogPostDtos);
		}

	}
}
