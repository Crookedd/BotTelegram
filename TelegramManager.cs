namespace Bot3;

internal class TelegramManager
{
    public string Token = "5697794511:AAEvb4Q6AKdxt3Br6Q1uqG8SOfLbWNW9m9g";
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
