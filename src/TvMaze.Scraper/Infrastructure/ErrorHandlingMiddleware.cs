using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TvShows.Scraper.Api.Infrastructure
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			var httpMethod = context.Request?.Method.ToUpperInvariant();
			var requestPath = context.Request?.Path;

			try
			{
				await _next(context);
			}

			catch (OperationCanceledException)
			{
				context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex,
					"Request: {HttpMethod} {RequestPath}; status: {HttpStatusCode};",
					httpMethod, requestPath, 500);

				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}
		}
	}
}
