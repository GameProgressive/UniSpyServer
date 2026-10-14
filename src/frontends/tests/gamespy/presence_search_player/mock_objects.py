from typing import cast

from frontends.gamespy.library.abstractions.connections import ServerParams
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.protocols.presence_search_player.applications.client import (
    Client,
)
from frontends.gamespy.protocols.presence_search_player.applications.handlers import (
    CheckHandler,
    SearchHandler,
)
from frontends.gamespy.protocols.presence_search_player.contracts.results import (
    CheckResult,
    SearchResult,
)
from frontends.tests.gamespy.library.mock_objects import (
    ConnectionMock,
    LogMock,
    RequestHandlerMock,
    create_mock_url,
)


class ClientMock(Client):
    pass


def create_client() -> Client:
    CONFIG.unittest.is_raise_except = True
    handler = RequestHandlerMock()
    logger = LogMock()
    param = ServerParams(
        config=CONFIG.servers["PresenceSearchPlayer"],
        client_cls=ClientMock,
        logger=logger,
    )
    conn = ConnectionMock(handler=handler, param=param)
    config = CONFIG.servers["PresenceSearchPlayer"]
    create_mock_url(
        config, CheckHandler, CheckResult.model_validate({"profile_id": 0}).model_dump()
    )

    create_mock_url(
        config,
        SearchHandler,
        SearchResult.model_validate(
            {
                "data": [
                    {
                        "profile_id": 0,
                        "nick": "spyguy",
                        "uniquenick": "spyguy",
                        "email": "spyguy@gamespy.com",
                        "firstname": "spy",
                        "lastname": "guy",
                        "namespace_id": 1,
                    }
                ]
            }
        ).model_dump(),
    )

    return cast(Client, conn._client)
