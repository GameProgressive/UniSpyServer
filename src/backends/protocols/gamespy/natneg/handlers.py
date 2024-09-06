from typing import Any, Coroutine
from backends.library.abstractions.handler_base import HandlerBase
from backends.protocols.gamespy.natneg.data import update_init_info
from backends.protocols.gamespy.natneg.init_packet_info import InitPacketInfo
from backends.protocols.gamespy.natneg.requests import InitRequest


class InitHandler(HandlerBase):
    def __init__(self, request: InitRequest) -> None:
        super().__init__(request)
        assert isinstance(request, InitRequest)

    async def data_fetch(self) -> None:
        info = InitPacketInfo(**self._request.model_dump_json())
        await update_init_info(info)

    async def result_construct(self) -> None:
        self.response = {"message", "ok"}
