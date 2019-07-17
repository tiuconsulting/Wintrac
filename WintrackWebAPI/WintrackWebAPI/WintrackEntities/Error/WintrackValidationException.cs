using System;

namespace WintrackEntities.Error
{
    public class WintrackValidationException : Exception
    {
        /// <summary>
        /// Localized string for error message.
        /// </summary>
        public string ErrorCode { get; set; }
        public WintrackValidationException(string errorCode, string errorMessage) : base(errorMessage)
        {
            ErrorCode = errorCode;
        }
    }
}
