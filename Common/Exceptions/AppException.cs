namespace Common.Exceptions
{
    public class AppException : Exception
    {
        public ErrorDetail Detail { get; }

        public AppException(ErrorDetail detail)
            : base(detail.Message)
        {
            Detail = detail;
        }

        public AppException(ErrorDetail detail, string customMessage)
            : base(customMessage)
        {
            Detail = new ErrorDetail(detail.HttpStatusCode, customMessage);
        }
    }
}
