namespace SkyRouteTravel.Api.Dtos;

/// <summary>
/// Generic base class for standardized API responses.
/// </summary>
/// <typeparam name="T">The type of data being returned.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// The data payload of the response. Null if the request failed or returned no data.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// A descriptive message about the response. Typically used for success messages or general information.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// A collection of error messages. Null or empty if the request succeeded.
    /// </summary>
    public string[]? Errors { get; set; }

    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
    public static ApiResponse<T> Success(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Data = data,
            Message = message,
            Errors = null
        };
    }

    /// <summary>
    /// Creates a failed response with errors.
    /// </summary>
    public static ApiResponse<T> Failure(string[] errors, string? message = null)
    {
        return new ApiResponse<T>
        {
            Data = default,
            Message = message,
            Errors = errors
        };
    }

    /// <summary>
    /// Creates a failed response with a single error.
    /// </summary>
    public static ApiResponse<T> Failure(string error, string? message = null)
    {
        return Failure([error], message);
    }
}
