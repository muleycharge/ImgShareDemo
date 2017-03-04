namespace ImgShareDemo.BO.DataTransfer
{
    using System.Net.Http;

    public class IncomingApiDataResponse<T>
    {
        public HttpResponseMessage Response { get; set; }

        public T Data { get; set; }
    }
}
