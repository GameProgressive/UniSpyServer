from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.library.network.udp_handler import UdpServer
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.contracts.general import (
    GtrHeartbeat,
)
import requests


class ServerLauncher(ServerLauncherBase):
    _public_ip: str

    def __init__(self) -> None:
        super().__init__(
            config_name="GameTrafficRelay",
            client_cls=Client,
            server_cls=UdpServer,
        )
        self._get_public_ip()

    def _get_public_ip(self):
        try:
            # post our server config to backends to register
            resp = requests.get(
                url=f"{CONFIG.backend.url}/GameSpy/GameTrafficRelay/get_my_ip")
            data = resp.json()
            if resp.status_code == 200:
                self._public_ip = data['ip']
                print(f"GameTrafficRelay public ip: {self._public_ip}")
            else:
                raise UniSpyException(data["error"])
        except requests.ConnectionError:
            raise UniSpyException(
                f"backend server: {CONFIG.backend.url} not available."
            )

    def _gtr_heartbeat(self):
        assert self.config
        req = GtrHeartbeat(
            server_id=self.config.server_id,
            public_ip_address=self._public_ip,
            public_port=self.config.listening_port,
            client_count=len(Client.client_pool),
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


if __name__ == "__main__":
    s = ServerLauncher()
    s.start()
