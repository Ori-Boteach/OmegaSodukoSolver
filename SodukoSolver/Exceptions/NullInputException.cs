public class NullInputException : Exception // a custom exception for null input -> mostly used for keyboard interrupt handling
{
    public NullInputException() { }

    public NullInputException(string message) : base(message) { }
}