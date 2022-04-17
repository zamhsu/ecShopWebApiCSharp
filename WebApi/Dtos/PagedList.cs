namespace WebApi.Dtos
{
    public class PagedList<T>
    {
        public PagedList()
        { }

        /// <summary>
        /// 分頁資料
        /// </summary>
        /// <param name="source">要分頁的資料</param>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="currentPage">目前頁數</param>
        public PagedList(IQueryable<T> source, int pageSize, int currentPage)
        {
            int count = source.Count();
            int totalPage = count > 0 ? (int)Math.Ceiling(count / (double)pageSize) : 0;

            this.Pagination = new Pagination()
            {
                TotalPage = totalPage,
                CurrentPage = currentPage,
                HasNext = currentPage < totalPage,
                HasPrevious = currentPage > 1
            };

            this.PagedData = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 分頁資料
        /// </summary>
        /// <param name="source">要分頁的資料</param>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="currentPage">目前頁數</param>
        public PagedList(List<T> source, int pageSize, int currentPage)
        {
            int count = source.Count();
            int totalPage = count > 0 ? (int)Math.Ceiling(count / (double)pageSize) : 0;

            this.Pagination = new Pagination()
            {
                TotalPage = totalPage,
                CurrentPage = currentPage,
                HasNext = currentPage < totalPage,
                HasPrevious = currentPage > 1
            };

            this.PagedData = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 分頁後的資料
        /// </summary>
        /// <value></value>
        public List<T> PagedData { get; set; } = null!;

        /// <summary>
        /// 分頁資訊
        /// </summary>
        /// <value></value>
        public Pagination Pagination { get; set; } = null!;
    }
}