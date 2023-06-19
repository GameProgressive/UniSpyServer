namespace UniSpy.Server.WebServer.Module.Auth
{
    public enum AuthErrorCode
    {
        Success = 0,
        ServerInitFailed,
        UserNotFound,
        InvalidPassword,
        InvalidProfile,
        UniqueNickExpired,

        DatabaseError,
        ServerError,
        FailureMax,         // must be the last failure

        // Login result (mLoginResult)
        HttpError = 100,    // ghttp reported an error, response ignored
        ParseError,         // couldn't parse http response
        InvalidCertificate, // login success but certificate was invalid!
        LoginFailed,        // failed login or other error condition
        OutOfMemory,        // could not process due to insufficient memory
        InvalidParameters,  // check the function arguments
        NoAvailabilityCheck,// No availability check was performed
        Cancelled,          // login request was cancelled
        UnknownError,       // error occured, but detailed information not available

        // response codes dealing with errors in response headers
        InvalidGameID = 200, // make sure GameID is properly set with wsSetGameCredentials
        InvalidAccessKey,       // make sure Access Key is properly set with wsSetGameCredentials

        // login results dealing with errors in response headers
        InvalidGameCredentials // check the parameters passed to wsSetGameCredentials
    }
    public class Exception : UniSpy.Exception
    {
        public AuthErrorCode ErrorCode { get; private set; }
        public Exception() : this("Unkown Error!", AuthErrorCode.UnknownError)
        {
        }

        public Exception(string message) : this(message, AuthErrorCode.UnknownError)
        {
        }
        public Exception(string message, AuthErrorCode code) : base(message)
        {
            ErrorCode = code;
        }
        public Exception(string message, AuthErrorCode code, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = code;
        }
    }
}