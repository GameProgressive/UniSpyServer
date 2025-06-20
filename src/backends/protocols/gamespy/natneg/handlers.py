from datetime import datetime, timezone
from backends.library.abstractions.handler_base import HandlerBase
from backends.library.database.pg_orm import InitPacketCaches
import backends.protocols.gamespy.natneg.data as data
from backends.protocols.gamespy.natneg.helpers import NatProtocolHelper, NatStrategy
from backends.protocols.gamespy.natneg.requests import ConnectRequest, InitRequest
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
)
from frontends.gamespy.protocols.natneg.contracts.results import ConnectResult


class InitHandler(HandlerBase):
    _request: InitRequest

    def __init__(self, request: InitRequest) -> None:
        assert isinstance(request, InitRequest)
        super().__init__(request)

    def _data_operate(self) -> None:
        info = data.get_init_info(
            self._request.cookie, self._request.client_index, self._request.port_type
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
            data.add_init_packet(info)
        else:
            info.update_time = datetime.now(timezone.utc)  # type: ignore
            data.update_init_info(info)


class ConnectHandler(HandlerBase):
    _request: ConnectRequest
    _strategy: NatStrategy

    def __init__(self, request: ConnectRequest) -> None:
        super().__init__(request)
        assert isinstance(request, ConnectRequest)

    def _data_operate(self) -> None:
        # analysis NAT of both parties and find the proper ips
        init_infos_1 = data.get_init_infos(
            self._request.cookie, self._request.client_index
        )
        # choose the other index of the currect client
        if self._request.client_index == NatClientIndex.GAME_CLIENT:
            client_index_2 = NatClientIndex.GAME_SERVER
        else:
            client_index_2 = NatClientIndex.GAME_CLIENT
        init_infos_2 = data.get_init_infos(self._request.cookie, client_index_2)
        if len(init_infos_1) == 0 or len(init_infos_2) == 0:
            raise UniSpyException(
                f"no init info found for cookie {self._request.cookie}"
            )
        assert isinstance(init_infos_1[0].public_ip, str)
        assert isinstance(init_infos_2[0].public_ip, str)
        nat_fail_infos = data.get_nat_fail_info_by_ip(
            init_infos_1[0].public_ip, init_infos_2[0].public_ip
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
        if self._strategy == NatStrategy.USE_PRIVATE_IP:
            self._result = ConnectResult(
                ip=self._helper2.private_ip,
                port=self._helper2.private_port,
                version=self._helper2.version,
                cookie=self._helper2.cookie,
            )
        elif self._strategy == NatStrategy.USE_PUBLIC_IP:
            self._result = ConnectResult(
                ip=self._helper2.public_ip,
                port=self._helper2.public_port,
                version=self._helper2.version,
                cookie=self._helper2.cookie,
            )

        elif self._strategy == NatStrategy.USE_GAME_TRAFFIC_RALEY:
            # get a small number of players server from database
            relay_servers = data.get_game_traffic_relay_servers(5)
            # select strategy to choose one gtr server
            rs = relay_servers[0]
            assert isinstance(rs.public_ip_address, str)
            assert isinstance(rs.public_port, int)
            self._result = ConnectResult(
                ip=rs.public_ip_address,
                port=rs.public_port,
                version=self._helper2.version,
                cookie=self._helper2.cookie,
            )
