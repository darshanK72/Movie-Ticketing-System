﻿using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Exceptions;
using System.Text.Json;

namespace MovieTicketingSystem.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFound)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";
            var response = new { notFound.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));

        }
        catch (ForbidException)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            var response = new { Message = "Access forbidden" };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (DbUpdateException dbUpdateEx)
        {
            context.Response.ContentType = "application/json";
            if (dbUpdateEx.InnerException?.Message.Contains("REFERENCE constraint") == true)
            {
                context.Response.StatusCode = 409;
                var response = new { Message = "Cannot delete this record because it has related records in other tables." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else
            {
                context.Response.StatusCode = 500;
                var response = new { Message = "A database error occurred while processing your request." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var response = new { Message = "Something went wrong" };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
