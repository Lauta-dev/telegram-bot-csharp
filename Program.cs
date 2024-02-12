using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using JsonReader;


namespace TelegramBot
{
  class Program
  {

    async static Task Main()
    {
      await new TelegramBotPuntoNet().Iniciar();
    }
class TelegramBotPuntoNet
{
  string[] commands = { "/start", "/config", "/date", "/close", "/commands" };

  async public Task Iniciar()
  {
    // Obtener la API_KEY
    string apiKey = ReadJsonFile.getApiKey();
    string API = apiKey;
    using CancellationTokenSource cts = new ();

    var botClient = new TelegramBotClient(API);

    // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
    ReceiverOptions receiverOptions = new ()
    {
      AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
    };

    botClient.StartReceiving(
      updateHandler: HandleUpdateAsync,
      pollingErrorHandler: HandlePollingErrorAsync,
      receiverOptions: receiverOptions,
      cancellationToken: cts.Token
    );

    var me = await botClient.GetMeAsync();

    Console.WriteLine($"Start listening for @{me.Username}");
    Console.ReadLine();

    // Send cancellation request to stop bot
    cts.Cancel();
  }

  async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
  {

    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    string a = messageText.StartsWith("/")
      ? commands.Contains(messageText) ? Ver.Elegir(messageText) : "El comando no existe"
      : $"Tu comentario es: {messageText}";

    // Echo received message text
    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: a,
        replyMarkup: Buttons(),
        cancellationToken: cancellationToken
    );
  }
  Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
  {
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
  }


  // Creacion de los botones
  ReplyKeyboardMarkup Buttons()
  {
    // Estos son los comandos
    var buttonsRows = new List<KeyboardButton[]>();

    // Se hace un foreach para añadir cada unno de los botones
    foreach(string command in commands)
    {
      // Se crea un boton
      var button = new KeyboardButton(command);
      
      // Añade el boton al array
      buttonsRows.Add(new KeyboardButton[] { command });
    }

    ReplyKeyboardMarkup replyKeyboardMarkup = new(buttonsRows.ToArray())
    {
      ResizeKeyboard = true
    };


    return replyKeyboardMarkup;
  }

}








class Ver
{
  public static string Elegir(string command)
  {
    string[] commands = {"/start", "/config", "/date", "/close", "/commands"};
    if (command == commands[0])
    {
      return "Iniciar la app";
    }

    if (command == commands[1])
    {
      return "Configurar la app";
    }

    if (command == commands[2])
    {
      return DateTime.Now.ToString();
    }

    if (command == commands[3])
    {
      return "Cerrar la app";
    }

    if (command == commands[4])
    {
      string result = string.Join("\n", commands);
      return result;
    }


    return "";
  }
}
  }
}



