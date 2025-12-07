namespace BookingSystem.Domain.Exceptions;

public class BookingSystemException : Exception
{
    public BookingSystemException(string message) : base(message)
    {
    }

    public BookingSystemException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}