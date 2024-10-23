from servers.natneg.src.enums.general import (
    NatClientIndex,
    NatPortMappingScheme,
    NatPortType,
    NatType,
    PreInitState,
    RequestType,
)
from typing import Optional, Union

import backends.library.abstractions.contracts as lib


class RequestBase(lib.RequestBase):
    raw_request: list
    version: int
    cookie: int
    port_type: NatPortType
    command_name: Union[RequestType, int]


class CommonRequestBase(RequestBase):
    client_index: Union[NatClientIndex, int]
    use_game_port: bool


class AddressCheckRequest(CommonRequestBase):
    pass


class ConnectAckRequest(RequestBase):
    client_index: Union[NatClientIndex, int]


class ConnectRequest(RequestBase):
    """
    Server will send this request to client to let them connect to each other
    """

    client_index: Union[NatClientIndex, int]


class ErtAckRequest(CommonRequestBase):
    pass


class InitRequest(CommonRequestBase):
    game_name: Optional[str]
    private_ip_address: Optional[str]


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
