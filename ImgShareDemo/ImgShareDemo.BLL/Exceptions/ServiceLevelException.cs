namespace ImgShareDemo.BLL.Exceptions
{
    using System;

    /// <summary>
    /// General exception for creating exception messages that are acceptable to bubble up to 
    /// the user.
    /// </summary>
    public class ServiceLevelException : Exception
    {
        /// <summary>
        /// API exception handler may use this to pass a value
        /// to use for a key along with the message since it uses
        /// key value pairs in the error payload.
        /// </summary>
        public string ErrorKey { get; set; }
        public ServiceLevelException(string message, string errorKey = null) : base(message)
        {
            ErrorKey = errorKey;
        }

        public ServiceLevelException(string message, Exception inner, string errorKey = null) : base(message, inner)
        {
            ErrorKey = errorKey;
        }
    }
}
