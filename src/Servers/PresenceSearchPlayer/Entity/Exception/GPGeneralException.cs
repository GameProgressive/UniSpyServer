using System;
using PresenceSearchPlayer.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public class GPGeneralException : GPExceptionBase
    {
        public new GPErrorCode ErrorCode => (GPErrorCode)base.ErrorCode;
        public override string ErrorResponse => $@"\error\\err\{(uint)ErrorCode}\fatal\\errmsg\{this.Message}\final\";
        public GPGeneralException() : this("There was an unknown error occurs.", GPErrorCode.General)
        {
        }

        public GPGeneralException(string message, GPErrorCode errorCode) : base(message, errorCode)
        {
        }

        public GPGeneralException(string message, GPErrorCode errorCode, Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}