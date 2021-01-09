using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IV.PCM.PowerManagement;
using IV.PCM.OSProcesses;

namespace IV.PCM.Worker
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		[UsedImplicitly]
		public static void ConfigureServices(IServiceCollection services)
		{
			services
				.AddControllers()
				.AddControllersAsServices();

			services.AddTransient<IPowerManagementService, PowerManagementService>();
			services.AddTransient<IProcessRunner, ProcessRunner>();
		}

		[UsedImplicitly]
		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
	}
}