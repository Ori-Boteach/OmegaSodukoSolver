namespace SodukoSolver
{
    [Serializable]
    public class InvalidInputLengthException : Exception
    {
        public InvalidInputLengthException() { }

        public InvalidInputLengthException(string message) : base(message) { }
    }

    [Serializable]
    public class InvalidInputCharException : Exception
    {
        public InvalidInputCharException() { }

        public InvalidInputCharException(string message) : base(message) { }
    }
}
