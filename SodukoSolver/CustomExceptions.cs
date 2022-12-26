namespace SodukoSolver
{
    [Serializable]
    public class InvalidInputLengthException : Exception // a custom exception for illegal input length
    {
        public InvalidInputLengthException() { }

        public InvalidInputLengthException(string message) : base(message) { }
    }

    [Serializable]
    public class InvalidInputCharException : Exception // a custom exception for illegal input char or chars
    {
        public InvalidInputCharException() { }

        public InvalidInputCharException(string message) : base(message) { }
    }
    
    [Serializable]
    public class InvalidInputPlaceException : Exception // a custom exception for initialy illegal inputted soduko 
    {
        public InvalidInputPlaceException() { }

        public InvalidInputPlaceException(string message) : base(message) { }
    }
}
