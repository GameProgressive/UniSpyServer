from enum import Enum
from typing import Any
import uuid
from pydantic import UUID4, BaseModel
import backends.library.abstractions.contracts as lib
import frontends.gamespy.protocols.chat.contracts.requests as fnt
import frontends.gamespy.protocols.chat.applications.messages as fnt_msg


class RequestBase(lib.RequestBase):
    info: fnt.ClientInfo
    data: fnt.RequestBase


# region General
class CdkeyRequest(RequestBase):
    data: fnt.CdkeyRequest


class CryptRequest(RequestBase):
    data: fnt.CryptRequest


class GetUdpRelayRequest(RequestBase):
    data: fnt.GetUdpRelayRequest


class InviteRequest(RequestBase):
    data: fnt.InviteRequest


class ListLimitRequest(RequestBase):
    data: fnt.ListLimitRequest


class ListRequest(RequestBase):
    data: fnt.ListRequest


class LoginPreAuthRequest(RequestBase):
    data: fnt.LoginPreAuthRequest


class LoginRequest(RequestBase):
    data: fnt.LoginRequest


class NickRequest(RequestBase):
    data: fnt.NickRequest


class PingRequest(RequestBase):
    data: fnt.PingRequest


class PongRequest(RequestBase):
    data: fnt.PongRequest


class QuitRequest(RequestBase):
    data: fnt.QuitRequest


class RegisterNickRequest(RequestBase):
    data: fnt.RegisterNickRequest


class SetKeyRequest(RequestBase):
    data: fnt.SetKeyRequest


class UserIPRequest(RequestBase):
    data: fnt.UserIPRequest


class UserRequest(RequestBase):
    data: fnt.UserRequest


class WhoIsRequest(RequestBase):
    data: fnt.WhoIsRequest


class WhoRequest(RequestBase):
    data: fnt.WhoRequest


class GetKeyRequest(RequestBase):
    data: fnt.GetKeyRequest


class ChannelRequestBase(RequestBase):
    data: fnt.ChannelRequestBase

# region Channel


class GetChannelKeyRequest(ChannelRequestBase):
    data: fnt.GetChannelKeyRequest


class GetCKeyRequest(ChannelRequestBase):
    data: fnt.GetCKeyRequest


class JoinRequest(ChannelRequestBase):
    data: fnt.JoinRequest


class KickRequest(ChannelRequestBase):
    data: fnt.KickRequest


class ModeRequest(ChannelRequestBase):
    data: fnt.ModeRequest


class NamesRequest(ChannelRequestBase):
    data: fnt.NamesRequest


class PartRequest(ChannelRequestBase):
    data: fnt.PartRequest


class SetChannelKeyRequest(ChannelRequestBase):
    data: fnt.SetChannelKeyRequest


class SetCKeyRequest(ChannelRequestBase):
    data: fnt.SetCKeyRequest


class SetGroupRequest(ChannelRequestBase):
    data: fnt.SetGroupRequest


class TopicRequest(ChannelRequestBase):
    data: fnt.TopicRequest


# region Message
class MessageRequestBase(ChannelRequestBase):
    data: fnt.ChannelMessageRequestBase


class AtmRequest(MessageRequestBase):
    data: fnt.AtmRequest


class NoticeRequest(MessageRequestBase):
    data: fnt.NoticeRequest


class PrivateRequest(MessageRequestBase):
    data: fnt.PrivateRequest


class UtmRequest(MessageRequestBase):
    data: fnt.UtmRequest


class PublishMessageType(Enum):
    CHANNEL = 0
    USER = 1


class MessageBroadcastRequest(RequestBase):
    data: fnt_msg.MessageBroadcastRequest


class WebsocketIpBindRequest(BaseModel):
    server_id: UUID4
    client_ip_address: str
    client_ws_address: str
