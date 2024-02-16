using System.Globalization;

namespace DateJournal.Utils
{
	public class CustomFormatProvider : IFormatProvider
	{
		public object? GetFormat(Type? formatType)
		{
			if (formatType == typeof(DateTimeFormatInfo))
			{
				var dateTimeFormatInfo = (DateTimeFormatInfo)CultureInfo.InvariantCulture.DateTimeFormat.Clone();
				dateTimeFormatInfo.ShortDatePattern = "dd.MM.yyyy";
				dateTimeFormatInfo.ShortTimePattern = "HH:mm";
				return dateTimeFormatInfo;
			}

			return null;
		}
	}
}
