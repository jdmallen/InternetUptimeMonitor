using System;
using Microsoft.Extensions.Logging;

namespace InternetUptimeMonitor.Service.DbLogProvider
{
	public static class DbLoggerExtensions
	{
		public static ILoggingBuilder AddContext(
			this ILoggingBuilder factory,
			Func<string, LogLevel, bool> filter,
			LoggingContext context)
		{
			factory.AddProvider(new DbLoggerProvider(filter, context));
			return factory;
		}

		public static ILoggingBuilder AddContext(
			this ILoggingBuilder factory,
			LogLevel minLevel,
			LoggingContext context)
		{
			return AddContext(
				factory,
				(_, logLevel) => logLevel >= minLevel, context);
		}
	}
}
