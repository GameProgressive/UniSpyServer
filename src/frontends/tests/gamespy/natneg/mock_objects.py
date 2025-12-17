from frontends.gamespy.protocols.natneg.contracts.results import AddressCheckResult, ConnectResult, InitResult
from frontends.tests.gamespy.library.mock_objects import (
    ConnectionMock,
    LogMock,
    RequestHandlerMock,
    create_mock_url,
)
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.natneg.applications.client import Client
from typing import cast

from frontends.gamespy.protocols.natneg.applications.handlers import (
    AddressCheckHandler,
    ConnectHandler,
    ErtAckHandler,
    InitHandler,
    NatifyHandler,
)


class ClientMock(Client):
    pass


def create_client() -> Client:
    CONFIG.unittest.is_raise_except = True
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["NatNegotiation"],
        t_client=ClientMock,
        logger=logger,
    )

    config = CONFIG.servers["NatNegotiation"]
    create_mock_url(config, InitHandler, InitResult.model_validate(
        {"version": 3, "cookie": 777, "public_ip_addr": "127.0.0.1", "public_port": 1234, "use_game_port": False, "port_type": 1, "client_index": 0, "use_game_port": 0}).model_dump(mode="json"))
    create_mock_url(config, AddressCheckHandler, AddressCheckResult.model_validate(
        {"version": 3, "cookie": 0, "public_ip_addr": "127.0.0.1", "public_port": 1234, "use_game_port": False, "port_type": 0, "client_index": 0, "use_game_port": 0}).model_dump(mode="json"))
    create_mock_url(config, NatifyHandler, {"message": "ok"})
    create_mock_url(config, ErtAckHandler, {"message": "ok"})
    create_mock_url(config, ConnectHandler, ConnectResult.model_validate(
        {"version": 3, "cookie": 0, "is_both_client_ready": True, "ip": "192.168.0.1", "port": 7890, "status": 0}).model_dump(mode="json"))
    return cast(Client, conn._client)
