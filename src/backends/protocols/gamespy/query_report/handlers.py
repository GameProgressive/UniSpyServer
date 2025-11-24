from backends.library.abstractions.contracts import OKResponse
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import ENGINE, GameServerCaches
from backends.protocols.gamespy.query_report.requests import (
    AvaliableRequest,
    ClientMessageRequest,
    HeartBeatRequest,
    KeepAliveRequest,
    LegacyHeartbeatRequest,
)
from frontends.gamespy.protocols.query_report.aggregates.enums import GameServerStatus
from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
import backends.protocols.gamespy.query_report.data as data
from sqlalchemy.orm import Session


class AvaliableHandler(HandlerBase):
    _request: AvaliableRequest
    response: OKResponse


class ChallengeHandler(HandlerBase):
    _request: HeartBeatRequest
    response: OKResponse

    def _data_operate(self) -> None:
        with Session(ENGINE) as session:
            cache = (
                session.query(GameServerCaches)
                .where(GameServerCaches.instant_key == self._request.instant_key)
                .first()
            )
            if cache is None:
                raise QRException(
                    "No server found, please make sure there is a server."
                )
            cache.avaliable = True  # type: ignore
            session.commit()


class HeartbeatHandler(HandlerBase):
    """
    v2 protocol qr heartbeat
    this heartbeat have instantkey which is using for natneg
    """
    _request: HeartBeatRequest
    response: OKResponse

    def _data_operate(self) -> None:
        # clean the expired server cache
        data.clean_expired_game_server_cache(self._session)

        cache = data.get_game_server_cache_by_ip_port(
            self._request.client_ip, self._request.client_port, self._session
        )

        if cache is None:
            # todo check whether these data can be null at first heartbeat
            if len(self._request.data) == 0:
                raise QRException(
                    "data in first heartbeat can not be null")
            # team data can be none in peertest sdk
            cache = GameServerCaches(
                instant_key=self._request.instant_key,
                server_id=self._request.server_id,
                host_ip_address=self._request.client_ip,
                game_name=self._request.game_name,
                query_report_port=self._request.client_port,
                status=self._request.status,
                data=self._request.data,
                avaliable=True,
            )
            data.create_game_server_cache(cache, self._session)
        else:
            data.update_game_server_cache(
                cache=cache,
                instant_key=self._request.instant_key,
                server_id=self._request.server_id,
                host_ip_address=self._request.client_ip,
                game_name=self._request.game_name,
                query_report_port=self._request.client_port,
                server_status=self._request.status,
                data=self._request.data,
                session=self._session,
            )


class LegacyHeartbeatHandler(HandlerBase):
    """
    same as HeartbeatHandler
    The v1 protocol heartbeat do not have instantkey
    """
    _request: LegacyHeartbeatRequest
    response: OKResponse

    def _data_operate(self) -> None:
        # clean the expired server cache
        data.clean_expired_game_server_cache(self._session)

        cache = data.get_game_server_cache_by_ip_port(
            self._request.client_ip, self._request.client_port, self._session
        )

        if cache is None:
            cache = GameServerCaches(
                instant_key=None,
                server_id=self._request.server_id,
                host_ip_address=self._request.client_ip,
                game_name=self._request.game_name,
                query_report_port=self._request.client_port,
                status=GameServerStatus.NORMAL,
                data=self._request.data,
                avaliable=True,
            )
            data.create_game_server_cache(cache, self._session)
        else:
            data.update_game_server_cache(
                cache=cache,
                instant_key=None,
                server_id=self._request.server_id,
                host_ip_address=self._request.client_ip,
                game_name=self._request.game_name,
                query_report_port=self._request.client_port,
                server_status=GameServerStatus.NORMAL,
                data=self._request.data,
                session=self._session,
            )


class KeepAliveHandler(HandlerBase):
    _request: KeepAliveRequest
    response: OKResponse

    def _data_operate(self) -> None:
        assert isinstance(self._request.instant_key, str)
        data.refresh_game_server_cache(
            self._request.client_ip, self._request.client_port, self._session)


class ClientMessageHandler(HandlerBase):
    _request: ClientMessageRequest
    response: OKResponse

    def _response_construct(self) -> None:
        # todo use websocket to send the message to qr client, but how to determine which qr router should be received
        # currently we just use broadcast message to all qr frontends
        from backends.protocols.gamespy.query_report.broker import MANAGER
        MANAGER.broadcast(self._request.model_dump_json())
