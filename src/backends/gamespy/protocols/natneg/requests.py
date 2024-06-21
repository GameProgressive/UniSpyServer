from dataclasses import dataclass
from servers.natneg.enums.general import (
    NatClientIndex,
    NatPortMappingScheme,
    NatPortType,
    NatType,
    PreInitState,
    RequestType,
)
from typing import Union

import backends.gamespy.library.abstractions.request_base as lib

@dataclass
class RequestBase(lib.RequestBase):
    raw_request: list
    version: int
    cookie: list
    port_type: NatPortType
    command_name: Union[RequestType, int]

@dataclass
class CommonRequestBase(RequestBase):
    client_index: Union[NatClientIndex, int]
    use_game_port: bool

@dataclass
class AddressCheckRequest(CommonRequestBase):
    pass

@dataclass
class ConnectAckRequest(RequestBase):
    client_index: Union[NatClientIndex, int]

@dataclass
class ConnectRequest(RequestBase):
    """
    Server will send this request to client to let them connect to each other
    """

    client_index: Union[NatClientIndex, int]

@dataclass
class ErtAckRequest(CommonRequestBase):
    pass

@dataclass
class InitRequest(CommonRequestBase):
    game_name: str = None
    private_ip_address: str = None

@dataclass
class NatifyRequest(CommonRequestBase):
    pass

@dataclass
class PreInitRequest(RequestBase):
    state: Union[PreInitState, int]
    target_cookie: list

@dataclass
class ReportRequest(CommonRequestBase):
    is_nat_success: bool
    game_name: str
    nat_type: NatType
    mapping_scheme: NatPortMappingScheme
