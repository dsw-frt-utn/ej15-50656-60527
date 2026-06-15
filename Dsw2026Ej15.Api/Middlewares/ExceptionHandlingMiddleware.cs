using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continúa con el flujo normal de la petición
                await _next(context);
            }
            catch (ValidationException ex)
            {
                // Si es nuestra excepción de validación, retornamos 400 Bad Request
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                // Para cualquier otro error, retornamos 500 Problem
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var result = JsonSerializer.Serialize(new { error = "Ha ocurrido un problema interno en el servidor." });
                await context.Response.WriteAsync(result);
            }
        }
    }
}