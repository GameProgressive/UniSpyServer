using System.Collections.Generic;
namespace UniSpy.Server.WebServer.Aggregate
{

    public record RecordFieldObject : FieldObject
    {
        public string FieldValue { get; private set; }
        public RecordFieldObject(string fieldValue, string fieldName, string filedType) : base(fieldName, filedType)
        {
            FieldValue = fieldValue;
        }
    }

    public record FieldObject
    {

        public string FieldName { get; private set; }
        public string FiledType { get; private set; }

        public FieldObject(string fieldName, string filedType)
        {
            this.FieldName = fieldName;
            this.FiledType = filedType;
        }
    }
}
