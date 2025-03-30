from typing import TYPE_CHECKING, cast
from frontends.gamespy.library.configs import CONFIG
from datetime import datetime
from sqlalchemy import (
    Boolean,
    SmallInteger,
    Text,
    create_engine,
    Column,
    Integer,
    String,
    ForeignKey,
    DateTime,
    text,
    UUID,
    create_engine
)
from sqlalchemy.orm.session import Session
from sqlalchemy.dialects.postgresql import JSONB, INET
from sqlalchemy.ext.declarative import DeclarativeMeta
from sqlalchemy.orm import sessionmaker, declarative_base
from sqlalchemy.types import TypeDecorator
from frontends.gamespy.protocols.natneg.aggregations.enums import NatClientIndex, NatPortType
from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import FriendRequestStatus, GPStatusCode
from frontends.gamespy.protocols.query_report.v2.aggregates.enums import GameServerStatus
import sqlalchemy as sa
import enum
from sqlalchemy.orm.decl_api import DeclarativeBase


class IntEnum(TypeDecorator):
    impl = sa.Integer

    def __init__(self, enumtype, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self._enumtype = enumtype

    def process_bind_param(self, value: enum.Enum, dialect):
        return value.value

    def process_result_value(self, value, dialect):
        return self._enumtype(value)


# Base: DeclarativeMeta = declarative_base()
# if TYPE_CHECKING:
#     Base = cast(DeclarativeMeta, Base)
class Base(DeclarativeBase):
    pass


class Users(Base):
    __tablename__ = "users"

    userid: Column[int] = Column(
        Integer, primary_key=True, autoincrement=True)
    email: Column[str] = Column(String, nullable=False)
    password: Column[str] = Column(String, nullable=False)
    emailverified: Column[bool] = Column(
        Boolean, default=True, nullable=False)
    lastip: Column[str] = Column(INET)
    lastonline: Column[datetime] = Column(DateTime, default=datetime.now())
    createddate: Column[datetime] = Column(
        DateTime, default=datetime.now(), nullable=False)
    banned: Column[bool] = Column(Boolean, default=False, nullable=False)
    deleted: Column[bool] = Column(Boolean, default=False, nullable=False)


class Profiles(Base):
    __tablename__ = "profiles"

    profileid: Column[int] = Column(
        Integer, primary_key=True, autoincrement=True)
    userid: Column[int] = Column(
        Integer, ForeignKey("users.userid"), nullable=False)
    nick: Column[str] = Column(String, nullable=False)
    serverflag: Column[int] = Column(Integer, nullable=False, default=0)
    status = Column(
        IntEnum(GPStatusCode), default=GPStatusCode.OFFLINE)
    statstring: Column[str] = Column(String, default="I love UniSpy")
    extra_info: Column[JSONB] = Column(JSONB)


class SubProfiles(Base):
    __tablename__ = "subprofiles"

    subprofileid = Column(
        Integer, ForeignKey("profiles.profileid"), primary_key=True, autoincrement=True
    )
    profileid: Column[int] = Column(Integer, nullable=False)
    uniquenick: Column[str] = Column(String)
    namespaceid: Column[int] = Column(Integer, nullable=False, default=0)
    partnerid: Column[int] = Column(Integer, nullable=False, default=0)
    productid: Column[int] = Column(Integer)
    gamename: Column[str] = Column(Text)
    cdkeyenc: Column[str] = Column(String)
    firewall: Column[int] = Column(SmallInteger, default=0)
    port: Column[int] = Column(Integer, default=0)
    authtoken: Column[str] = Column(String)
    session_key: Column[str] = Column(String)


class Blocked(Base):
    __tablename__ = "blocked"

    blockid = Column(Integer, primary_key=True, autoincrement=True)
    profileid = Column(Integer, ForeignKey(
        "profiles.profileid"), nullable=False)
    namespaceid = Column(Integer, nullable=False)
    targetid = Column(Integer, nullable=False)


class Friends(Base):
    __tablename__ = "friends"

    friendid = Column(Integer, primary_key=True, autoincrement=True)
    profileid = Column(Integer, ForeignKey(
        "profiles.profileid"), nullable=False)
    targetid = Column(Integer, nullable=False)
    namespaceid = Column(Integer, nullable=False)


class FriendAddRequest(Base):
    __tablename__ = "addrequests"

    addrequestid = Column(Integer, primary_key=True, autoincrement=True)
    profileid = Column(Integer, ForeignKey(
        "profiles.profileid"), nullable=False)
    targetid = Column(Integer, ForeignKey(
        "profiles.profileid"), nullable=False)
    namespaceid = Column(Integer, nullable=False)
    reason = Column(String, nullable=False)
    status = Column(IntEnum(FriendRequestStatus), nullable=False,
                    default=FriendRequestStatus.PENDING)


class Games(Base):
    __tablename__ = "games"

    gameid = Column(Integer, primary_key=True)
    gamename = Column(String, nullable=False)
    secretkey = Column(String)
    description = Column(String(4095), nullable=False)
    disabled = Column(Boolean, nullable=False)


class GroupList(Base):
    __tablename__ = "grouplist"

    groupid = Column(Integer, primary_key=True)
    gameid = Column(Integer, ForeignKey("games.gameid"), nullable=False)
    roomname = Column(Text, nullable=False)


class Messages(Base):
    __tablename__ = "messages"

    messageid = Column(Integer, primary_key=True, autoincrement=True)
    namespaceid = Column(Integer, nullable=False)
    type = Column(Integer)
    from_user = Column(Integer, nullable=False)
    to_user = Column(Integer, nullable=False)
    date = Column(DateTime, nullable=False, default=datetime.now())
    message = Column(Text, nullable=False)


class Partner(Base):
    __tablename__ = "partner"

    partnerid = Column(Integer, primary_key=True)
    partnername = Column(String, nullable=False)


class PStorage(Base):
    __tablename__ = "pstorage"

    pstorageid = Column(Integer, primary_key=True, autoincrement=True)
    profileid = Column(Integer, ForeignKey(
        "profiles.profileid"), nullable=False)
    ptype = Column(Integer, nullable=False)
    dindex = Column(Integer, nullable=False)
    data = Column(JSONB)


class SakeStorage(Base):
    __tablename__ = "sakestorage"

    sakestorageid = Column(Integer, primary_key=True, autoincrement=True)
    tableid = Column(Integer, nullable=False)
    data: Column[JSONB] = Column(JSONB, nullable=False)


class InitPacketCaches(Base):
    __tablename__ = "init_packet_caches"

    cookie = Column(Integer, primary_key=True, nullable=False)
    server_id = Column(UUID, nullable=False)
    version = Column(Integer, nullable=False)
    port_type = Column(IntEnum(NatPortType), nullable=False)
    client_index = Column(IntEnum(NatClientIndex), nullable=False)
    game_name = Column(String, nullable=False)
    use_game_port = Column(Boolean, nullable=False)
    public_ip = Column(String, nullable=False)
    public_port = Column(Integer, nullable=False)
    private_ip = Column(String, nullable=False)
    private_port = Column(Integer, nullable=False)
    update_time = Column(DateTime, nullable=False)


class NatFailCaches(Base):
    __tablename__ = "nat_fail_cachess"
    record_id = Column(Integer, primary_key=True, autoincrement=True)
    public_ip_address1 = Column(INET, nullable=False)
    public_ip_address2 = Column(INET, nullable=False)
    update_time = Column(DateTime, nullable=False)

# class ReportPackets(Base):
#     __tablename__ = "report_packets"
#     public_ip_address1 = Column(String, nullable=False)
#     public_ip_address2 = Column(String, nullable=False)
#     update_time = Column(DateTime, nullable=False)


class RelayServerCaches(Base):
    __tablename__ = "relay_server_caches"
    server_id = Column(UUID, primary_key=True, nullable=False)
    public_ip_address = Column(String, nullable=False)
    public_port = Column(Integer, nullable=False)
    client_count = Column(Integer, nullable=False)


class ChatChannelCaches(Base):
    __tablename__ = "chat_channel_caches"
    server_id = Column(UUID, nullable=False)
    channel_name = Column(String, primary_key=True, nullable=False)
    game_name = Column(String, nullable=False)
    room_name = Column(String, nullable=False)
    topic = Column(String, nullable=True)
    password = Column(String, nullable=True)
    group_id = Column(Integer, nullable=False)
    max_num_user = Column(Integer, nullable=False)
    key_values = Column(JSONB, default={})
    invited_nicks = Column(JSONB, default=[])
    update_time = Column(DateTime, nullable=False)
    modes = Column(JSONB, default=[])
    banned_nicks = Column(JSONB, default=[])


class ChatUserCaches(Base):
    """
    each user only have a unique nick caches, but have multiple user caches
    """
    __tablename__ = "chat_user_caches"
    server_id = Column(UUID, nullable=False)
    nick_name = Column(String, primary_key=True, nullable=False)
    user_name = Column(String, nullable=True)
    game_name = Column(String, nullable=True)
    remote_ip_address = Column(INET, nullable=False)
    remote_port = Column(Integer, nullable=False)
    key_value = Column(JSONB)
    update_time = Column(DateTime, nullable=False)


class ChatChannelUserCaches(Base):
    __tablename__ = "chat_channel_user_caches"
    nick_name = Column(String, ForeignKey(
        "chat_user_caches.nick_name"), primary_key=True, nullable=False)
    user_name = Column(String, ForeignKey(
        "chat_user_caches.user_name"), primary_key=True, nullable=False)
    channel_name = Column(String, ForeignKey(
        "chat_channel_caches.channel_name"), nullable=False)
    update_time = Column(DateTime, nullable=False)
    # can we directly store the flags?
    is_voiceable = Column(Boolean, nullable=False)
    is_channel_operator = Column(Boolean, nullable=False)
    is_channel_creator = Column(Boolean, nullable=False)
    remote_ip_address = Column(INET, nullable=False)
    remote_port = Column(Integer, nullable=False)
    key_values = Column(JSONB, default={})


class GameServerCaches(Base):
    __tablename__ = "game_server_caches"
    instant_key = Column(String, primary_key=True, nullable=False)
    server_id = Column(UUID, nullable=False)
    host_ip_address = Column(INET, nullable=False)
    game_name = Column(String, nullable=False)
    query_report_port = Column(Integer, nullable=False)
    update_time = Column(DateTime, nullable=False)
    status = Column(IntEnum(GameServerStatus))
    player_data = Column(JSONB, nullable=False)
    server_data = Column(JSONB, nullable=False)
    team_data = Column(JSONB, nullable=False)
    avaliable = Column(Boolean, nullable=True)


def connect_to_db() -> Session:
    ENGINE = create_engine(CONFIG.postgresql.url)
    session = sessionmaker(bind=ENGINE)()
    # Base.metadata.create_all(ENGINE)
    with ENGINE.connect() as conn:
        conn.execute(text("SELECT 1")).first()
    return session


PG_SESSION = connect_to_db()

if __name__ == "__main__":
    PG_SESSION.query(Users.userid == 0).all()  # type:ignore
    # profile = Profiles(userid=1, nick="spyguy",
    #                    extra_info={}, status=GPStatusCode.OFFLINE)
    # PG_SESSION.add(profile)
    # PG_SESSION.commit()
    pass
