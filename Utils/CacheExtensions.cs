using DateJournal.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DateJournal.Extensions
{
	public static class CacheExtensions
	{
		public static UserCache GetByUserName(this IEnumerable<UserCache> userCaches, string username)
		{
			return userCaches.First(x => x.UserName == username);
		}

		public static string GetStringFromArray(this IEnumerable<string> strings)
		{
			var sb = new StringBuilder();

			foreach (var item in strings)
			{
				sb.AppendLine(item);
			}

			return sb.ToString();
		}
	}
}
