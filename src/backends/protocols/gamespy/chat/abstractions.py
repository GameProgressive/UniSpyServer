
from backends.library.abstractions.contracts import ErrorResponse, RequestBase
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import ChatChannelCaches, ChatChannelUserCaches, ChatUserCaches
from backends.protocols.gamespy.chat.requests import ChannelRequestBase
from library.exceptions.general import UniSpyException
from protocols.chat.aggregates.exceptions import NoSuchChannelException, NoSuchNickException
import backends.protocols.gamespy.chat.data as data


class ChannelHandlerBase(HandlerBase):
    _request: ChannelRequestBase
    _user: ChatUserCaches | None
    _channel: ChatChannelCaches | None
    _channel_user: ChatChannelUserCaches | None
    _is_broadcast: bool

    def __init__(self, request: RequestBase) -> None:
        super().__init__(request)
        self._user = None
        self._channel = None
        self._channel_user = None
        self._is_broadcast = False

    def _get_user(self):
        self._user = data.get_user_cache_by_ip_port(
            self._request.client_ip, self._request.client_port)

    def _get_channel(self):
        self._channel = data.get_channel_by_name(
            self._request.channel_name)

    def _get_channel_user(self):
        self._channel_user = data.get_channel_user_cache_by_name_and_ip_port(
            self._request.channel_name,
            self._request.client_ip,
            self._request.client_port)

    def _check_user(self):
        if self._user is None:
            raise NoSuchNickException(
                f"Can not find user with ip address: {self._request.client_ip}:{self._request.client_port}")

    def _check_channel(self):
        if self._channel is None:
            raise NoSuchChannelException(
                f"Can not find channel with name: {self._request.channel_name}")

    def _check_channel_user(self):
        if self._channel_user is None:
            raise NoSuchNickException(
                f"Can not find channel user with channel name: {self._request.channel_name}, ip address: {self._request.client_ip}:{self._request.client_port}")

    async def _request_check(self) -> None:
        self._get_user()
        self._check_user()

        self._get_channel()
        self._check_channel()

        self._get_channel_user()
        self._check_channel_user()

    async def _boradcast(self) -> None:
        # todo boradcast message here
        raise NotImplementedError()
        pass

    async def handle(self) -> None:
        try:
            await self._request_check()
            await self._data_operate()
            await self._result_construct()
            await self._response_construct()

            await self._boradcast()
        except UniSpyException as ex:
            self.logger.error(ex.message)
            self._response = ErrorResponse(message=ex.message)
        except Exception as ex:
            self.logger.error(ex)
            self._response = ErrorResponse(message=str(ex))


class MessageHandlerBase(ChannelHandlerBase):
    pass
