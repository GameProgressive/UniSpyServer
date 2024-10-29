from typing import cast
from library.src.configs import CONFIG
from library.tests.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from servers.presence_search_player.src.applications.client import Client
from servers.presence_search_player.src.applications.handlers import CheckHandler, SearchHandler


class ClientMock(Client):
    pass


def create_client() -> Client:
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["PresenceSearchPlayer"], t_client=ClientMock,
        logger=logger)
    config = CONFIG.servers["PresenceSearchPlayer"]
    create_mock_url(config, CheckHandler, {"profile_id": 0})
    create_mock_url(config, SearchHandler, {"result": [{"profile_id": 0, "nick": "spyguy", "uniquenick": "spyguy",
                                                        "email": "spyguy@unispy.org", "firstname": "spy", "lastname": "guy", "namespace_id": 0}]})

    return cast(Client, conn._client)
