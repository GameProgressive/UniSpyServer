from backends.library.abstractions.contracts import OKResponse, RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import InitPacketCaches
from backends.protocols.gamespy.natneg.data import update_init_info
from backends.protocols.gamespy.natneg.requests import ConnectRequest, InitRequest


class InitHandler(HandlerBase):
    def __init__(self, request: InitRequest) -> None:
        assert isinstance(request, InitRequest)
        super().__init__(request)

    def _data_operate(self) -> None:
        info = InitPacketCaches(**self._request.model_dump(mode="json"))
        update_init_info(info)


class ConnectHandler(HandlerBase):
    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        assert isinstance(request, ConnectRequest)
