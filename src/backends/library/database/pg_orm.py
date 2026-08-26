import enum
from datetime import datetime, timezone

from sqlalchemy import (
    UUID,
    Boolean,
    DateTime,
    ForeignKey,
    Integer,
    SmallInteger,
    String,
    Text,
    create_engine,
    text,
)
from sqlalchemy.dialects.postgresql import INET, JSONB
from sqlalchemy.orm import Mapped, mapped_column
from sqlalchemy.orm.decl_api import DeclarativeBase
from sqlalchemy.orm.session import Session
from sqlalchemy.types import TypeDecorator

from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
    NatPortMappingScheme,
    NatPortType,
    NatType,
)
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    GPStatusCode,
)
from frontends.gamespy.protocols.query_report.aggregates.enums import GameServerStatus


class IntEnum(TypeDecorator):
    impl = Integer

    def __init__(self, enumtype, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self._enumtype = enumtype
        self.cache_ok = True

    def process_bind_param(self, value: enum.Enum, dialect):
        return value.value

    def process_result_value(self, value, dialect):
        return self._enumtype(value)


class Base(DeclarativeBase):
    pass


class Users(Base):
    __tablename__ = "users"

    userid: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    email: Mapped[str] = mapped_column(String, nullable=False)
    password: Mapped[str] = mapped_column(String, nullable=False)
    emailverified: Mapped[bool] = mapped_column(Boolean, default=True, nullable=False)
    lastip: Mapped[str | None] = mapped_column(INET)
    lastonline: Mapped[datetime | None] = mapped_column(
        DateTime, default=datetime.now()
    )
    createddate: Mapped[datetime] = mapped_column(
        DateTime, default=datetime.now(), nullable=False
    )
    banned: Mapped[bool] = mapped_column(Boolean, default=False, nullable=False)
    deleted: Mapped[bool] = mapped_column(Boolean, default=False, nullable=False)


class Profiles(Base):
    __tablename__ = "profiles"

    profileid: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    userid: Mapped[int] = mapped_column(ForeignKey("users.userid"), nullable=False)
    nick: Mapped[str] = mapped_column(nullable=False)
    serverflag: Mapped[int] = mapped_column(nullable=False, default=0)
    status: Mapped[GPStatusCode] = mapped_column(
        IntEnum(GPStatusCode), default=GPStatusCode.OFFLINE
    )
    statstring: Mapped[str] = mapped_column(default="I love UniSpy")
    extra_info: Mapped[dict] = mapped_column(JSONB, default={})


class SubProfiles(Base):
    __tablename__ = "subprofiles"
    subprofileid: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    profileid: Mapped[int] = mapped_column(
        ForeignKey("profiles.profileid"), nullable=False
    )
    uniquenick: Mapped[str | None] = mapped_column()
    namespaceid: Mapped[int] = mapped_column(nullable=False, default=0)
    partnerid: Mapped[int] = mapped_column(nullable=False, default=0)
    productid: Mapped[int | None] = mapped_column()
    gamename: Mapped[str | None] = mapped_column()
    cdkeyenc: Mapped[str | None] = mapped_column()
    firewall: Mapped[int] = mapped_column(default=0)
    port: Mapped[int] = mapped_column(default=0)
    authtoken: Mapped[str | None] = mapped_column()
    session_key: Mapped[str | None] = mapped_column()


class Blacklist(Base):
    __tablename__ = "black_list"
    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    profileid: Mapped[int] = mapped_column(
        ForeignKey("profiles.profileid"), nullable=False
    )
    targetid: Mapped[int] = mapped_column(nullable=False)
    namespaceid: Mapped[int] = mapped_column(
        ForeignKey("subprofiles.namespaceid"), nullable=False
    )
    update_time: Mapped[datetime | None] = mapped_column(
        default=datetime.now()
    )


class FriendRequest(Base):
    __tablename__ = "friend_request"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    sender_profileid: Mapped[int] = mapped_column(
        ForeignKey("profiles.profileid"), nullable=False
    )
    receiver_profileid: Mapped[int] = mapped_column(
        ForeignKey("profiles.profileid"), nullable=False
    )
    namespaceid: Mapped[int] = mapped_column(
        ForeignKey("subprofiles.namespaceid"), nullable=False
    )
    reason: Mapped[str] = mapped_column(nullable=False)
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


class Friendlist(Base):
    __tablename__ = "friend_list"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    profileid: Mapped[int] = mapped_column(
        ForeignKey("profiles.profileid"), nullable=False
    )
    targetid: Mapped[int] = mapped_column(
        ForeignKey("profiles.profileid"), nullable=False
    )
    namespaceid: Mapped[int] = mapped_column(
        ForeignKey("subprofiles.namespaceid"), nullable=False
    )
    update_time: Mapped[datetime | None] = mapped_column(
        default=datetime.now()
    )


class Games(Base):
    __tablename__ = "games"

    gameid: Mapped[int] = mapped_column(primary_key=True)
    gamename: Mapped[str] = mapped_column(nullable=False)
    secretkey: Mapped[str] = mapped_column(nullable=False)
    description: Mapped[str] = mapped_column(String(4095), nullable=False)
    disabled: Mapped[bool] = mapped_column(default=False)


class GroupList(Base):
    __tablename__ = "grouplist"

    groupid: Mapped[int] = mapped_column(primary_key=True)
    gameid: Mapped[int] = mapped_column(ForeignKey("games.gameid"), nullable=False)
    roomname: Mapped[str] = mapped_column(Text, nullable=False)


class Messages(Base):
    __tablename__ = "messages"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    namespaceid: Mapped[int] = mapped_column(nullable=False)

    type: Mapped[int | None] = mapped_column()

    from_user: Mapped[int] = mapped_column(nullable=False)
    to_user: Mapped[int] = mapped_column(nullable=False)
    date: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )
    message: Mapped[str] = mapped_column(Text, nullable=False)


