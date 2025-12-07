namespace BookingSystem.Domain.Exceptions;

public class NotFoundException : BookingSystemException
{
    public NotFoundException(string message) : base(message)
    {
    }
}