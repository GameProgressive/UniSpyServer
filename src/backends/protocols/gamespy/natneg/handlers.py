from datetime import datetime, timezone
from time import sleep
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import InitPacketCaches, NatResultCaches
import backends.protocols.gamespy.natneg.data as data
from backends.protocols.gamespy.natneg.helpers import NatProtocolHelper, NatStrategy
from backends.protocols.gamespy.natneg.requests import ConnectRequest, InitRequest, ReportRequest
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    ConnectPacketStatus,
    NatClientIndex,
)
from frontends.gamespy.protocols.natneg.contracts.results import ConnectResult


class InitHandler(HandlerBase):
    _request: InitRequest

    def __init__(self, request: InitRequest) -> None:
        assert isinstance(request, InitRequest)
        super().__init__(request)

    def _data_operate(self) -> None:
        info = data.get_init_cache(
            self._request.cookie,
            self._request.client_index,
            self._request.port_type,
            self._session,
        )
        if info is None:
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
            data.add_init_packet(info, self._session)
        else:
            info.update_time = datetime.now(timezone.utc)  # type: ignore
            data.update_init_info(info, self._session)


class ConnectHandler(HandlerBase):
    _request: ConnectRequest
    _strategy: NatStrategy | None
    _is_valid: bool

    def __init__(self, request: ConnectRequest) -> None:
        super().__init__(request)
        assert isinstance(request, ConnectRequest)
        self._strategy = None

    def _data_operate(self) -> None:
        # first sleep 5 time to
        sleep(2)
        self._get_client_pair()

    def _get_client_pair(self) -> None:
        # analysis NAT of both parties and find the proper ips
        init_infos_1 = data.get_init_caches(
            self._request.cookie, self._request.client_index, self._session
        )
        # choose the other index of the currect client
        if self._request.client_index == NatClientIndex.GAME_CLIENT:
            client_index_2 = NatClientIndex.GAME_SERVER
        else:
            client_index_2 = NatClientIndex.GAME_CLIENT
        init_infos_2 = data.get_init_caches(
            self._request.cookie, client_index_2, self._session
        )
        if len(init_infos_1) == 0 or len(init_infos_2) == 0:
            self._is_valid = False
            self.logger.info(
                f"client1 init count:{len(init_infos_1)}, client2 init count:{len(init_infos_2)}")
            return
        else:
            self._is_valid = True

        assert isinstance(init_infos_1[0].public_ip, str)
        assert isinstance(init_infos_2[0].public_ip, str)
        nat_fail_infos = data.get_nat_result_info_by_ip(
            init_infos_1[0].public_ip, self._session
        )
        self._strategy = NatStrategy.USE_GAME_TRAFFIC_RALEY
        if len(nat_fail_infos) != 0:
            self._strategy = NatStrategy.USE_GAME_TRAFFIC_RALEY
        else:
            self._helper1 = NatProtocolHelper(init_infos_1)
            self._helper2 = NatProtocolHelper(init_infos_2)
            self._strategy = NatProtocolHelper.choose_nat_strategy(
                self._helper1, self._helper2
            )

    def _result_construct(self) -> None:
        if not self._is_valid:
            self._result = ConnectResult(
                is_both_client_ready=False,
                ip=None,
                port=None,
                status=None
            )
            return

        if self._strategy == NatStrategy.USE_PRIVATE_IP:
            self._result = ConnectResult(
                is_both_client_ready=True,
                ip=self._helper2.private_ip,
                port=self._helper2.private_port,
                status=ConnectPacketStatus.NO_ERROR
            )
        elif self._strategy == NatStrategy.USE_PUBLIC_IP:
            self._result = ConnectResult(
                is_both_client_ready=True,
                ip=self._helper2.public_ip,
                port=self._helper2.public_port,
                status=ConnectPacketStatus.NO_ERROR
            )

        elif self._strategy == NatStrategy.USE_GAME_TRAFFIC_RALEY:
            # get a small number of players server from database
            relay_servers = data.get_game_traffic_relay_servers(
                self._session, 5)
            # select strategy to choose one gtr server
            rs = relay_servers[0]
            assert isinstance(rs.public_ip, str)
            assert isinstance(rs.public_port, int)
            self._result = ConnectResult(
                is_both_client_ready=True,
                ip=rs.public_ip,
                port=rs.public_port,
                status=ConnectPacketStatus.NO_ERROR
            )


class ReportHandler(HandlerBase):
    _request: ReportRequest

    def _data_operate(self) -> None:
        init_cache = data.get_init_cache(
            self._request.cookie,
            self._request.client_index,
            self._request.port_type,
            self._session
        )
        if init_cache is None:
            raise UniSpyException(
                f"No init package found for report pacakge cookie: {self._request.cookie} client_index: {self._request.client_index}")
        report_cache = NatResultCaches(
            cookie=self._request.cookie,
            public_ip=init_cache.public_ip,
            private_ip=init_cache.private_ip,
            is_success=self._request.is_nat_success,
            nat_type=self._request.nat_type,
            client_index=self._request.client_index,
            game_name=self._request.game_name,
        )
        data.update_nat_result_info(report_cache, self._session)
