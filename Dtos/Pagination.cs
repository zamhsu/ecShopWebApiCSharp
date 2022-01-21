namespace WebApi.Dtos
{
    public class Pagination
    {
        /// <summary>
        /// 總頁數
        /// </summary>
        /// <value></value>
        public int TotalPage { get; set; }

        /// <summary>
        /// 目前頁數
        /// </summary>
        /// <value></value>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 是否有上一頁
        /// </summary>
        /// <value></value>
        public bool HasPrevious { get; set; }

        /// <summary>
        /// 是否有下一頁
        /// </summary>
        /// <value></value>
        public bool HasNext { get; set; }
    }
}