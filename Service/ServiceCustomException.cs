namespace Service;

public class ServiceException : Exception
{
    public string ErrorCode { get; private set; }
    public string CustomMessage { get; private set; }

    public ServiceException()
    {
    }

    public ServiceException(string message)
        : base(message)
    {
    }

    public ServiceException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public ServiceException(string errorCode, string customMessage)
        : this($"Error Code: {errorCode}, Message: {customMessage}")
    {
        ErrorCode = errorCode;
        CustomMessage = customMessage;
    }

    public ServiceException(string errorCode, string customMessage, Exception inner)
        : this($"Error Code: {errorCode}, Message: {customMessage}", inner)
    {
        ErrorCode = errorCode;
        CustomMessage = customMessage;
    }
}