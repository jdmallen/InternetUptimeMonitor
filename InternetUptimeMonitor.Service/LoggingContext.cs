using InternetUptimeMonitor.Service.Models;
using JDMallen.Toolbox.Infrastructure.EFCore.Config;
using Microsoft.EntityFrameworkCore;

namespace InternetUptimeMonitor.Service
{

	public class LoggingContext : EFContextBase
	{
		public LoggingContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Model.AddEntityType(typeof(LogEntry));
			modelBuilder.Model.AddEntityType(typeof(ConnectionEvent));
			base.OnModelCreating(modelBuilder);
		}
	}
}
