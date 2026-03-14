
using System;
using Newtonsoft.Json;

[Serializable]
public class ResponseData
{
    [JsonProperty]
    private bool success;

    [JsonProperty]
    private Error error;

    [JsonIgnore]
    public bool Success
    {
        get
        {
            return success;
        }
        set
        {
            success = value;
        }
    }

    [JsonIgnore]
    public Error Error
    {
        get
        {
            return error;
        }
        set
        {
            error = value;
        }
    }

    [JsonIgnore]
    public object CustomData { get; set; }

    public static T GetErrorResponse<T>(ResponseData data) where T : ResponseData, new()
    {
        return GetErrorResponse<T>(data.Error.Code, string.IsNullOrEmpty(data.Error.Message) ? data.Error.ToString() : data.Error.Message);
    }

    public static T GetErrorResponse<T>(Errors errorCode, string message = "") where T : ResponseData, new()
    {
        return new T
        {
            Success = false,
            Error = new Error
            {
                Code = errorCode,
                Message = (string.IsNullOrEmpty(message) ? errorCode.ToString() : message)
            }
        };
    }

    public static T GetErrorResponse<T>(int code, string message = "") where T : ResponseData, new()
    {
        return GetErrorResponse<T>((Errors)code, message);
    }

    public static T GetSuccessResponse<T>() where T : ResponseData, new()
    {
        return new T
        {
            Success = true
        };
    }
}