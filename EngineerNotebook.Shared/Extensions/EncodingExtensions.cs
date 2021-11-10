namespace EngineerNotebook.Shared.Extensions;
public static class EncodingExtensions
{
    public static string ToBase64(this string text)
    {
        var plainBytes = System.Text.Encoding.UTF8.GetBytes(text);
        return System.Convert.ToBase64String(plainBytes);
    }

    public static string FromBase64(this string text)
    {
        var encodedBytes = System.Convert.FromBase64String(text);
        return System.Text.Encoding.UTF8.GetString(encodedBytes);
    }
}
