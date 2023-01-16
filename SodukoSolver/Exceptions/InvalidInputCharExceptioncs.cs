[Serializable]
public class InvalidInputCharException : Exception // a custom exception for illegal input char or chars
{
    public InvalidInputCharException() { }

    public InvalidInputCharException(string message) : base(message) { }
}