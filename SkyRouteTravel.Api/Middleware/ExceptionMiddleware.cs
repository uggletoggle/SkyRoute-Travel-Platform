using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FluentValidation;
using SkyRouteTravel.Api.Dtos;
using SkyRouteTravel.Application.Exceptions;

namespace SkyRouteTravel.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during the request pipeline.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errorMessages = validationException.Errors
                .Select(e => e.ErrorMessage)
                .ToArray();

            var response = ApiResponse<object>.Failure(
                errorMessages,
                "One or more validation errors occurred.");

            await context.Response.WriteAsJsonAsync(response);
        }
        else if (exception is ArgumentException || exception is BadHttpRequestException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = ApiResponse<object>.Failure(
                exception.Message,
                "Bad Request");

            await context.Response.WriteAsJsonAsync(response);
        }
        else if (exception is NotFoundException notFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            var response = ApiResponse<object>.Failure(
                notFoundException.Message,
                "Resource Not Found");

            await context.Response.WriteAsJsonAsync(response);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = ApiResponse<object>.Failure(
                "Please contact support if this issue persists.",
                "An unexpected error occurred.");

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
