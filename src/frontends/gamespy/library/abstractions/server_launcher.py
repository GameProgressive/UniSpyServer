from threading import Thread, Event
from types import MappingProxyType

from frontends.gamespy.library.abstractions.connections import NetworkServerBase
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.library.extentions.schedular import Schedular
from frontends.gamespy.library.log.log_manager import LogManager, LogWriter
from frontends.gamespy.library.configs import CONFIG, ServerConfig
import pyfiglet
import requests
from prettytable import PrettyTable
from frontends.gamespy.library.abstractions.client import ClientBase
from typing import final

VERSION = 0.46
_SERVER_FULL_SHORT_NAME_MAPPING = MappingProxyType(
    {
        "PresenceConnectionManager": "PCM",
        "PresenceSearchPlayer": "PSP",
        "CDKey": "CDKey",
        "ServerBrwoserV1": "SBv1",
        "ServerBrowserV2": "SBv2",
        "QueryReport": "QR",
        "NatNegotiation": "NN",
        "GameStatus": "GS",
        "Chat": "Chat",
        "WebServices": "Web",
        "GameTrafficRelay": "GTR",
    }
)


class ServerLauncherBase:
    config: ServerConfig
    _config_name: str
    _server_cls: type[NetworkServerBase]
    _client_cls: type[ClientBase]
    _logger: LogWriter
    _server: NetworkServerBase
    _available_checker: Schedular

    def __init__(
        self,
        config_name: str,
        client_cls: type[ClientBase],
        server_cls: type[NetworkServerBase],
    ):
        assert issubclass(client_cls, ClientBase)
        assert issubclass(server_cls, NetworkServerBase)
        assert isinstance(config_name, str)
        assert config_name in CONFIG.servers
        self.config = CONFIG.servers[config_name]
        self._server_cls = server_cls
        self._client_cls = client_cls
        self._create_logger()
        self._create_server()

    @final
    def _create_logger(self):
        assert self.config is not None
        short_name = _SERVER_FULL_SHORT_NAME_MAPPING[self.config.server_name]
        self._logger = LogManager.create(short_name)

    @final
    def _create_server(self):
        assert self._logger is not None
        assert self._server_cls is not None
        assert self._client_cls is not None
        self._server = self._server_cls(
            self.config, self._client_cls, self._logger)

    @final
    def start(self):
        self._server.start()

    @final
    def stop(self):
        self._server.stop()

    @staticmethod
    def get_data_from_backends(url: str, json_str: str):
        try:
            # post our server config to backends to register
            resp = requests.post(url=url, data=json_str)

            data = resp.json()
            if resp.status_code != 200:
                raise UniSpyException(data["message"])
            else:
                return data
        except requests.ConnectionError:
            raise UniSpyException(
                f"backend server: {CONFIG.backend.url} not available."
            )

    @final
    def _heartbeat_to_backend(self, url: str, json_str: str):
        """
        send heartbeat to backends
        """
        assert isinstance(json_str, str)
        ServerLauncherBase.get_data_from_backends(url, json_str=json_str)

    def connect_to_backend(self):
        """
        check backend availability
        """
        assert self.config is not None
        if CONFIG.unittest.is_collect_request:
            return
        self._heartbeat_to_backend(
            CONFIG.backend.url, self.config.model_dump_json())

    @final
    def launch_heartbeat_schedular(self):
        """
        set the schedular to send heartbeat info to backend to keep the infomation update
        """
        #! temperarily use connect to backend function
        self._available_checker = Schedular(self.connect_to_backend, 30)
        self._available_checker.start()


class ServerFactory:
    _lauchers: list[ServerLauncherBase]

    def __init__(
        self,
        launchers: list[ServerLauncherBase]
    ):
        self._lauchers = launchers

    def start(self):
        self._connect_to_backend()
        self.__show_unispy_logo()
        self._launch_servers()
        print("Server successfully launched.")
        self._keep_running()

    def _connect_to_backend(self):
        for info in self._lauchers:
            info.connect_to_backend()
            info.launch_heartbeat_schedular()

    def __show_unispy_logo(self):
        # display logo
        print(pyfiglet.Figlet().renderText("UniSpy.Server"))
        # display version info
        print(f"version {VERSION}")
        table = PrettyTable()
        table.field_names = [
            "Server Name",
            "Listening Address",
            "Listening Port",
        ]
        for info in self._lauchers:
            table.add_row(
                [
                    info.config.server_name,
                    info.config.public_address,
                    info.config.listening_port,
                ]
            )
        print(table)

    @final
    def _launch_servers(self) -> None:
        """
        assign data in child class so the related instance can be created here
        """

        for info in self._lauchers:
            info.start()

    @final
    def _keep_running(self):
        print("Press ctr+c to Quit\n")
        from time import sleep
        try:
            while True:
                sleep(1)
                pass
        except KeyboardInterrupt:
            for info in self._lauchers:
                info.stop()
            print("\nUniSpy shutdown.")
