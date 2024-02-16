using Telegram.Bot.Types;
using Telegram.Bot;
using DateJournal.Infrastructure.Models;
using DateJournal.Infrastructure.Repositories;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using MongoDB.Bson.Serialization.Serializers;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using DateJournal.Utils;

namespace DateJournal.Telegram
{
	public class TelegramBot
	{
		public const string TOKEN = "6445300231:AAHjTPUBcBPLpdvUy_MbRxERiEFxxBipgE4";
		public static readonly IReadOnlyCollection<string> USERS = new string[] { "neitema", "nikitastud" };
		public const string MESSAGE_RESTRICTED = "Извините, у вас нет доступа(";
		public const string MESSAGE_SENDSTORY = "Расскажите мне, как вы провели сегодняшний день или просто ваши мысли. Учтите, что сообщение сохраняется \bсразу\b и не подлежит изменению 🙃.\n" +
			"Также мне можно присылать фото (желательно 1-2), а я сохраню ваше сообщение в архив.";
		public const string MESSAGE_NOTCOMMAND = "Извините, но такой команды не существует";
		public const string MESSAGE_SAVED = "История успешно сохранена!";
		public const string MESSAGE_DONTUNDERSTAND = "Извините, я вас не понимаю 🤔";
		public const string MESSAGE_BEGINORDATE = "Выберите, хотите посмотреть сначала или с какой-то даты?";
		private const string MESSAGE_SENDDATE = "Пришлите дату в формате: " + DATE_FORMAT;
		public const string MESSAGE_MAIN = "Привет, я бот, который помогает вести журнал отношений. В моем арсенале есть набор команд, которые ты можешь увидеть написав /help или тыкнув на набор команд слева от чата. Думаю, я смогу чем-то помочь тебе 😉";
		private const string DATE_FORMAT = "дд.мм.гггг";
		private const string DATE_FORMAT_ENG = "dd.MM.yyyy";
		private const string STICKER_OK = "👍";
		private const string STICKER_BAD = "👎";
		public static readonly DateTime DATE_BEGINDATING = DateTime.Parse("17.12.2023 19:30", new CustomFormatProvider());
		public static readonly string[] COMMANDS = new string[]
		{
			"Список команд: ",
			"/help",
			"/newstory",
			"/viewstories",
			"/days"
		};
		public static readonly ReplyKeyboardMarkup PicturesKeyboard = new(
			new List<KeyboardButton[]>()
			{
				new KeyboardButton[]
				{
					new KeyboardButton("👈"),
					new KeyboardButton("👉")
				}
			})
			{
				ResizeKeyboard = true
			};

		private readonly TelegramBotClient _botClient;
		private readonly CacheRepository _cacheRepository;
		private readonly StoryRepository _storyRepository;
		private Dictionary<string, UserCache> userCache;
		private int offset = 0;

		//public TelegramBot()
		//{
		//	_botClient = new TelegramBotClient(TOKEN);
		//	_cacheRepository = CacheRepository.Create();
		//	_storyRepository = StoryRepository.Create();
		//	userCache = new Dictionary<string, UserCache>()
		//	{
		//		{
		//			"nikitastud", new UserCache()
		//			{
		//				UserName = "nikitastud",
		//				CurrentStep = Step.Command,
		//			}
		//		},
		//		{
		//			"neitema", new UserCache()
		//			{
		//				UserName = "neitema",
		//				CurrentStep = Step.Command,
		//			}
		//		}
		//	};
		//}

		//public async Task Start()
		//{
		//	userCache = await _cacheRepository.GetCache();
		//	while (true)
		//	{
		//		var message = await WaitForMessage();
		//		var userName = message.Chat.Username!;


		//	}
		//}

		//private async Task<Message> WaitForMessage()
		//{
		//	while (true)
		//	{
		//		var updates = await _botClient.GetUpdatesAsync(offset);

		//		foreach (var update in updates)
		//		{
		//			offset = update.Id + 1;
		//			if (update.Message is not null)
		//			{
		//				return update.Message;
		//			}
		//		}
		//	}
		//}



		//private async Task HandleMessage(Message message)
		//{
		//	var userName = message.Chat.Username!;
		//	if (!userCache.TryGetValue(userName, out var cache) && cache is not null)
		//	{
		//		string reply = cache.CurrentStep switch
		//		{
		//			Step.Command => HandleCommand(message),
		//			Step.SendingData => await HandleSendingData(message),
		//			Step.Confirm => await HandleConfirm(),
		//			_ => await HandleError(),
		//		};

		//		cache.CurrentStep = (Step)((int)(cache.CurrentStep + 1) % 3);
		//		await _cacheRepository.ChangeCache(userName, cache);

		//		await _botClient.SendTextMessageAsync(message.Chat.Id, reply);
		//	}
		//}

		//private Task<string> HandleError()
		//{
		//	throw new NotImplementedException();
		//}

		//private Task<string> HandleConfirm()
		//{
		//	throw new NotImplementedException();
		//}

		//private Task<string> HandleSendingData(Message message)
		//{

		//}
	}
}
