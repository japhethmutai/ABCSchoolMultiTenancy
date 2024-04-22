
namespace Application.Models.Wrapper
{
    public class ResponseWrapper : IResponseWrapper
    {
        public List<string> Messages { get; set; } = [];
        public bool IsSuccessful { get; set; }

        public static IResponseWrapper Fail()
        {
            return new ResponseWrapper { IsSuccessful = false };
        }

        public static IResponseWrapper Fail(string message)
        {
            return new ResponseWrapper { IsSuccessful = false, Messages = new List<string> { message } };
        }

        public static IResponseWrapper Fail(List<string> messages)
        {
            return new ResponseWrapper { IsSuccessful = false, Messages = messages };
        }

        public static Task<IResponseWrapper> FailAsync()
        {
            return Task.FromResult(Fail());
        }
        
        public static Task<IResponseWrapper> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static Task<IResponseWrapper> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }
        public static IResponseWrapper Success()
        {
            return new ResponseWrapper { IsSuccessful = true };
        }

        public static IResponseWrapper Success(string message)
        {
            return new ResponseWrapper { IsSuccessful = true, Messages = new List<string> { message } };
        }

        public static Task<IResponseWrapper> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResponseWrapper> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

    }

    public class ResponseWrapper<T> : ResponseWrapper, IResponseWrapper<T>
    {
        public ResponseWrapper()
        {
        }

        public T Data { get; set; }

        public new static ResponseWrapper<T> Fail()
        {
            return new ResponseWrapper<T> { IsSuccessful = false };
        }

        public new static ResponseWrapper<T> Fail(string message)
        {
            return new ResponseWrapper<T> { IsSuccessful = false, Messages = new List<string> { message } };
        }

        public new static ResponseWrapper<T> Fail(List<string> messages)
        {
            return new ResponseWrapper<T> { IsSuccessful = false, Messages = messages };
        }

        public new static Task<ResponseWrapper<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public new static Task<ResponseWrapper<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public new static Task<ResponseWrapper<T>> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public new static ResponseWrapper<T> Success()
        {
            return new ResponseWrapper<T> { IsSuccessful = true };
        }

        public new static ResponseWrapper<T> Success(string message)
        {
            return new ResponseWrapper<T> { IsSuccessful = true, Messages = new List<string> { message } };
        }

        public static ResponseWrapper<T> Success(T data)
        {
            return new ResponseWrapper<T> { IsSuccessful = true, Data = data };
        }

        public static ResponseWrapper<T> Success(T data, string message)
        {
            return new ResponseWrapper<T> { IsSuccessful = true, Data = data, Messages = new List<string> { message } };
        }

        public static ResponseWrapper<T> Success(T data, List<string> messages)
        {
            return new ResponseWrapper<T> { IsSuccessful = true, Data = data, Messages = messages };
        }

        public new static Task<ResponseWrapper<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public new static Task<ResponseWrapper<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<ResponseWrapper<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<ResponseWrapper<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
    }
}
