using AutoMapper;
using CodePusle.API.Models.Domain;
using CodePusle.API.Models.DTO;

namespace CodePusle.API.Mappings
{
	public class AutoMapperProfiles: Profile
	{
        public AutoMapperProfiles()
        {
            CreateMap<Category, CreateCategoryRequestDto>()
                .ReverseMap();
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryRequestDto>().ReverseMap();
            CreateMap<CreateBlogPostRequestDto, BlogPost>().ReverseMap();
			CreateMap<UpdateBlogPostRequestDto, BlogPost>().ReverseMap();
			CreateMap<BlogPost, BlogPostDto>().ReverseMap();
        }
    }
}
