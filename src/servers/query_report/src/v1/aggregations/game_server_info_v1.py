from mongoengine import (
    Document,
    StringField,
    IntField,
    UUIDField,
    BooleanField,
    DictField,
)


class GameServerInfoV1(Document):
    server_id = UUIDField(binary=False, required=True)
    host_ip_address = StringField(required=True)
    host_port = IntField(required=True)
    game_name = StringField(required=True)
    is_validated = BooleanField(required=True)
    server_data = DictField(required=True)
    meta = {"expireAfterSeconds": 30}
