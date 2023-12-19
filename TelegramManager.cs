namespace BotTelegram;

internal class TelegramManager
{
    public string Token = "Token";
    public List<string> ObsceneWord = new List<string>() { "мат", "mat", "цензура" };
    public static TelegramManager Instance
    {
        get
        {
            if (instance == null) instance = new TelegramManager();
            return instance;
        }
    }
    private static TelegramManager instance;
}
