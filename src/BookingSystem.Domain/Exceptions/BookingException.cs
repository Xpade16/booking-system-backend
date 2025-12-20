namespace BookingSystem.Domain.Exceptions;

public class BookingException : BookingSystemException
{
    public BookingException(string message) : base(message)
    {
    }
}