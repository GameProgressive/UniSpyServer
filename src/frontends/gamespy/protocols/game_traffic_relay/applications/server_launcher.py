from datetime import datetime, timedelta
from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.extentions.schedular import Schedular
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER
from frontends.gamespy.library.network.udp_handler import UdpServer
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.applications.connection import ConnectionListener
from frontends.gamespy.protocols.game_traffic_relay.contracts.general import (
    GtrHeartbeat,
)


class ServerLauncher(ServerLauncherBase):
    _public_ip: str

    def __init__(self) -> None:
        super().__init__(
            config_name="GameTrafficRelay",
            client_cls=Client,
            server_cls=UdpServer,
        )
        self._get_public_ip()
        self._launch_connection_expire_checker()

    def _get_public_ip(self):
        url = f"{CONFIG.backend.url}/GameSpy/GameTrafficRelay/get_my_ip"
        data = self._get_data_from_backends(url=url, is_post=False)
        self._public_ip = data['ip']

    def _gtr_heartbeat(self):
        assert self.config
        req = GtrHeartbeat(
            server_id=self.config.server_id,
            public_ip_address=self.config.public_address,
            public_port=self.config.listening_port,
            client_count=len(ConnectionListener.client_pool),
        )
        req_str = req.model_dump_json()
        self._heartbeat_to_backend(
            f"{CONFIG.backend.url}/GameSpy/GameTrafficRelay/heartbeat", req_str
        )

    def _connect_to_backend(self):
        """
        check backend availability
        """
        assert self.logger is not None
        if CONFIG.unittest.is_collect_request:
            self.logger.debug(
                "CONFIG.unittest.is_collect_request is enabled ignore send heartbeat to backend"
            )
            return
        super()._connect_to_backend()
        self._gtr_heartbeat()

    # region Expire Checker
    def _launch_connection_expire_checker(self):
        self._conn_checker = Schedular(self.__check_expired_connection, 30)
        self._conn_checker.start()

    def __check_expired_connection(self):
        expired_time = datetime.now() - timedelta(seconds=30)
        try:
            for key in ConnectionListener.cookie_pool.keys():
                pair = ConnectionListener.cookie_pool[key]
                if pair[0].info.last_receive_time < expired_time:
                    del ConnectionListener.cookie_pool[key]
                    del ConnectionListener.client_pool[pair[0].connection.ip_endpoint]
                    del ConnectionListener.client_pool[pair[1].connection.ip_endpoint]
        except Exception as e:
            GLOBAL_LOGGER.warn(f"Errors occured when doing cookie delete: {e}")


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    helper = DebugHelper(
        "./frontends/gamespy/protocols/game_traffic_relay", ServerLauncher)
    helper.start()
