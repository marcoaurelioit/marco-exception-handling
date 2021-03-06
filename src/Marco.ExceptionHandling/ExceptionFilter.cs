﻿using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Marco.ExceptionHandling
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ILogger _logger;

        public ExceptionFilter(IExceptionHandler exceptionHandler, ILoggerFactory loggerFactory)
        {
            _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
            _logger = loggerFactory?.CreateLogger<ExceptionFilter>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public void OnException(ExceptionContext context)
        {
            try
            {
                _exceptionHandler.HandleAsync(context.Exception, context.HttpContext).Wait();
                context.ExceptionHandled = true;
            }
            catch (Exception e)
            {
                _logger.LogCritical(0, e, $"BUG na biblioteca que implementa '{typeof(IExceptionHandler).FullName}'. Verifique a exceção no log para obter mais detalhes.");
            }
        }
    }
}