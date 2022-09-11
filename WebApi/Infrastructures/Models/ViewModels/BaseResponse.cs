namespace WebApi.Infrastructures.Models.ViewModels
{
    public class BaseResponse<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <value></value>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        /// <value></value>
        public string Message { get; set; }

        /// <summary>
        /// 回應資料
        /// </summary>
        /// <value></value>
        public T Data { get; set; }
    }
}