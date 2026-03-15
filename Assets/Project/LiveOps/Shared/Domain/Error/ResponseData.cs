using System;
using Newtonsoft.Json;

[Serializable]
public class ResponseData
{
    [JsonProperty("success")]
    private bool success;

    [JsonProperty("error")]
    private Error error;

    [JsonIgnore]
    public bool Success
    {
        get => success;
        set => success = value;
    }

    [JsonIgnore]
    public Error Error
    {
        get => error;
        set => error = value;
    }

    [JsonIgnore]
    public object CustomData { get; set; }

    // =========================
    // FACTORY
    // =========================

    public static T GetSuccessResponse<T>() where T : ResponseData, new()
    {
        return new T
        {
            Success = true
        };
    }

    public static T GetErrorResponse<T>(Errors errorCode, string message = "")
        where T : ResponseData, new()
    {
        return new T
        {
            Success = false,
            Error = new Error
            {
                Code = errorCode,
                Message = string.IsNullOrEmpty(message)
                    ? errorCode.ToString()
                    : message
            }
        };
    }

    public static T GetErrorResponse<T>(int code, string message = "")
        where T : ResponseData, new()
    {
        return GetErrorResponse<T>((Errors)code, message);
    }

    public static T GetErrorResponse<T>(ResponseData data)
        where T : ResponseData, new()
    {
        if (data?.Error == null)
        {
            return GetErrorResponse<T>(
                Errors.Unknown,
                "Unknown error"
            );
        }

        return GetErrorResponse<T>(
            data.Error.Code,
            data.Error.Message
        );
    }
}