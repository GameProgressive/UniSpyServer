from mongoengine import (
    Document,
    StringField,
    IntField,
    UUIDField,
    BooleanField,
    DictField,
)


class ChannelInfo(Document):
    game_name = StringField(required=True)
    channel_name = StringField(required=True)


class ChannelUser(Document):
    server_id = UUIDField(binary=False, required=True)
    is_voiceable = BooleanField(required=True)
    is_channel_operator = BooleanField(required=True)
    is_channel_creator = BooleanField(required=True)
    remote_ip_address = StringField(required=True)
    remote_port = IntField(required=True)
    key_values = DictField(required=True)
