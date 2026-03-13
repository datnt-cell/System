using Newtonsoft.Json;

public class Error
{
    [JsonProperty]
    private Errors code;

    [JsonProperty]
    private string message;

    [JsonIgnore]
    public Errors Code
    {
        get
        {
            return code;
        }
        set
        {
            code = value;
        }
    }

    [JsonIgnore]
    public string Message
    {
        get
        {
            return message;
        }
        set
        {
            message = value;
        }
    }

    public override string ToString()
    {
        return $"[{Code}] {Message}";
    }
}