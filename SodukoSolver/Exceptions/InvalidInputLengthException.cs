public class InvalidInputLengthException : Exception // a custom exception for illegal input length
{
    public InvalidInputLengthException() { }

    public InvalidInputLengthException(string message) : base(message) { }
}