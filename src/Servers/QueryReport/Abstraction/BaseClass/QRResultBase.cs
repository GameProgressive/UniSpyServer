using QueryReport.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class QRResultBase : UniSpyResultBase
    {
        public new QRErrorCode ErrorCode 
        { 
            get { return (QRErrorCode)base.ErrorCode; }
            set { base.ErrorCode = value; } 
        }
        public QRResultBase()
        {
            ErrorCode = QRErrorCode.NoError;
        }
    }
}