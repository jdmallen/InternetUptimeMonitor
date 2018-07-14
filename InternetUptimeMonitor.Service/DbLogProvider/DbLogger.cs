using System;
using InternetUptimeMonitor.Service.Models;
using Microsoft.Extensions.Logging;

namespace InternetUptimeMonitor.Service.DbLogProvider
{
	public class DbLogger : ILogger
	{
		private readonly string _categoryName;
		private readonly Func<string, LogLevel, bool> _filter;
		private readonly LoggingContext _context;

		public DbLogger(
			string categoryName, 
			Func<string, LogLevel, bool> filter, 
			LoggingContext context)
		{
			_categoryName = categoryName;
			_filter = filter;
			_context = context;
		}

		/// <summary>Writes a log entry.</summary>
		/// <param name="logLevel">Entry will be written on this level.</param>
		/// <param name="eventId">Id of the event.</param>
		/// <param name="state">The entry to be written. Can be also an object.</param>
		/// <param name="exception">The exception related to this entry.</param>
		/// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}
			if (formatter == null)
			{
				throw new ArgumentNullException(nameof(formatter));
			}
			var message = formatter(state, exception);
			if (string.IsNullOrEmpty(message))
			{
				return;
			}
			if (exception != null)
			{
				message += Environment.NewLine + exception;
			}
			LogEntry eventLog = new LogEntry
			{
				Message = message,
				EventId = eventId.Id,
				LogLevel = logLevel.ToString(),
				DateCreated = DateTime.UtcNow,
				DateModified = DateTime.UtcNow
			};
			_context.Add(eventLog);
			_context.SaveChanges();
		}

		/// <summary>
		/// Checks if the given <paramref name="logLevel" /> is enabled.
		/// </summary>
		/// <param name="logLevel">level to be checked.</param>
		/// <returns><c>true</c> if enabled.</returns>
		public bool IsEnabled(LogLevel logLevel) 
			=> _filter == null || _filter(_categoryName, logLevel);

		/// <summary>Begins a logical operation scope.</summary>
		/// <param name="state">The identifier for the scope.</param>
		/// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
		public IDisposable BeginScope<TState>(TState state) => null;
	}
}
