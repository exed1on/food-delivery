using System;

namespace food_delivery.Service
{
    public class LowBalanceException : Exception
    {
        public LowBalanceException(string message) : base(message)
        {
        }
    }
}