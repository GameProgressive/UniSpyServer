from backends.library.abstractions.handler_base import HandlerBase
import backends.protocols.gamespy.chat.data as data
from backends.protocols.gamespy.chat.requests import JoinRequest


class JoinHandler(HandlerBase):
    _request: JoinRequest

    async def _data_fetch(self) -> None:
        data.is_channel_exist(self._request.channel_name,
                              self._request.game_name)