class Partner(Base):
    __tablename__ = "partner"

    partnerid: Mapped[int] = mapped_column(primary_key=True)
    partnername: Mapped[str] = mapped_column(nullable=False)


class PStorage(Base):
    __tablename__ = "pstorage"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    profileid: Mapped[int] = mapped_column(
        ForeignKey("profiles.profileid"), nullable=False
    )
    ptype: Mapped[int] = mapped_column(nullable=False)
    dindex: Mapped[int] = mapped_column(nullable=False)
    data: Mapped[str] = mapped_column(nullable=False)
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


class GameStatusSnapShot(Base):
    __tablename__ = "game_status_snapshot"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)

    connection_id: Mapped[int | None] = mapped_column()
    session_key: Mapped[str | None] = mapped_column()
    game_name: Mapped[str | None] = mapped_column()

    game_data: Mapped[dict | None] = mapped_column(JSONB)
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


class SakeStorage(Base):
    __tablename__ = "sakestorage"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    tableid: Mapped[str] = mapped_column(nullable=False)
    record: Mapped[dict] = mapped_column(JSONB, nullable=False)


class InitPacketCaches(Base):
    __tablename__ = "init_packet_caches"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    cookie: Mapped[int] = mapped_column(SmallInteger, nullable=False)
    server_id: Mapped[str] = mapped_column(UUID, nullable=False)
    version: Mapped[int] = mapped_column(nullable=False)
    port_type: Mapped[NatPortType] = mapped_column(IntEnum(NatPortType), nullable=False)
    client_index: Mapped[NatClientIndex] = mapped_column(
        IntEnum(NatClientIndex), nullable=False
    )
    game_name: Mapped[str | None] = mapped_column()
    use_game_port: Mapped[bool] = mapped_column(nullable=False)
    public_ip: Mapped[str] = mapped_column(nullable=False)
    public_port: Mapped[int] = mapped_column(nullable=False)
    private_ip: Mapped[str] = mapped_column(nullable=False)
    private_port: Mapped[int] = mapped_column(nullable=False)
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


class NatResultCaches(Base):
    __tablename__ = "nat_result_caches"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    cookie: Mapped[int] = mapped_column(SmallInteger, nullable=False)
    public_ip: Mapped[str] = mapped_column(INET, nullable=False)
    private_ip: Mapped[str] = mapped_column(INET, nullable=False)
    is_success: Mapped[bool] = mapped_column(nullable=False)
    port_mapping_scheme: Mapped[NatPortMappingScheme] = mapped_column(
        IntEnum(NatPortMappingScheme), nullable=False
    )
    nat_type: Mapped[NatType] = mapped_column(IntEnum(NatType), nullable=False)
    port_type: Mapped[NatPortType] = mapped_column(IntEnum(NatPortType), nullable=False)
    client_index: Mapped[NatClientIndex] = mapped_column(
        IntEnum(NatClientIndex), nullable=False
    )
    game_name: Mapped[str | None] = mapped_column()
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


class RelayServerCaches(Base):
    __tablename__ = "relay_server_caches"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    server_id: Mapped[str] = mapped_column(UUID, primary_key=True, nullable=False)
    public_ip: Mapped[str] = mapped_column(nullable=False)
    public_port: Mapped[int] = mapped_column(nullable=False)
    client_count: Mapped[int] = mapped_column(nullable=False)
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


