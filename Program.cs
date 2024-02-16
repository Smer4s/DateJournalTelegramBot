using DateJournal.Telegram;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

var _botClient = new TelegramBotClient(TelegramBot.TOKEN);
var _receiverOptions = new ReceiverOptions()
{
	AllowedUpdates = new UpdateType[]
	{
		UpdateType.Message
	},
	ThrowPendingUpdates = false,
};

_botClient.StartReceiving(new TelegramBotHandler(), _receiverOptions);

Console.WriteLine("Bot has been started. \nPress any key to stop.");
Console.ReadKey();
