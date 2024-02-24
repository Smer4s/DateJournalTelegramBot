using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateJournal.Infrastructure.Models
{
	public class UserCache : BaseEntity
	{
		public string? UserName {  get; set; }
		public bool IsStory { get; set; } = false;
		public Guid? SessionId { get; set; }
		public int CurrentStoryIndex { get; set; } = 0;
	}
}
