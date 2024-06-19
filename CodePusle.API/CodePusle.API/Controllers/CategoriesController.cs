using AutoMapper;
using CodePusle.API.Data;
using CodePusle.API.Models.Domain;
using CodePusle.API.Models.DTO;
using CodePusle.API.Repositories.Inteface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePusle.API.Controllers
{
    //https:localhost:xxxx/api/categories

    [Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesRepository categoriesRepository;
		private readonly IMapper mapper;

		public CategoriesController(ICategoriesRepository categoriesRepository, IMapper mapper)
        {
			this.categoriesRepository = categoriesRepository;
			this.mapper = mapper;
		}

		// POST: 

		[HttpPost]

		public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto createCategoryRequestDto)
		{
			var categoryDomain = mapper.Map<Category>(createCategoryRequestDto);

			categoryDomain = await categoriesRepository.CreateAsync(categoryDomain);

			var catergoryDto = mapper.Map<CategoryDto>(categoryDomain);

			return Ok(catergoryDto);

		}
    }
}
