using DateJournal.Telegram;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

UserCredential credential;

using (var stream = new FileStream("C:\\Users\\Nikita\\source\\repos\\DateJournal\\Utils\\credentials.json", FileMode.Open, FileAccess.Read))
{
	string credPath = "token.json";
	credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
		GoogleClientSecrets.Load(stream).Secrets,
		new[] { DriveService.Scope.Drive },
		"user",
		CancellationToken.None,
		new FileDataStore(credPath, true)).Result;
}

var service = new DriveService(new BaseClientService.Initializer()
{
	HttpClientInitializer = credential,
	ApplicationName = "TestGoogleDriveAPI",
});

var fileMetadata = new Google.Apis.Drive.v3.Data.File()
{
	Name = "example.jpg"
};

FilesResource.CreateMediaUpload request;

using (var stream = new FileStream("C:\\Users\\Nikita\\source\\repos\\DateJournal\\bin\\Release\\net7.0\\pictures\\nikitastud\\AgACAgIAAxkBAAIDmmXL_KvEEKpawvAbITp2S50mFFmWAAI71TEb3p5hStFxVKg7q0vPAQADAgADeQADNAQ", FileMode.Open))
{
	request = service.Files.Create(fileMetadata, stream, "image/jpeg");
	request.Upload();
}

var file = request.ResponseBody;
Console.WriteLine("File ID: " + file.Id);


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
