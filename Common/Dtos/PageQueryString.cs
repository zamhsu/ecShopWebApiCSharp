namespace Common.Dtos
{
    public class PageQueryString
    {
        /// <summary>
        /// 資料筆數
        /// </summary>
        /// <value></value>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 頁數
        /// </summary>
        /// <value></value>
        public int Page { get; set; } = 1;
    }
}