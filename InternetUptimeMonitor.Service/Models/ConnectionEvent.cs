using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JDMallen.Toolbox.Extensions;
using JDMallen.Toolbox.Infrastructure.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetUptimeMonitor.Service.Models
{
	[Table("ConnectionEvent")]
	public class ConnectionEvent : IComplexEntityModel<int>
	{
		[Key]
		[Column("Id", TypeName = "INTEGER")]
		public int Id { get; set; }

		[Column("ConnectionEventTypeId", TypeName = "INTEGER")]
		public int ConnectionEventTypeId { get; set; }
		
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

		public void OnModelCreating(ModelBuilder modelBuilder)
		{
//			throw new NotImplementedException();
		}
	}
}
