using System.Text.Json.Serialization;

namespace FamilyFeet.Api.Common;

/// <summary>
/// Standard envelope returned by every endpoint.
/// Result, Msg and Count come from the OUTPUT parameters @RESULT, @MSG and @COUNTER of the stored procedures.
/// </summary>
/// <typeparam name="T">Type of the payload.</typeparam>
public class Response<T>
{
    /// <summary>1 = success, 0 = validation/business error, 2 = not found, 3 = conflict, -1 = server error.</summary>
    public int Result { get; set; }

    /// <summary>Human readable message describing the outcome.</summary>
    public string Msg { get; set; } = string.Empty;

    /// <summary>Payload of the response (null when the operation fails).</summary>
    public T? Data { get; set; }

    /// <summary>Number of records affected/found. For paginated queries it is the total number of records.</summary>
    public int Count { get; set; }

    [JsonIgnore]
    public bool IsSuccess => Result == ResultCode.Success;

    public static Response<T> Ok(T? data, string msg, int count = 0) => new()
    {
        Result = ResultCode.Success,
        Msg = msg,
        Data = data,
        Count = count
    };

    public static Response<T> Fail(int result, string msg, T? data = default) => new()
    {
        Result = result,
        Msg = msg,
        Data = data
    };
}
