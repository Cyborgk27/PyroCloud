namespace PyroCloud.Core.Domain.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public int StatusCode { get; set; } = 400;
        public UserFriendlyException(string message) : base(message) { }
        public UserFriendlyException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
