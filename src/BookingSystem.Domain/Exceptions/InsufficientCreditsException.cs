namespace BookingSystem.Domain.Exceptions;

public class InsufficientCreditsException : BookingSystemException
{
    public InsufficientCreditsException(string message) : base(message)
    {
    }
}