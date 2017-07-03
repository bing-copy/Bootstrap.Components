using Bootstrap.Components.Models.Constants;

namespace Bootstrap.Components.Models.ResponseModels
{
    public class DataResponse<T> : BaseResponse
    {
        public T Data { get; set; }

        public DataResponse()
        {
        }

        public DataResponse(T data)
        {
            Data = data;
        }

        public DataResponse(int code) : base(code)
        {
        }

        public DataResponse(int code, string message) : base(code, message)
        {
        }
    }
}
