from pydantic import UUID4

import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.query_report.aggregates.enums import GameServerStatus

import frontends.gamespy.protocols.query_report.v1.contracts.requests as v1fnt
import frontends.gamespy.protocols.query_report.v2.contracts.requests as v2fnt


class RequestBase(lib.RequestBase):
    pass


# region v2


class AvaliableRequest(RequestBase):
    data: v2fnt.AvaliableRequest


class ChallengeRequest(RequestBase):
    data: v2fnt.ChallengeRequest


class ClientMessageAckRequest(RequestBase):
    data: v2fnt.ClientMessageAckRequest


class ClientMessageRequest(RequestBase):
    data: v2fnt.ClientMessageRequest


class HeartBeatRequest(RequestBase):
    data: v2fnt.HeartbeatRequest


class EchoRequest(RequestBase):
    data: v2fnt.EchoRequest


class KeepAliveRequest(RequestBase):
    data: v2fnt.KeepAliveRequest


# region v1
class HeartbeatRequestV1(lib.RequestBase):
    data: v1fnt.LegacyHeartbeatRequest
