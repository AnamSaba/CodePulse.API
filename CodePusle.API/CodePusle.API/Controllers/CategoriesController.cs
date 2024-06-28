using AutoMapper;
using CodePusle.API.Data;
using CodePusle.API.Models.Domain;
using CodePusle.API.Models.DTO;
using CodePusle.API.Repositories.Inteface;
using Microsoft.AspNetCore.Authorization;
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
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto createCategoryRequestDto)
		{
			var categoryDomain = mapper.Map<Category>(createCategoryRequestDto);

			categoryDomain = await categoriesRepository.CreateAsync(categoryDomain);

			var catergoryDto = mapper.Map<CategoryDto>(categoryDomain);

			return Ok(catergoryDto);

		}

		//GET: https://localhost:7145/api/Categories

		[HttpGet]
		
		public async Task<IActionResult> GetAllCategory()
		{
			var categories = await categoriesRepository.GetAllAsync();

			// Map Domain model to Dto

			var catergoriesDto = mapper.Map<List<CategoryDto>>(categories);

			return Ok(catergoriesDto);
		}

		//GET: https://localhost:7145/api/Categories/{id}

		[HttpGet]
		[Route("{id:guid}")]

		public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
		{
			var categoryDomain = await categoriesRepository.GetByIdAsync(id);

			if(categoryDomain == null)
			{
				return NotFound();
			}

			var catergoryDto = mapper.Map<CategoryDto>(categoryDomain);

			return Ok(catergoryDto);
		}

		[HttpPut]
		[Route("{id:guid}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto updateCategoryRequestDto)
		{
			var categoryDomain = mapper.Map<Category>(updateCategoryRequestDto);

			categoryDomain = await categoriesRepository.UpdateAsync(id,categoryDomain);

			if(categoryDomain == null)
			{
				return NotFound();
			}

			var categoryDto = mapper.Map<CategoryDto>(categoryDomain);
			return Ok(categoryDto);
		}


		[HttpDelete]
		[Route("{id:guid}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> DeleteCategory([FromRoute]Guid id)
		{
			var categoryDomain = await categoriesRepository.DeleteAsync(id);

			if(categoryDomain == null)
			{
				return NotFound();
			}

			var categoryDto = mapper.Map<CategoryDto>(categoryDomain);
			return Ok(categoryDto);
		}

	}
}
