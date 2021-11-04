using Microsoft.Extensions.Logging;

namespace WebApi.Base
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;
        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        /// <summary>
        /// 記錄資訊層級訊息
        /// </summary>
        /// <param name="message">文字訊息</param>
        /// <param name="args">參數</param>
        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        /// <summary>
        /// 記錄警告層級訊息
        /// </summary>
        /// <param name="message">文字訊息</param>
        /// <param name="args">參數</param>
        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }
    }
}