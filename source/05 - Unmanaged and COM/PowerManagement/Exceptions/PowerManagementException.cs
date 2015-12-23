using System;

namespace PowerManagement.Exceptions
{
    public class PowerManagementException: Exception
    {
        public uint ErrorCode { get; set; }

        public PowerManagementException(string message, uint errorCode): base(message)
        {
            ErrorCode = errorCode;
        }

    }
}
