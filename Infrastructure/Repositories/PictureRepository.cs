using DateJournal.Telegram;

namespace DateJournal.Infrastructure.Repositories
{
	public class PictureRepository
	{
		private readonly string _path;
		public PictureRepository()
		{
			_path = Path.Combine(Directory.GetCurrentDirectory(), "pictures");

			foreach (var folderName in TelegramBot.USERS)
			{
				var path = Path.Combine(_path, folderName);

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
			}
		}

		public FileStream SavePicture(string username, string id, out string path)
		{
			path = Path.Combine(_path, username, id);
			return File.Create(path);
		}

		public FileStream GetPicture(string path)
		{
			return File.OpenRead(path);
		}
	}
}
