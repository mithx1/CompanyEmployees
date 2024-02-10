namespace Entities.Exceptions.BaseExceptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message)
            : base(message) { }
    }
}
