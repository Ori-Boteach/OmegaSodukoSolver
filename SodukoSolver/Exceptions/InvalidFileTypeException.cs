[Serializable]
public class InvalidFileTypeException : Exception // a custom exception for an illegal file type 
{
    public InvalidFileTypeException() { }

    public InvalidFileTypeException(string message) : base(message) { }
}