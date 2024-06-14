from datetime import datetime, timezone
import uuid
from mongoengine import (
    Document,
    StringField,
    IntField,
    UUIDField,
    DateTimeField,
    EnumField,
    ListField,
    DictField,
)

from library.database.mongodb_orm import connect_to_db, get_ttl_param
from servers.query_report.v2.enums.general import GameServerStatus


class GameServerInfoV2(Document):
    created = DateTimeField(default=datetime.now(timezone.utc))
    server_id = UUIDField(binary=False, required=True)
    host_ip_address = StringField(required=True)
    instant_key = IntField(required=True)
    last_package_received_time = DateTimeField(required=True)
    server_status = EnumField(GameServerStatus, required=True)
    server_data = DictField(required=True)
    query_report_port = IntField()
    player_data = ListField(DictField())
    team_data = ListField(DictField())
    meta = get_ttl_param(30)
    """expire after 30 seconds"""


if __name__ == "__main__":
    connect_to_db()
    result = GameServerInfoV2.objects().first()
    if result is None:
        dd = GameServerInfoV2(
            server_id=uuid.UUID("212b7218-1758-11ef-94cc-70a8d36da155"),
            host_ip_address="192.168.0.1",
            instant_key=123,
            last_package_received_time=datetime.now(timezone.utc),
            server_status=GameServerStatus.NORMAL,
            server_data={"hello": "hi"},
        )
        dd.save()
    pass
