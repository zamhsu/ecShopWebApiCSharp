using WebApi.Dtos;

namespace WebApi.Extensions
{
    public static class ListExtension
    {
        /// <summary>
        /// 將資料分頁
        /// </summary>
        /// <param name="source">要分頁的資料</param>
        /// <param name="pageSize">一頁資料的筆數</param>
        /// <param name="currentPage">目前頁數</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this List<T> source, int pageSize, int currentPage)
        {
            return new PagedList<T>(source, pageSize, currentPage);
        }
    }
}