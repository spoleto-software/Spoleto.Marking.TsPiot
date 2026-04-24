namespace Spoleto.Marking.TsPiot.Exceptions
{
    /// <summary>
    /// Выбрасывается политикой retry при получении HTTP 400 Bad Request.
    /// Повторные попытки для этого исключения не выполняются.
    /// </summary>
    public sealed class TsPiotNoRetryException : TsPiotException
    {
        public TsPiotNoRetryException(string message) : base(message) { }

        public TsPiotNoRetryException(string message, Exception innerException) : base(message, innerException) { }
    }
}
