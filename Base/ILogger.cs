namespace WebApi.Base
{
    public interface IAppLogger<T>
    {
        /// <summary>
        /// 記錄資訊層級訊息
        /// </summary>
        /// <param name="message">文字訊息</param>
        /// <param name="args">參數</param>
        void LogInformation(string message, params object[] args);

        /// <summary>
        /// 記錄警告層級訊息
        /// </summary>
        /// <param name="message">文字訊息</param>
        /// <param name="args">參數</param>
        void LogWarning(string message, params object[] args);
    }
}