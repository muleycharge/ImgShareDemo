using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShareDemo.BO.DataTransfer
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
