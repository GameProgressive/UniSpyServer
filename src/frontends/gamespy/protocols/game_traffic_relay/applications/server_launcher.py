from datetime import datetime, timedelta
from frontends.gamespy.library.abstractions.server_launcher import ServicesFactory, ServiceBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER
from frontends.gamespy.library.network.udp_handler import UdpServer
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.applications.connection import ConnectionListener
from frontends.gamespy.protocols.game_traffic_relay.contracts.general import (
    GtrHeartbeat,
)


class Service(ServiceBase):

    def __init__(self) -> None:
        super().__init__(
            config_name="GameTrafficRelay",
            client_cls=Client,
            network_server_cls=UdpServer,
        )

    def _post_task(self):
        super()._post_task()
        self.__gtr_heartbeat()
        self.__check_expired_connection()

    def __gtr_heartbeat(self):
        assert self.config
        req = GtrHeartbeat(
            server_id=self.config.server_id,
            public_ip_address=self.config.listening_address,
            public_port=self.config.listening_port,
            client_count=len(ConnectionListener.client_pool),
        )
        req_str = req.model_dump_json()
        self._heartbeat_to_backend(
            f"{CONFIG.backend.url}/GameSpy/GameTrafficRelay/Heartbeat", req_str
        )

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
    gtr = Service()
    helper = DebugHelper("./frontends/", ServicesFactory([gtr]))
    helper.start()
