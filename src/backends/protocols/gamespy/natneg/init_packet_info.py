from mongoengine import (
    Document,
    StringField,
    IntField,
    UUIDField,
    EnumField,
    BooleanField,
)

from servers.natneg.src.enums.general import NatClientIndex, NatPortType
import datetime


class InitPacketInfo(Document):
    server_id = UUIDField(binary=False, required=True)
    cookie = IntField(required=True)
    version = IntField(required=True)
    port_type = EnumField(NatPortType, required=True)
    client_index = EnumField(NatClientIndex, required=True)
    game_name = StringField(required=True)
    use_game_port = BooleanField(required=True)
    public_ip = StringField(required=True)
    public_port = IntField(required=True)
    private_ip = StringField(required=True)
    private_port = IntField(required=True)
    meta = {"expireAfterSeconds": int(
        datetime.timedelta(minutes=3).total_seconds())}


class NatFailInfo(Document):
    public_ip_address1 = StringField(required=True)
    public_ip_address2 = StringField(required=True)
    meta = {"expireAfterSeconds": int(
        datetime.timedelta(days=1).total_seconds())}
    """expire after 1 day"""


if __name__ == "__main__":
    InitPacketInfo.objects()
