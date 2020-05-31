using ManchkinWebApi.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Azure;

namespace ManchkinWebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
				builder
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowAnyOrigin();
				//.WithOrigins("http://192.168.1.12:8080/");
			}));


			services.AddAzureClients(builder =>
			{
				builder.AddBlobServiceClient(Configuration["ConnectionStrings:gamewebapistorage"]);
			}); 
			services.AddSignalR(hubOptions =>
			{
				hubOptions.EnableDetailedErrors = true;
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("CorsPolicy");

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<GameHub>("/gameHub");
			});
		}
	}
}
