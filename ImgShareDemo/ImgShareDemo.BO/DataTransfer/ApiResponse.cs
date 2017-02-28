namespace ImgShareDemo.BO.DataTransfer
{
    using System.Collections.Generic;

    public class ApiResponse<TData, TMeta>
    {
        public TData Data { get; set; }

        public List<KeyValuePair<string, string>> Messages { get; set; }

        public List<KeyValuePair<string, string>> Errors { get; set; }

        public string MetaType => this.Meta == null ? null : typeof(TMeta).Name;

        public TMeta Meta { get; set; }

        public ApiResponse()
        {
            Messages = new List<KeyValuePair<string, string>>();
            Errors = new List<KeyValuePair<string, string>>();
        }
    }

    public class ApiResponse<TData> : ApiResponse<TData, object>
    {

    }

    public class ApiResponse : ApiResponse<object, object>
    {

    }
}
