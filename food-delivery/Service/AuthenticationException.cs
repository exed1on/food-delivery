namespace food_delivery.Service
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message)
        {
        }
    }
}