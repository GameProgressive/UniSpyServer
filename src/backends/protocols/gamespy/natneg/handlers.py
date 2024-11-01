from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import InitPacketCaches
from backends.protocols.gamespy.natneg.data import update_init_info
from backends.protocols.gamespy.natneg.requests import InitRequest


class InitHandler(HandlerBase):
    def __init__(self, request: InitRequest) -> None:
        super().__init__(request)
        assert isinstance(request, InitRequest)

    async def _data_fetch(self) -> None:
        info = InitPacketCaches(**self._request.model_dump())
        update_init_info(info)

    async def _result_construct(self) -> None:
        self.response = {"message": "ok"}
