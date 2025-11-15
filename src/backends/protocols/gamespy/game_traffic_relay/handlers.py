from datetime import datetime, timedelta
import logging
import socket
from backends.library.abstractions.contracts import OKResponse
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import RelayServerCaches
from backends.protocols.gamespy.game_traffic_relay.requests import (
    GtrHeartBeatRequest,
)

import backends.protocols.gamespy.game_traffic_relay.data as data
from frontends.gamespy.library.exceptions.general import UniSpyException


class GtrHeartBeatHandler(HandlerBase):
    _request: GtrHeartBeatRequest
    response: OKResponse

    def _data_operate(self) -> None:
        data.check_expired_server(self._session)
        info = data.search_relay_server(
            self._request.server_id, self._request.public_ip_address, self._session
        )
        if info is None:
            info = RelayServerCaches(
                server_id=self._request.server_id,
                public_ip=self._request.public_ip_address,
                public_port=self._request.public_port,
                client_count=self._request.client_count,
            )
            data.create_relay_server(info, self._session)
        else:
            # refresh update time
            data.update_relay_server(info, self._session)
