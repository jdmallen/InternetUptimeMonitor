using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InternetUptimeMonitor.Service
{

	public class LoggingContext : DbContext
	{
		public LoggingContext(DbContextOptions options)
			: base(options)
		{
		}

		public IQueryable<T> GetQueryable<T>()
			where T : class => Set<T>();
	}
}
