using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JDMallen.Toolbox.Extensions;
using JDMallen.Toolbox.Interfaces;

namespace InternetUptimeMonitor.Service.Models
{
	[Table("ConnectionEventType")]
	public class ConnectionEventType : IEntityModel<int>
	{
		[Key]
		[Column(TypeName = "INTEGER")]
		public int Id { get; set; }

		[Column(TypeName = "VARCHAR(50)")]
		public string TypeName { get; set; }

		[NotMapped]
		public EventType Type
		{
			get
			{
				var ok = Enum.TryParse<EventType>(TypeName, true, out var type);
				return ok ? type : EventType.Unknown;
			}
			set => TypeName = value.ToString("G");
		}

		[NotMapped]
		public DateTime DateCreated
		{
			get => DateCreatedUnix.FromUnixTimeMillis();
			set => DateCreatedUnix = value.ToUnixTimeMillis();
		}
		
		[Column("DateCreated")]
		public long DateCreatedUnix { get; set; }

		[NotMapped]
		public DateTime DateModified
		{
			get => DateModifiedUnix.FromUnixTimeMillis();
			set => DateModifiedUnix = value.ToUnixTimeMillis();
		}
		
		[Column("DateModified")]
		public long DateModifiedUnix { get; set; }
	}
}
