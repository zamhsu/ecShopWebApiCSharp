namespace WebApi.Infrastructures.Models.Paramaters
{
    public class BaseRequest<T>
    {
        /// <summary>
        /// 資料
        /// </summary>
        /// <value></value>
        public T Data { get; set; }

        /// <summary>
        /// 時區(格式為00:00:00)
        /// </summary>
        /// <value></value>
        public string TimeZone { get; set; } = "00:00:00";

        private TimeSpan _ts = new TimeSpan(0, 0, 0);
        /// <summary>
        /// 已轉換好的使用者時區
        /// </summary>
        /// <value></value>
        internal TimeSpan UserTimeZone
        {
            get => _ts;
            set
            {
                _ = TimeSpan.TryParse(TimeZone, out _ts);
            }
        }
    }
}