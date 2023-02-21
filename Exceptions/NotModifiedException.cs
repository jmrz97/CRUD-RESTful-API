namespace FastDeliveryApi.Exceptions;

public class NotModifiedException : ApplicationException
{
    public NotModifiedException(string message) : base(message)
    {
        
    }
}