using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InternetUptimeMonitor.Service
{
	public class MainService : IHostedService
	{
		private readonly Timer _timer;
		private readonly IEnumerable<string> _ips;
		private readonly LoggingContext _context;
		private readonly Settings _settings;
		private readonly ILogger<MainService> _logger;

		public MainService(
			LoggingContext context,
			IOptions<Settings> settings,
			ILogger<MainService> logger)
		{
			_context = context;
			_logger = logger;
			_settings = settings.Value;
			_ips = _settings.PingIpsCsv.Split(',');
			var interval = _settings.PollingIntervalMilliseconds;
			_timer = new Timer(interval)
			{
				AutoReset = true
			};
			_timer.Elapsed += PollSites;
		}

		public void PollSites(object sender, ElapsedEventArgs args)
		{
			bool atLeastOneResponded = false;
			bool allResponded = true;

			var ipDict = new Dictionary<string, bool>();

			foreach (var ip in _ips)
			{
				var result = PingHost(ip);
				_logger.LogInformation($"Response from {ip}: {result}");
				ipDict.Add(ip, result);
				atLeastOneResponded |= result;
				allResponded &= result;
			}

			var logMsg =
				$"{(long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds} - at least one responded: {atLeastOneResponded}, all responded: {allResponded}";
			_logger.LogInformation(logMsg);
		}

		public static bool PingHost(string nameOrAddress)
		{
			bool pingable = false;
			
			try
			{
				using (Ping pinger = new Ping())
				{
					PingReply reply = pinger.Send(nameOrAddress);
					pingable = reply?.Status == IPStatus.Success;
				}
			}
			catch (PingException)
			{
				// Discard PingExceptions and return false;
			}

			return pingable;
		}

		/// <summary>
		/// Triggered when the application host is ready to start the service.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_timer.Start();
		}

		/// <summary>
		/// Triggered when the application host is performing a graceful shutdown.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
		public async Task StopAsync(CancellationToken cancellationToken)
		{
			_timer.Stop();
		}
	}
}
