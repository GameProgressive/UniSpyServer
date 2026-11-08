from pydantic import UUID4

import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.query_report.aggregates.enums import GameServerStatus

import frontends.gamespy.protocols.query_report.v1.contracts.requests as v1fnt
import frontends.gamespy.protocols.query_report.v2.contracts.requests as qr_v2_fnt


class RequestBase(lib.RequestBase):
    pass


# region v2


class AvaliableRequest(RequestBase):
    data: qr_v2_fnt.AvaliableRequest


class ChallengeRequest(RequestBase):
    data: qr_v2_fnt.ChallengeRequest


class ClientMessageAckRequest(RequestBase):
    data: qr_v2_fnt.ClientMessageAckRequest


class ClientMessageRequest(RequestBase):
    data: qr_v2_fnt.ClientMessageRequest


class HeartBeatRequest(RequestBase):
    data: qr_v2_fnt.HeartbeatRequest


class EchoRequest(RequestBase):
    data: qr_v2_fnt.EchoRequest


class KeepAliveRequest(RequestBase):
    data: qr_v2_fnt.KeepAliveRequest


# region v1
class HeartbeatRequestV1(lib.RequestBase):
    data: v1fnt.LegacyHeartbeatRequest
