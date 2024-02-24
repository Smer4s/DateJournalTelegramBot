using DateJournal.Infrastructure.Models;
using DateJournal.Infrastructure.Repositories;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DateJournal.Extensions;

namespace DateJournal.Telegram
{
	public class TelegramBotHandler : IUpdateHandler
	{
		List<UserCache> userCaches = new()
		{
			new () { UserName = "nikitastud" },
			new () { UserName = "neitema" }
		};

		Dictionary<string, Guid> sessions = new Dictionary<string, Guid>();

		List<Story>? stories;
		readonly StoryRepository storyRepository = StoryRepository.Create();
		readonly PictureRepository pictureRepository = PictureRepository.Create();
		readonly CacheRepository cacheRepository = CacheRepository.Create();

		public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
		{
			await Task.Run(() =>
			{
				var ErrorMessage = error switch
				{
					ApiRequestException apiRequestException
						=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
					_ => error.ToString()
				};

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ErrorMessage);
				Console.ForegroundColor = ConsoleColor.White;
			}, cancellationToken);
		}

		public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			try
			{
				var message = update.Message!;
				var username = message.From!.Username!;
				var chat = message.Chat;

				if (!TelegramBot.USERS.Contains(username))
				{
					await botClient.SendTextMessageAsync(
						chat,
						TelegramBot.MESSAGE_RESTRICTED,
						cancellationToken: cancellationToken
						);
					return;
				}

				await cacheRepository.GetCache(userCaches);
				var userCache = userCaches.GetByUserName(username);
				switch (message.Type)
				{
					case MessageType.Text:
						{
							var text = message.Text!;
							if (text.StartsWith('/'))
							{
								await HandleCommand(
									botClient,
									chat,
									text,
									username,
									userCache,
									cancellationToken);
							}
							else if (userCache.IsStory && userCache.SessionId == sessions[username])
							{
								await HandleCreatingStory(
									botClient,
									message,
									username,
									chat,
									text,
									userCache,
									cancellationToken);
							}
							else
							{
								switch (text)
								{
									case "👈":
										userCache.CurrentStoryIndex--;
										await ShowStory(botClient, chat, userCache, cancellationToken);
										break;
									case "👉":
										userCache.CurrentStoryIndex++;
										await ShowStory(botClient, chat, userCache, cancellationToken);
										break;
									default:
										await DontUnderstand(botClient, chat, cancellationToken);
										break;
								}
							}
						}
						break;
					case MessageType.Photo:
						{
							if (userCache.IsStory && userCache.SessionId == sessions[username])
							{
								var text = message.Caption!;

								var photos = await GetPhotoAlbum(botClient, cancellationToken);

								await HandleCreatingStory(
									botClient,
									message,
									username,
									chat,
									text,
									userCache,
									cancellationToken,
									photos);
							}
						}
						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.ToString());
				Console.ForegroundColor = ConsoleColor.White;
			}
			finally
			{
				await cacheRepository.SaveCache(userCaches);
			}
		}

		private static async Task<List<PhotoSize?>> GetPhotoAlbum(ITelegramBotClient botClient, CancellationToken cancellationToken)
		{
			var photos = new List<PhotoSize?>();

			Update[]? updates;
			int offset = 0;
			do
			{
				updates = await botClient.GetUpdatesAsync(
					offset: offset,
					allowedUpdates: new UpdateType[] { UpdateType.Message },
					cancellationToken: cancellationToken);

				foreach (var item in updates)
				{
					offset = item.Id + 1;
					if (item.Message is not null && item.Message.Photo is not null)
					{
						photos.Add(item.Message.Photo.Last());
					}
				}

			} while (updates.Length != 0);

			return photos;
		}

		private static async Task DontUnderstand(ITelegramBotClient botClient, Chat chat, CancellationToken cancellationToken)
		{
			await botClient.SendTextMessageAsync(
				chat,
				TelegramBot.MESSAGE_DONTUNDERSTAND,
				cancellationToken: cancellationToken);
		}

		private async Task HandleCreatingStory(ITelegramBotClient botClient, Message message, string username, Chat chat, string text, UserCache userCache, CancellationToken cancellationToken, List<PhotoSize?>? photos = null)
		{
			var story = new Story
			{
				Created = DateOnly.FromDateTime(message.Date.AddHours(3)),
				Message = text,
				IssuerUsername = username
			};

			if (photos is not null)
			{
				story.PhotoUrls = new List<string>();

				foreach (var photo in photos)
				{
					using var fileStream = pictureRepository.SavePicture(username, photo!.FileId, out string path);
					await botClient.GetInfoAndDownloadFileAsync(photo.FileId, fileStream);
					story.PhotoUrls.Add(path);
				}
			}

			await storyRepository.Create(story);

			userCache.IsStory = false;
			userCache.SessionId = null;
			sessions.Remove(username);

			await botClient.SendTextMessageAsync(
				chat,
				TelegramBot.MESSAGE_SAVED,
				cancellationToken: cancellationToken);

			stories = await storyRepository.GetStories();
		}

		private async Task HandleCommand(ITelegramBotClient botClient, Chat chat, string text, string username, UserCache userCache, CancellationToken cancellationToken)
		{
			switch (text.ToLower())
			{
				case "/newstory":
					await botClient.SendTextMessageAsync(
						chat,
						TelegramBot.MESSAGE_SENDSTORY,
						cancellationToken: cancellationToken);

					userCache.IsStory = true;
					userCache.SessionId = Guid.NewGuid();
					sessions.Add(username, userCache.SessionId.Value);
					break;
				case "/viewstories":
					await ShowStory(botClient, chat, userCache, cancellationToken);
					break;
				case "/start":
					await botClient.SendTextMessageAsync(
						chat,
						TelegramBot.MESSAGE_MAIN,
						cancellationToken: cancellationToken);
					break;
				case "/help":
					await botClient.SendTextMessageAsync(
						chat,
						TelegramBot.COMMANDS.GetStringFromArray(),
						cancellationToken: cancellationToken);
					break;
				case "/days":
					var days = DateTime.Now - TelegramBot.DATE_BEGINDATING;

					await botClient.SendTextMessageAsync(
						chat,
						$"Итак, вы терпите друг друга уже" +
						$"\n{(int)days.TotalDays} дней, или же" +
						$"\n{(int)days.TotalHours} часов, или же" +
						$"\n{(int)days.TotalMinutes} минут, или же" +
						$"\n{(int)days.TotalSeconds} секунд 😏." +
						$"\nЯ думаю этого дорогого стоит, вы молодцы, дерзайте дальше 😘😘😘",
						cancellationToken: cancellationToken);
					break;
				default:
					await botClient.SendTextMessageAsync(
						chat,
						TelegramBot.MESSAGE_NOTCOMMAND,
						cancellationToken: cancellationToken);
					break;
			}
		}

		private async Task ShowStory(ITelegramBotClient botClient, Chat chat, UserCache userCache, CancellationToken cancellationToken)
		{
			stories ??= await storyRepository.GetStories();

			userCache.CurrentStoryIndex = userCache.CurrentStoryIndex < 0 ?
										  userCache.CurrentStoryIndex += stories.Count :
										  userCache.CurrentStoryIndex %= stories.Count;

			var story = stories[userCache.CurrentStoryIndex];

			if (story.PhotoUrls is not null)
			{
				IEnumerable<InputMediaPhoto> enumerable()
				{
					foreach (var photoPath in story.PhotoUrls)
					{
						var fileStream = pictureRepository.GetPicture(photoPath);
						yield return new InputMediaPhoto(InputFile.FromStream(
							fileStream,
							Path.GetFileName(photoPath)));
					}
				}

				var media = enumerable();

				await botClient.SendMediaGroupAsync(
					chat,
					media,
					cancellationToken: cancellationToken);
			}

			await botClient.SendTextMessageAsync(
					chat,
					story.ToString(),
					cancellationToken: cancellationToken,
					replyMarkup: TelegramBot.PicturesKeyboard);
		}
	}
}
