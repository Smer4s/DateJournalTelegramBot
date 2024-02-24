namespace DateJournal.Infrastructure.Models
{
	public class Story : BaseEntity
	{
		public DateOnly Created { get; set; }
		public TimeOnly? Time { get; set; }
		public string IssuerUsername { get; set; } = null!;
		public string Message { get; set; } = null!;
		public List<string>? PhotoUrls { get; set; } = null;

		public override string ToString()
		{
			return $"Автор: @{IssuerUsername}\n" +
				$"{Created.ToLongDateString()} {(Time.HasValue ? Time.ToString() : string.Empty)}\n" +
				$"{Message}";
		}
	}
}
