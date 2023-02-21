namespace FastDeliveryApi.Exceptions;

public class UniqueEmailException : ApplicationException
{
    public UniqueEmailException(string email) : base ($"{email} ya existe")
    {
        
    }
}