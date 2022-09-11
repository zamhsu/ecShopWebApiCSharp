namespace Repository.Interfaces
{
    public interface ISqlCommand
    {
        /// <summary>
        /// 查詢
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL語句</param>
        /// <param name="param">參數</param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string sql, object param = null);

        /// <summary>
        /// 查詢
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL語句</param>
        /// <param name="param">參數</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null);

        /// <summary>
        /// 查詢一筆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL語句</param>
        /// <param name="param">參數</param>
        /// <returns></returns>
        T QueryFirstOrDefault<T>(string sql, object param = null);

        /// <summary>
        /// 查詢一筆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL語句</param>
        /// <param name="param">參數</param>
        /// <returns></returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null);

        /// <summary>
        /// 執行非查詢動作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL語句</param>
        /// <param name="param">參數</param>
        /// <returns></returns>
        int Execute(string sql, object param = null);

        /// <summary>
        /// 執行非查詢動作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL語句</param>
        /// <param name="param">參數</param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string sql, object param = null);

        /// <summary>
        /// 查詢所傳回之結果集中第一個資料列的第一個資料行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL語句</param>
        /// <param name="param">參數</param>
        /// <returns></returns>
        T ExecuteScalar<T>(string sql, object param = null);

        /// <summary>
        /// 查詢所傳回之結果集中第一個資料列的第一個資料行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL語句</param>
        /// <param name="param">參數</param>
        /// <returns></returns>
        Task<T> ExecuteScalarAsync<T>(string sql, object param = null);
    }
}