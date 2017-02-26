namespace ImgShareDemo.BO.DataTransfer
{
    using System.Net.Http;

    public class ApiDataResponse<T>
    {
        public HttpResponseMessage Response { get; set; }

        public T Data { get; set; }
    }
}
