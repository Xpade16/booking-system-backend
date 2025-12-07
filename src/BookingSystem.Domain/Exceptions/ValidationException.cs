namespace BookingSystem.Domain.Exceptions;

public class ValidationException : BookingSystemException
{
    public ValidationException(string message) : base(message)
    {
    }
}