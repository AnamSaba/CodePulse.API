using CodePusle.API.Data;
using CodePusle.API.Mappings;
using CodePusle.API.Repositories.Implementation;
using CodePusle.API.Repositories.Inteface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services.AddControllers();

		builder.Services.AddHttpContextAccessor();

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlServer(builder.Configuration.GetConnectionString("CodePulseConnectionString"));
		});

		builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
		builder.Services.AddScoped<ICategoriesRepository, CategoryRepository>();
		builder.Services.AddScoped<IBlogPostRepository, BlogPostRespository>();
		builder.Services.AddScoped<IImageRespository, ImageRepository>();

var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseCors(options =>
		{
			options.AllowAnyHeader();
			options.AllowAnyOrigin();
			options.AllowAnyMethod();
		});

		app.UseAuthorization();

		app.UseStaticFiles(new StaticFileOptions
		{
			FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
			RequestPath = "/Images"
		});

		app.MapControllers();

		app.Run();
	}
}