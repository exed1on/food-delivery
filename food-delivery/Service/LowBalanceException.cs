using System;

namespace food_delivery.Service
{
    public class LowBalanceException : Exception
    {
        public string ErrorCode { get; }

        public LowBalanceException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}