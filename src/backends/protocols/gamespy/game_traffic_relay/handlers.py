from datetime import datetime
import logging
from backends.library.abstractions.contracts import OKResponse
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import RelayServerCaches
from backends.protocols.gamespy.game_traffic_relay.requests import (
    GTRHeartBeat,
)

import backends.protocols.gamespy.game_traffic_relay.data as data


class GTRHeartBeatHandler(HandlerBase):
    _request: GTRHeartBeat

    def __init__(self, request: GTRHeartBeat) -> None:
        assert isinstance(request, GTRHeartBeat)
        self._request = request
        self.logger = logging.getLogger("backend")
        self._result = None
        self._response = OKResponse()

    def _data_operate(self) -> None:
        info = data.search_relay_server(
            self._request.server_id, self._request.public_ip_address, self._session
        )
        if info is None:
            info = RelayServerCaches(
                server_id=self._request.server_id,
                public_ip_address=self._request.public_ip_address,
                public_port=self._request.public_port,
                client_count=self._request.client_count,
                update_time=datetime.now(),
            )
            data.add_relay_server(info, self._session)
        else:
            # refresh update time
            data.update_relay_server(info, self._session)
