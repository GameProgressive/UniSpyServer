from typing import cast
from frontends.gamespy.library.configs import CONFIG
from frontends.tests.gamespy.library.mock_objects import ConnectionMock, LogMock, RequestHandlerMock, create_mock_url
from frontends.gamespy.protocols.presence_search_player.applications.client import Client
from frontends.gamespy.protocols.presence_search_player.applications.handlers import CheckHandler, SearchHandler
from frontends.gamespy.protocols.presence_search_player.contracts.results import CheckResult, SearchResult


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
    create_mock_url(config, CheckHandler, CheckResult.model_validate(
        {"profile_id": 0}).model_dump())

    create_mock_url(config, SearchHandler, SearchResult.model_validate({"data": [{"profile_id": 0, "nick": "spyguy", "uniquenick": "spyguy",
                                                                                    "email": "spyguy@unispy.org", "firstname": "spy", "lastname": "guy", "namespace_id": 0}]}).model_dump())

    return cast(Client, conn._client)
