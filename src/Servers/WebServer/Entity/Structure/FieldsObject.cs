namespace WebServer.Entity.Structure
{
    public class FieldsObject
    {
        public string FieldName { get; private set; }
        public string FiledType { get; private set; }

        public FieldsObject(string fieldName, string filedType)
        {
            this.FieldName = fieldName;
            this.FiledType = filedType;
        }
    }
}
