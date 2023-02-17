using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.WebServer.Module.Auth.Exception
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
    public class AuthException : UniSpyException
    {
        public AuthErrorCode ErrorCode { get; private set; }
        public AuthException() : this("Unkown Error!", AuthErrorCode.UnknownError)
        {
        }

        public AuthException(string message) : this(message, AuthErrorCode.UnknownError)
        {
        }
        public AuthException(string message, AuthErrorCode code)
        {
            ErrorCode = code;
        }
        public AuthException(string message, AuthErrorCode code, System.Exception innerException) : base(message, innerException)
        {
            ErrorCode = code;
        }
    }
}