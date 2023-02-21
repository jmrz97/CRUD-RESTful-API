namespace FastDeliveryApi.Exceptions;

public class NoContentException : ApplicationException
{
    public NoContentException(string message) : base(message)
    {
        
    }
}