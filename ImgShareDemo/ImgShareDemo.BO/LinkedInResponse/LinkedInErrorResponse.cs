namespace ImgShareDemo.BO.LinkedInResponse
{
    public class LinkedInErrorResponse
    {
        public int errorCode { get; set; }
        public string message { get; set; }
        public string requestId { get; set; }
        public int status { get; set; }
        public long timestamp { get; set; }
    }
}
