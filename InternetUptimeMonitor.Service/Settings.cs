namespace InternetUptimeMonitor.Service
{
	public class Settings
	{
		public string DbFilePath { get; set; }

		public string PingIpsCsv { get; set; }

		public int PollingIntervalMilliseconds { get; set; }
	}
}
