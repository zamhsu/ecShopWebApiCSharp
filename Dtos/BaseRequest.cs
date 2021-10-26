namespace WebApi.Dtos
{
    public class BaseRequest<T>
    {
        public T Data { get; set; }
    }
}