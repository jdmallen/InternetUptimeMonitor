using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternetUptimeMonitor.Service.Models
{
	[Table("LogEntry")]
	public class LogEntry
	{
		[Key]
		[Column("Id", TypeName = "BIGINT")]
		public long Id { get; set; }
		
		[Column("Date", TypeName = "DATETIME")]
		public DateTime Date { get; set; }
		
		[Column("Level", TypeName = "VARCHAR(50)")]
		public string Level { get; set; }
		
		[Column("Logger", TypeName = "VARCHAR(255)")]
		public string Logger { get; set; }
		
		[Column("Message", TypeName = "TEXT")]
		public string Message { get; set; }
	}
}
