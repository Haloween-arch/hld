namespace UrlShortener.Utils;

public static class Base62Generator
{
    private const string Characters =
        "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string Encode(long number)
    {
        if (number == 0)
            return "0";

        var result = new Stack<char>();

        while (number > 0)
        {
            result.Push(Characters[(int)(number % 62)]);
            number /= 62;
        }

        return new string(result.ToArray());
    }
}