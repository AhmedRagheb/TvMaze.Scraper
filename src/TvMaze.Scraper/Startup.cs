using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TvShows.Scraper.Api.Infrastructure;
using TvShows.Scraper.Api.Settings;

namespace TvShows.Scraper
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			CurrentEnvironment = env;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment CurrentEnvironment { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddResponseCompression(options => { options.EnableForHttps = true; });

			services.AddMvc();
			services.AddControllers();
			services.AddDatabase(Configuration.GetSection("ConnectionString").Value);

			services.AddDependencyInjection();
			services.AddConfigurations(Configuration);

			services.AddSwagger();

			services.AddHttpClient();
			services.AddTvMazeHttpClient();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMiddleware<ErrorHandlingMiddleware>();

			app.UseResponseCompression();

			app.UseHttpsRedirection();

			app.UseRouting();
			app.UseDatabase();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwaggerSettings();
		}
	}
}
