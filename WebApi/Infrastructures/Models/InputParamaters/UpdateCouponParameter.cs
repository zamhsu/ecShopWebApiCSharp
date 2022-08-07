namespace WebApi.Infrastructures.Models.InputParamaters
{
    public class UpdateCouponParameter
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Used { get; set; }
        public int StatusId { get; set; }
    }
}