class ChatChannelCaches(Base):
    __tablename__ = "chat_channel_caches"

    channel_name: Mapped[str] = mapped_column(primary_key=True, nullable=False)
    server_id: Mapped[str] = mapped_column(UUID, nullable=False)
    creator: Mapped[str] = mapped_column(nullable=False)
    game_name: Mapped[str] = mapped_column(nullable=False)
    room_name: Mapped[str] = mapped_column(nullable=False)
    topic: Mapped[str | None] = mapped_column()
    password: Mapped[str | None] = mapped_column()
    group_id: Mapped[int] = mapped_column(nullable=False)
    max_num_user: Mapped[int] = mapped_column(nullable=False)
    key_values: Mapped[dict] = mapped_column(JSONB, default={})
    invited_nicks: Mapped[list[str]] = mapped_column(JSONB, default=[])
    modes: Mapped[list[str]] = mapped_column(JSONB, default=[])
    banned_nicks: Mapped[list[str]] = mapped_column(JSONB, default=[])
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


class ChatUserCaches(Base):
    """
    each user only have a unique nick caches, but have multiple user caches
    """

    __tablename__ = "chat_user_caches"

    nick_name: Mapped[str | None] = mapped_column(primary_key=True, nullable=False)
    server_id: Mapped[str] = mapped_column(UUID, nullable=False)
    user_name: Mapped[str | None] = mapped_column()
    game_name: Mapped[str | None] = mapped_column()
    remote_ip: Mapped[str] = mapped_column(INET, nullable=False)
    remote_port: Mapped[int] = mapped_column(nullable=False)
    websocket_address: Mapped[str] = mapped_column(nullable=False)
    key_value: Mapped[dict] = mapped_column(JSONB, default={})
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


class ChatChannelUserCaches(Base):
    __tablename__ = "chat_channel_user_caches"
    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    nick_name: Mapped[str] = mapped_column(
        ForeignKey("chat_user_caches.nick_name"), nullable=False
    )
    user_name: Mapped[str] = mapped_column(
        ForeignKey("chat_user_caches.user_name"), nullable=False
    )
    channel_name: Mapped[str] = mapped_column(
        ForeignKey("chat_channel_caches.channel_name"), nullable=False
    )
    server_id: Mapped[str] = mapped_column(UUID, nullable=False)
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )
    is_voiceable: Mapped[bool] = mapped_column(nullable=False)
    is_channel_operator: Mapped[bool] = mapped_column(nullable=False)
    is_channel_creator: Mapped[bool] = mapped_column(nullable=False)
    remote_ip: Mapped[str] = mapped_column(INET, nullable=False)
    remote_port: Mapped[int] = mapped_column(nullable=False)
    key_values: Mapped[dict] = mapped_column(JSONB, default={})


class GameServerCaches(Base):
    __tablename__ = "game_server_caches"

    id: Mapped[int] = mapped_column(primary_key=True)
    instant_key: Mapped[str | None] = mapped_column()
    server_id: Mapped[str] = mapped_column(UUID, nullable=False)
    host_ip_address: Mapped[str] = mapped_column(INET, nullable=False)
    game_name: Mapped[str] = mapped_column(nullable=False)
    query_report_port: Mapped[int] = mapped_column(nullable=False)
    status: Mapped[GameServerStatus] = mapped_column(IntEnum(GameServerStatus))
    data: Mapped[dict] = mapped_column(JSONB, nullable=False, default={})
    avaliable: Mapped[bool | None] = mapped_column()
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


class FrontendInfo(Base):
    __tablename__ = "frontend_info"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    server_id: Mapped[str] = mapped_column(UUID, nullable=False)
    server_name: Mapped[str] = mapped_column(nullable=False)
    external_ip: Mapped[str] = mapped_column(nullable=False)
    listening_ip: Mapped[str] = mapped_column(INET, nullable=False)
    listening_port: Mapped[int] = mapped_column(nullable=False)
    update_time: Mapped[datetime] = mapped_column(
        nullable=False, default=datetime.now()
    )


ENGINE = create_engine(CONFIG.postgresql.url)
with ENGINE.connect() as conn:
    conn.execute(text("SELECT 1")).first()

if __name__ == "__main__":
    with Session(ENGINE) as session:
        session.query(Users.userid == 0).all()
    # profile = Profiles(userid=1, nick="spyguy",
    #                    extra_info={}, status=GPStatusCode.OFFLINE)
    # PG_SESSION.add(profile)
    # PG_SESSION.commit()
