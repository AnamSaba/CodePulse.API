using CodePusle.API.Data;
using CodePusle.API.Mappings;
using CodePusle.API.Repositories.Implementation;
using CodePusle.API.Repositories.Inteface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

		builder.Services.AddDbContext<AuthDbContext>(options =>
		{
			options.UseSqlServer(builder.Configuration.GetConnectionString("CodePulseConnectionString"));
		});

		builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
		builder.Services.AddScoped<ICategoriesRepository, CategoryRepository>();
		builder.Services.AddScoped<IBlogPostRepository, BlogPostRespository>();
		builder.Services.AddScoped<IImageRespository, ImageRepository>();
		builder.Services.AddScoped<ITokenRespository, TokenRespository>();

		builder.Services.AddIdentityCore<IdentityUser>()
		.AddRoles<IdentityRole>()
		.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("CodePulse")
		.AddEntityFrameworkStores<AuthDbContext>()
		.AddDefaultTokenProviders();

		builder.Services.Configure<IdentityOptions>(options =>
		{
			options.Password.RequireDigit = false;
			options.Password.RequireLowercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequiredLength = 6;
			options.Password.RequiredUniqueChars = 1;
		});

		builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(options =>
				  options.TokenValidationParameters = new TokenValidationParameters
				  {
					  ValidateIssuer = true,
					  ValidateAudience = true,
					  ValidateLifetime = true,
					  ValidateIssuerSigningKey = true,
					  ValidIssuer = builder.Configuration["Jwt:Issuer"],
					  ValidAudience = builder.Configuration["Jwt:Audience"],
					  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
				  });

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

		app.UseAuthentication();
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