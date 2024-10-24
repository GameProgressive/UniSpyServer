from typing import Optional
from library.src.configs import CONFIG
from sqlalchemy import Enum, create_engine
from sqlalchemy.orm.session import Session
from datetime import datetime
from sqlalchemy import (
    Boolean,
    Double,
    SmallInteger,
    Text,
    create_engine,
    Column,
    Integer,
    String,
    ForeignKey,
    DateTime,
    text,
    UUID
)
from sqlalchemy.dialects.postgresql import JSONB, INET
from sqlalchemy.ext.declarative import DeclarativeMeta
from sqlalchemy.orm import sessionmaker, declarative_base

from servers.natneg.src.enums.general import NatClientIndex, NatPortType
from servers.query_report.src.v2.aggregates.enums import GameServerStatus

Base: DeclarativeMeta = declarative_base()


class Users(Base):
    __tablename__ = "users"

    userid = Column(Integer, primary_key=True, autoincrement=True)
    email = Column(String, nullable=False)
    password = Column(String, nullable=False)
    emailverified = Column(Boolean, default=True, nullable=False)
    lastip = Column(INET)
    lastonline = Column(DateTime, default=datetime.now())
    createddate = Column(DateTime, default=datetime.now(), nullable=False)
    banned = Column(Boolean, default=False, nullable=False)
    deleted = Column(Boolean, default=False, nullable=False)


class Profiles(Base):
    __tablename__ = "profiles"

    profileid = Column(Integer, primary_key=True, autoincrement=True)
    userid = Column(Integer, ForeignKey("users.userid"), nullable=False)
    nick = Column(String, nullable=False)
    serverflag = Column(Integer, nullable=False, default=0)
    status = Column(SmallInteger, default=0)
    statstring = Column(String, default="I love UniSpy")
    location = Column(String)
    firstname = Column(String)
    lastname = Column(String)
    publicmask = Column(Integer, default=0)
    latitude = Column(Double, default=0)
    longitude = Column(Double, default=0)
    aim = Column(String, default="")
    picture = Column(Integer, default=0)
    occupationid = Column(Integer, default=0)
    incomeid = Column(Integer, default=0)
    industryid = Column(Integer, default=0)
    marriedid = Column(Integer, default=0)
    childcount = Column(Integer, default=0)
    interests1 = Column(Integer, default=0)
    ownership1 = Column(Integer, default=0)
    connectiontype = Column(Integer, default=0)
    sex = Column(SmallInteger, default=0)
    zipcode = Column(String, default="00000")
    countrycode = Column(String, default="1")
    homepage = Column(String, default="unispy.org")
    birthday = Column(Integer, default=0)
    birthmonth = Column(Integer, default=0)
    birthyear = Column(Integer, default=0)
    icquin = Column(Integer, default=0)
    quietflags = Column(SmallInteger, nullable=False, default=0)
    streetaddr = Column(Text)
    streeaddr = Column(Text)
    city = Column(Text)
    cpubrandid = Column(Integer, default=0)
    cpuspeed = Column(Integer, default=0)
    memory = Column(SmallInteger, default=0)
    videocard1string = Column(Text)
    videocard1ram = Column(SmallInteger, default=0)
    videocard2string = Column(Text)
    videocard2ram = Column(SmallInteger, default=0)
    subscription = Column(Integer, default=0)
    adminrights = Column(Integer, default=0)


class SubProfiles(Base):
    __tablename__ = "subprofiles"

    subprofileid = Column(
        Integer, ForeignKey("profiles.profileid"), primary_key=True, autoincrement=True
    )
    profileid = Column(Integer, nullable=False)
    uniquenick = Column(String)
    namespaceid = Column(Integer, nullable=False, default=0)
    partnerid = Column(Integer, nullable=False, default=0)
    productid = Column(Integer)
    gamename = Column(Text)
    cdkeyenc = Column(String)
    firewall = Column(SmallInteger, default=0)
    port = Column(Integer, default=0)
    authtoken = Column(String)


class AddRequest(Base):
    __tablename__ = "addrequests"

    addrequestid = Column(Integer, primary_key=True, autoincrement=True)
    profileid = Column(Integer, ForeignKey(
        "profiles.profileid"), nullable=False)
    namespaceid = Column(Integer, nullable=False)
    targetid = Column(Integer, ForeignKey(
        "profiles.profileid"), nullable=False)
    reason = Column(String, nullable=False)
    syncrequested = Column(String, nullable=False)


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
    namespaceid = Column(Integer, nullable=False)
    targetid = Column(Integer, nullable=False)


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
    namespaceid = Column(Integer)
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
    tableid = Column(String, nullable=False)


class InitPacketCaches(Base):
    __tablename__ = "init_packet_caches"

    cookie = Column(Integer, primary_key=True, nullable=False)
    server_id = Column(UUID, nullable=False)
    version = Column(Integer, nullable=False)
    port_type = Column(Enum(NatPortType), nullable=False)
    client_index = Column(Enum(NatClientIndex), nullable=False)
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


class ChatChannelCaches(Base):
    __tablename__ = "chat_channel_caches"
    channel_name = Column(String, primary_key=True, nullable=False)
    server_id = Column(UUID, nullable=False)
    game_name = Column(String, nullable=False)
    room_name = Column(String, nullable=False)
    topic = Column(String, nullable=False)
    password = Column(String, nullable=True)
    group_id = Column(Integer, nullable=False)
    max_num_user = Column(Integer, nullable=False)
    key_values = Column(JSONB)
    update_time = Column(DateTime, nullable=False)


class ChatUserCaches(Base):
    __tablename__ = "chat_user_caches"
    nick_name = Column(String, primary_key=True, nullable=False)
    channel_name = Column(String, ForeignKey(
        "chat_channel_caches.channel_name"), nullable=False)
    server_id = Column(UUID, nullable=False)
    user_name = Column(String, nullable=False)
    update_time = Column(DateTime, nullable=False)
    is_voiceable = Column(Boolean, nullable=False)
    is_channel_operator = Column(Boolean, nullable=False)
    is_channel_creator = Column(Boolean, nullable=False)
    remote_ip_address = Column(INET, nullable=False)
    remote_port = Column(Integer, nullable=False)
    key_values = Column(JSONB)


class GameServerCaches(Base):
    __tablename__ = "game_server_caches"
    instant_key = Column(Integer, primary_key=True, nullable=False)
    server_id = Column(UUID, nullable=False)
    host_ip_address = Column(INET, nullable=False)
    game_name = Column(String, nullable=False)
    query_report_port = Column(Integer, nullable=False)
    update_time = Column(DateTime, nullable=False)
    status = Column(Enum(GameServerStatus))
    player_data = Column(JSONB, nullable=False)
    server_data = Column(JSONB, nullable=False)
    team_data = Column(JSONB, nullable=False)


def connect_to_db() -> Session:
    ENGINE = create_engine(CONFIG.postgresql.url)
    session = sessionmaker(bind=ENGINE)()
    Base.metadata.create_all(ENGINE)
    with ENGINE.connect() as conn:
        conn.execute(text("SELECT 1"))
    return session


PG_SESSION = connect_to_db()

if __name__ == "__main__":
    session = connect_to_db()
    session.query(Users.userid == 0)
    pass
