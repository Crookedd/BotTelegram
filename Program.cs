using BotTelegram;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

string TELEGRAM_TOKEN = TelegramManager.Instance.Token;
var BotClient = new TelegramBotClient(TELEGRAM_TOKEN);
using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};
BotClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token
    );
var me = await BotClient.GetMeAsync();
Console.WriteLine("Начало работы c @" + me.Username);
await Task.Delay(int.MaxValue);
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    InlineKeyboardMarkup mainMenu = new InlineKeyboardMarkup(new[] {
        new[] {InlineKeyboardButton.WithUrl(text: "Ссылка на канал ", url: "https://t.me/+idcsDi88AIVmZGNi") } });

    if (update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text)
    {

        var chatID = update.Message.Chat.Id;
        var messageText = update.Message.Text;
        var messageID = update.Message.MessageId;
        var fromID = update.Message.From!.Id;
        string firstName = update.Message.From.FirstName;
        Console.WriteLine($"Получено сообщение: '{messageText}' в чате {chatID}");

        for (int Index = 0; Index < TelegramManager.Instance.ObsceneWord.Count; Index++)
        {
            string Word = TelegramManager.Instance.ObsceneWord[Index];
            if (messageText != null && messageText == Word)
            {
                await botClient.DeleteMessageAsync(chatId: chatID, messageId: messageID,
                      cancellationToken: cancellationToken);
                await BotClient.SendTextMessageAsync(
                chatId: chatID,
                text: $"Я попрошу вас не выражаться, {firstName}!Спасибо,за понимание.",
                cancellationToken: cancellationToken);

            }
        }
        if (messageText == "/start")
        {
            var task = SendPhoto(chatID, cancellationToken);
            task.Wait();
            Message sentMessage = await BotClient.SendTextMessageAsync(
                chatId: chatID,
                text: $"Привет {firstName}! \n\nЯ чат бот беседы" + Environment.NewLine + "Буду очень благодарен если вы подпишитесь на канал моего создателя))",
                replyMarkup: mainMenu,
                cancellationToken: cancellationToken);
        }

        if (messageText.ToLower().Contains("help"))
        {
            Message sentMessage = await BotClient.SendTextMessageAsync(
                chatId: chatID,
                text: $"Несколько вещей,что я могу делать:\n\n" +
                          $"~Удалять сообщения которые содержат в себе ненормативную лексику\n\n" +
                          $"~И отвечать на вопрос <Как дела?>))",
                replyMarkup: mainMenu,
                cancellationToken: cancellationToken);
        }
        if (messageText.ToLower().Contains("Как дела?"))
        {
            Message sentMessage = await BotClient.SendTextMessageAsync(
                chatId: chatID,
                text: "Всё хорошо!! Спасибо,что спросили!",
                cancellationToken: cancellationToken);
        }
    }
}

//нужен для обработки ошибок в случае обновлений.
Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMesssage = exception switch
    {
        ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMesssage);
    return Task.CompletedTask;
}

async Task SendPhoto(long chatId, CancellationToken token)
{
    Message message = await BotClient.SendPhotoAsync(
        chatId: chatId,
        photo: "https://sun9-38.userapi.com/impg/uLwl_vAX2Z1kbUWK6107AVPmiLobfSj_4gS2Hg/jhJESM6jLD8.jpg?size=564x543&quality=96&sign=6d9cbd6a2a2446ccf18cefbf143298c2&type=album",
        parseMode: ParseMode.Html,
        cancellationToken: token);
}