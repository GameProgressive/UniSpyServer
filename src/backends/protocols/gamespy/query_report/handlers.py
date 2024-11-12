from datetime import datetime
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import PG_SESSION, GameServerCaches
from backends.protocols.gamespy.query_report.requests import *
from servers.query_report.src.aggregates.exceptions import QRException


class AvaliableHandler(HandlerBase):
    _request: AvaliableRequest


class ChallengeHandler(HandlerBase):
    _request: HeartBeatRequest

    async def _data_operate(self) -> None:
        cache = PG_SESSION.query(GameServerCaches).where(
            GameServerCaches.instant_key == self._request.instant_key).first()
        if cache is None:
            raise QRException(
                "No server found, please make sure there is a server.")
        cache.avaliable = True  # type: ignore
        PG_SESSION.commit()


class Heartbeathandler(HandlerBase):
    _request: HeartBeatRequest

    async def _data_operate(self) -> None:
        cache = PG_SESSION.query(GameServerCaches).where(
            GameServerCaches.instant_key == self._request.instant_key).first()
        if cache is None:
            cache = GameServerCaches(instant_key=self._request.instant_key,
                                     server_id=self._request.server_id,
                                     host_ip_address=self._request.client_ip,
                                     game_name=self._request.game_name,
                                     query_report_port=self._request.client_port,
                                     update_time=datetime.now(),
                                     status=self._request.server_status,
                                     player_data=self._request.player_data,
                                     server_data=self._request.server_data,
                                     team_data=self._request.team_data,
                                     avaliable=True)
        else:
            cache.instant_key = self._request.instant_key  # type: ignore
            cache.server_id = self._request.server_id  # type: ignore
            cache.host_ip_address = self._request.client_ip  # type: ignore
            cache.game_name = self._request.game_name  # type: ignore
            cache.query_report_port = self._request.client_port  # type: ignore
            cache.update_time = datetime.now()  # type: ignore
            cache.status = self._request.server_status  # type: ignore
            cache.player_data = self._request.player_data  # type: ignore
            cache.server_data = self._request.server_data  # type: ignore
            cache.team_data = self._request.team_data  # type: ignore
            cache.avaliable = True  # type: ignore

        PG_SESSION.commit()


class KeepAliveHandler(HandlerBase):
    _request: KeepAliveRequest

    async def _data_operate(self) -> None:
        cache = PG_SESSION.query(GameServerCaches).where(
            GameServerCaches.instant_key == self._request.instant_key).first()
        # update heartbeat time
        cache.update_time = datetime.now()  # type: ignore

        PG_SESSION.commit()
