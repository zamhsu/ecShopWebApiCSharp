namespace WebApi.Infrastructures.Models.InputParamaters
{
    public class PageParameter
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
