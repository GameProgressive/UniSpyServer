from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
    NatPortMappingScheme,
    NatPortType,
    NatType,
    PreInitState,
    RequestType,
)
from typing import Union

import backends.library.abstractions.contracts as lib
from frontends.gamespy.protocols.natneg.contracts.results import ConnectResult

# region Requests
class RequestBase(lib.RequestBase):
    raw_request: str
    version: int
    cookie: int
    port_type: NatPortType
    command_name: RequestType


class CommonRequestBase(RequestBase):
    client_index: NatClientIndex
    use_game_port: bool


class AddressCheckRequest(CommonRequestBase):
    pass


class ConnectAckRequest(RequestBase):
    client_index: NatClientIndex


class ConnectRequest(CommonRequestBase):
    """
    Server will send this request to client to let them connect to each other
    """


class ErtAckRequest(CommonRequestBase):
    pass


class InitRequest(CommonRequestBase):
    game_name: str | None = None
    private_ip: str
    private_port: int


class NatifyRequest(CommonRequestBase):
    pass


class PreInitRequest(RequestBase):
    state: Union[PreInitState, int]
    target_cookie: list


class ReportRequest(CommonRequestBase):
    is_nat_success: bool
    game_name: str
    nat_type: NatType
    mapping_scheme: NatPortMappingScheme

