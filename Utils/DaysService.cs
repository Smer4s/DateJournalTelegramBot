using System.Text;

namespace DateJournal.Utils
{
	public static class DaysService
	{
		public static string GetDays(this TimeSpan timeSpan)
		{
			var sb = new StringBuilder();

			sb.AppendLine("Итак, вы терпите друг друга уже");

			if (timeSpan.TotalDays > 365)
			{
				sb.AppendLine(YearsToString(timeSpan) + ", или же");
			}
			if (timeSpan.TotalDays > 30)
			{ 
				sb.AppendLine(MonthesToString(timeSpan) + ", или же");
			}
			if (timeSpan.TotalDays > 0)
			{
				sb.AppendLine(DaysToString(timeSpan) + ", или же");
			}
			if (timeSpan.TotalHours > 0)
			{
				sb.AppendLine(HoursToString(timeSpan) + ", или же");
			}
			if (timeSpan.TotalMinutes > 0)
			{
				sb.AppendLine(MinutesToString(timeSpan) + ", или же");
			}
			if (timeSpan.TotalSeconds > 0)
			{
				sb.AppendLine(SecondsToString(timeSpan) + " 😏.");
			}

			sb.AppendLine("Я думаю этого дорогого стоит, вы молодцы, дерзайте дальше 😘😘😘");

			return sb.ToString();
		}

		private static string MinutesToString(TimeSpan timeSpan)
		{
			var sb = new StringBuilder();
			var minutes = (int)timeSpan.TotalMinutes;
			sb.Append(minutes.ToString() + " ");

			if (minutes % 10 is 1 && minutes % 100 is not 11)
			{
				sb.Append("минуту");
			}
			else if (minutes % 100 >= 5 && minutes % 100 <= 20)
			{
				sb.Append("минут");
			}
			else if (minutes % 10 is 2 || minutes % 10 is 3 || minutes % 10 is 4)
			{
				sb.Append("минуты");
			}
			else
			{
				sb.Append("минут");
			}

			return sb.ToString();
		}

		private static string SecondsToString(TimeSpan timeSpan)
		{
			var sb = new StringBuilder();
			var seconds = (int)timeSpan.TotalSeconds;
			sb.Append(seconds.ToString() + " ");

			if (seconds % 10 is 1 && seconds % 100 is not 11)
			{
				sb.Append("секунду");
			}
			else if (seconds % 100 >= 5 && seconds % 100 <= 20)
			{
				sb.Append("секунд");
			}
			else if (seconds % 10 is 2 || seconds % 10 is 3 || seconds % 10 is 4)
			{
				sb.Append("секунды");
			}
			else
			{
				sb.Append("секунд");
			}

			return sb.ToString();
		}

		private static string DaysToString(TimeSpan timeSpan)
		{
			var sb = new StringBuilder();
			var days = (int)timeSpan.TotalDays;

			sb.Append(days.ToString() + " ");

			if (days % 10 is 1 && days % 100 is not 11)
			{
				sb.Append("день");
			}
			else if (days % 100 >= 5 && days % 100 <= 20)
			{
				sb.Append("дней");
			}
			else if (days % 10 is 2 || days % 10 is 3 || days % 10 is 4)
			{
				sb.Append("дня");
			}
			else
			{
				sb.Append("дней");
			}

			return sb.ToString();
		}

		private static string HoursToString(TimeSpan timeSpan)
		{
			var sb = new StringBuilder();
			var hours = (int)timeSpan.TotalHours;

			sb.Append(hours.ToString() + " ");

			if (hours % 10 is 1 && hours % 100 is not 11)
			{
				sb.Append("час");
			}
			else if (hours % 100 >= 5 && hours % 100 <= 20)
			{
				sb.Append("часов");
			}
			else if (hours % 10 is 2 || hours % 10 is 3 || hours % 10 is 4)
			{
				sb.Append("часа");
			}
			else
			{
				sb.Append("часов");
			}

			return sb.ToString();
		}

		private static string YearsToString(TimeSpan timeSpan)
		{
			var sb = new StringBuilder();
			var years = (int)(timeSpan.TotalDays / 365);
			sb.Append(years.ToString() + " ");

			if (years % 10 is 1 && years % 100 is not 11)
			{
				sb.Append("год");
			}
			else if (years % 100 >= 5 && years % 100 <= 20)
			{
				sb.Append("лет");
			}
			else if (years % 10 is 2 || years % 10 is 3 || years % 10 is 4)
			{
				sb.Append("года");
			}
			else
			{
				sb.Append("лет");
			}

			return sb.ToString();
		}

		private static string MonthesToString(TimeSpan timeSpan)
		{
			var sb = new StringBuilder();
			var monthes = (int)(timeSpan.TotalDays / 30);

			sb.Append(monthes.ToString() + " ");

			if (monthes % 10 is 1 && monthes % 100 is not 11)
			{
				sb.Append("месяц");
			}
			else if (monthes % 100 >= 5 && monthes % 100 <= 20)
			{
				sb.Append("месяцев");
			}
			else if (monthes % 10 is 2 || monthes % 10 is 3 || monthes % 10 is 4)
			{
				sb.Append("месяца");
			}
			else
			{
				sb.Append("месяцев");
			}

			return sb.ToString();
		}
	}
}
