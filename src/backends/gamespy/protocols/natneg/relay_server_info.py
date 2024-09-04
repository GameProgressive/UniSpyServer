from mongoengine import Document, StringField, IntField, UUIDField


class RelayServerInfo(Document):
    server_id = UUIDField(binary=False, required=True)
    public_ip_address = StringField(required=True)
    public_port = IntField(required=True)
    client_count = IntField(required=True)
    # report how many client are connect to this relay server


