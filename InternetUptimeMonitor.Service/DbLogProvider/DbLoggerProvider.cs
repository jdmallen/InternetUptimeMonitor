using System;
using Microsoft.Extensions.Logging;

namespace InternetUptimeMonitor.Service.DbLogProvider
{
	public class DbLoggerProvider: ILoggerProvider
	{
		private readonly Func<string, LogLevel, bool> _filter;
		private readonly LoggingContext _context;

		public DbLoggerProvider(Func<string, LogLevel, bool> filter, LoggingContext context)
		{
			_filter = filter;
			_context = context;
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			
		}

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
		/// </summary>
		/// <param name="categoryName">The category name for messages produced by the logger.</param>
		/// <returns></returns>
		public ILogger CreateLogger(string categoryName)
			=> new DbLogger(categoryName, _filter, _context);
	}
}
