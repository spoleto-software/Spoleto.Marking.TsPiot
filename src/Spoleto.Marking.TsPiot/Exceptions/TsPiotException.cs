namespace Spoleto.Marking.TsPiot.Exceptions
{
    /// <summary>
    /// Исключение при общей HTTP-ошибке или исчерпании всех попыток.
    /// </summary>
    public class TsPiotException : Exception
    {
        public TsPiotException(string message) : base(message) { }

        public TsPiotException(string message, Exception inner) : base(message, inner) { }
    }
}
