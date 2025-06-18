import requests
from frontends.gamespy.library.abstractions.brocker import BrockerBase
from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.library.network.brockers import WebSocketBrocker
from frontends.gamespy.library.network.udp_handler import UdpServer
from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client
from frontends.gamespy.protocols.game_traffic_relay.contracts.general import (
    UpdateGTRServiceRequest,
)


class ServerLauncher(ServerLauncherBase):
    _broker: BrockerBase

    # todo: implement the websocket brocker to receive the info from backends
    def __init__(self) -> None:
        super().__init__()
        self.config = CONFIG.servers["GameTrafficRelay"]
        self._broker = WebSocketBrocker(
            name=self.config.server_name,
            url=f"{CONFIG.backend.url}/GameTrafficRelay/ws",
            call_back_func=self._process_brocker_message,
        )

    def _process_brocker_message(self, message):
        # todo handle message here
        pass

    def _launch_server(self):
        assert self.config is not None
        assert self.logger is not None
        self.server = UdpServer(self.config, Client, self.logger)
        super()._launch_server()

    def _gtr_heartbeat(self):
        assert self.config
        req = UpdateGTRServiceRequest(
            server_id=self.config.server_id,
            public_ip_address=self.config.public_address,
            public_port=self.config.listening_port,
            client_count=len(Client.client_pool),
        )
        try:
            resp = requests.post(
                url=CONFIG.backend.url + "/heartbeat", data=req.model_dump_json()
            )
            if resp.status_code == 200:
                data = resp.json()
                if data["status"] != "online":
                    raise UniSpyException(
                        f"backend server: {CONFIG.backend.url} not available."
                    )
        except requests.ConnectionError:
            raise UniSpyException(
                f"backend server: {CONFIG.backend.url} not available."
            )

    def _heartbeat_to_backend(self):
        self._gtr_heartbeat()
        super()._heartbeat_to_backend()

    def _connect_to_backend(self):
        """
        check backend availability
        """
        super()._connect_to_backend()
        if CONFIG.unittest.is_collect_request:
            return
        try:
            # post our server config to backends to register
            assert self.config is not None
            data = self.config.model_dump_json()
            import json

            data = json.loads(data)
            data["clients"] = len(Client.client_pool)
            resp = requests.post(url=CONFIG.backend.url + "/heartbeat", json=data)
            if resp.status_code == 200:
                data = resp.json()
                if data["status"] != "online":
                    raise UniSpyException(
                        f"backend server: {CONFIG.backend.url} not available."
                    )
        except requests.ConnectionError:
            raise UniSpyException(
                f"backend server: {CONFIG.backend.url} not available."
            )
