namespace Communication.Responses;

public class ResponseLoginUserJson
{
    public string Name { get; set; } = string.Empty;
    public ResponseTokensJson Tokens { get; set; } = default!;
}