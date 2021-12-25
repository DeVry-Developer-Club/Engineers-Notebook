namespace EngineerNotebook.Shared;
public class ResultOf<T>
{
    /// <summary>
    /// Value that shall be returned
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Message (if applicable)
    /// </summary>
    public string ErrorMessage { get; init; }

    /// <summary>
    /// HTTP Status code for what happened
    /// </summary>
    public int StatusCode { get; init; } = 200;

    /// <summary>
    /// Was the message successful (must be in range of 200 + no error message)
    /// </summary>
    public bool Success => ErrorMessage == string.Empty || StatusCode is >= 200 and <= 299;

    public static ResultOf<T> Failure(string errorMessage, int statusCode = 404) =>
        new()
        {
            ErrorMessage = errorMessage,
            StatusCode = statusCode
        };

    public static ResultOf<T> Failure(int statusCode) =>
        new()
        {
            StatusCode = statusCode
        };
}
