from datetime import datetime, timezone
from backends.library.abstractions.contracts import RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import InitPacketCaches
from backends.protocols.gamespy.natneg.data import update_init_info
from backends.protocols.gamespy.natneg.requests import ConnectRequest, InitRequest


class InitHandler(HandlerBase):
    _request: InitRequest

    def __init__(self, request: InitRequest) -> None:
        assert isinstance(request, InitRequest)
        super().__init__(request)

    def _data_operate(self) -> None:
        info = InitPacketCaches(
            cookie=self._request.cookie,
            server_id=self._request.server_id,
            version=self._request.version,
            port_type=self._request.port_type,
            client_index=self._request.client_index,
            game_name=self._request.game_name,
            use_game_port=self._request.use_game_port,
            public_ip=self._request.client_ip,
            public_port=self._request.client_port,
            private_ip=self._request.private_ip,
            private_port=self._request.private_port,
            update_time=datetime.now(timezone.utc),
        )
        update_init_info(info)


class ConnectHandler(HandlerBase):
    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        assert isinstance(request, ConnectRequest)

    def _data_operate(self) -> None:
        raise NotImplementedError()
