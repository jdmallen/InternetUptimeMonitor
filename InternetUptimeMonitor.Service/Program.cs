using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using InternetUptimeMonitor.Service.DbLogProvider;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace InternetUptimeMonitor.Service
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			// BEGIN INITIALIZATION STUFF //

			var cancellationToken = new CancellationToken(false);

			var host = new HostBuilder()
				.ConfigureHostConfiguration(configHost =>
				{
					configHost.SetBasePath(Directory.GetCurrentDirectory());
					configHost.AddJsonFile("hostsettings.json", true);
					configHost.AddEnvironmentVariables();
				})
				.ConfigureAppConfiguration((hostContext, configApp) =>
				{
					configApp.AddJsonFile("appsettings.json", true, true);
					configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json");
					configApp.AddEnvironmentVariables();
					if (hostContext.HostingEnvironment.IsDevelopment())
						configApp.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddOptions();
					services.Configure<Settings>(hostContext.Configuration);
					var connStr = new SqliteConnectionStringBuilder
					{
						DataSource = hostContext.Configuration["DbFilePath"]
					}.ConnectionString;
					services.AddDbContext<LoggingContext>(contextOptions =>
					{
						contextOptions.UseSqlite(connStr);
					});
					services.AddLogging();
					services.AddHostedService<MainService>();
				})
				.ConfigureLogging((hostContext, configLogging) =>
				{
					configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
					configLogging.AddConsole();
					configLogging.AddDebug();
					var dbContext = configLogging.Services.BuildServiceProvider()
												.GetRequiredService<LoggingContext>();
					configLogging.AddContext(LogLevel.Information, dbContext);
					dbContext.Database.EnsureCreated();
				})
				.UseConsoleLifetime()
				.Build();

			await host.RunAsync(cancellationToken);
		}
	}
}
