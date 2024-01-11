namespace Infrastructure;

public class RepositoryException : Exception
{
    public string ErrorCode { get; private set; }
    public string CustomMessage { get; private set; }

    public RepositoryException()
    {
    }

    public RepositoryException(string message)
        : base(message)
    {
    }

    public RepositoryException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public RepositoryException(string errorCode, string customMessage)
        : this($"Error Code: {errorCode}, Message: {customMessage}")
    {
        ErrorCode = errorCode;
        CustomMessage = customMessage;
    }

    public RepositoryException(string errorCode, string customMessage, Exception inner)
        : this($"Error Code: {errorCode}, Message: {customMessage}", inner)
    {
        ErrorCode = errorCode;
        CustomMessage = customMessage;
    }
}