namespace Application.Models.Wrapper
{
    public interface IResponseWrapper
    {
        List<string> Messages { get; set; }
        public bool IsSuccessful { get; set; }
    }

    public interface IResponseWrapper<out T> : IResponseWrapper
    {
        T Data { get; }
    }
}
