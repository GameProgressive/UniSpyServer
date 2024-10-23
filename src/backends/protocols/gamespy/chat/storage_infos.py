from mongoengine import (
    Document,
    StringField,
    IntField,
    UUIDField,
    BooleanField,
    DictField,
    DateTimeField
)


class ChannelInfo(Document):
    game_name = StringField(required=True)
    channel_name = StringField(required=True)
    key_values = DictField()
    max_num_user = IntField(max_value=200)
    room_name = StringField()
    topic = StringField()
    password = StringField()
    group_id = IntField()
    create_time = DateTimeField(required=True)
    previously_joined_channel = StringField()


class ChannelUser(Document):
    """
    We can use one of the info below to search the channel info
    """
    server_id = UUIDField(binary=False, required=True)
    channel_name = StringField(required=True)
    user_name = StringField(required=True)
    nick_name = StringField(required=True)
    is_voiceable = BooleanField(required=True)
    is_channel_operator = BooleanField(required=True)
    is_channel_creator = BooleanField(required=True)
    remote_ip_address = StringField(required=True)
    remote_port = IntField(required=True)
    key_values = DictField(required=True)
