using CryptoWalletsServices.Utils;
using CryptoWalletsServices.Utils.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace CryptoWalletsServices.WebCryptoSim.Infrastructure
{
	/// <summary>
	/// Фильтр исключений web api.
	/// </summary>
	public class BaseExceptionHandleAttribure : ExceptionFilterAttribute
	{
		private Logger _logger;
		private static readonly object _loggerLock = new object();

		public BaseExceptionHandleAttribure()
		{
			this._logger = LogManager.GetCurrentClassLogger();
		}

		/// <summary>
		/// Обработка исключений.
		/// </summary>
		/// <param name="context">Контекст исключения.</param>
		public override void OnException(HttpActionExecutedContext context)
		{
			Guid errorId = Guid.NewGuid();

			var responseCode = HttpStatusCode.InternalServerError;
			var errorText = context.Exception.Message;
			LogLevel logLevel = null;

			if (context.Exception is ApplicationExceptionBase)
			{
				responseCode = HttpStatusCode.BadRequest;
				errorText = context.Exception.Message;
				logLevel = LogLevel.Warn;
			}
			else
			{
				responseCode = HttpStatusCode.InternalServerError;
				errorText = $"Ошибка {errorId}: {context.Exception.Message}";
				logLevel = LogLevel.Error;
			}

			this._WriteLog(errorId, context.Exception, logLevel);

			context.Response = context.Request.CreateErrorResponse(responseCode, errorText);
		}

		/// <summary>
		/// Записать в лог исключение.
		/// </summary>
		/// <param name="errorId">Идентификатор исключения.</param>
		/// <param name="exception">Исключение.</param>
		/// <param name="level">Уровень исключения. Если он пустой, лог не пишется.</param>
		private void _WriteLog(Guid errorId, Exception exception, LogLevel level = null)
		{
			Argument.Require(errorId != Guid.Empty, "Не задан идентификатор исключения.");
			Argument.Require(exception != null, "Исключение пустое.");
			if (level == null)
			{
				return;
			}

			string errorText = $"Ошибка {errorId}. {exception.ToString()}";
			lock (_loggerLock)
			{
				this._logger.Log(level, errorText);
			}
		}
	}
}