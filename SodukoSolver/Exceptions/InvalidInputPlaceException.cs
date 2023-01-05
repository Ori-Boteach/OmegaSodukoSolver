[Serializable]
public class InvalidInputPlaceException : Exception // a custom exception for initialy illegal inputted soduko 
{
    public InvalidInputPlaceException() { }

    public InvalidInputPlaceException(string message) : base(message) { }
}