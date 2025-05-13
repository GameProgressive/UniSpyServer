from types import MappingProxyType
from typing import Optional
from frontends.gamespy.library.abstractions.connections import NetworkServerBase
from frontends.gamespy.library.exceptions.general import UniSpyException
from frontends.gamespy.library.log.log_manager import LogManager, LogWriter
from frontends.gamespy.library.configs import CONFIG, ServerConfig
import pyfiglet
import requests
from prettytable import PrettyTable

VERSION = 0.45
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
        "GameTrafficReplay": "GTR",
    }
)


class ServerLauncherBase:
    config: Optional[ServerConfig]
    logger: Optional[LogWriter]
    server: Optional[NetworkServerBase]

    def __init__(self):
        self.server = None
        self.logger = None
        self.config = None

    def start(self):
        self._connect_to_backend()
        self._create_logger()
        self.__show_unispy_logo()
        self._launch_server()
        print("Server successfully launched.")
        self.__keep_running()

    def __show_unispy_logo(self):
        # display logo
        print(pyfiglet.Figlet().renderText("UniSpy.Server"))
        # display version info
        print(f"version {VERSION}")
        table = PrettyTable()
        table.field_names = ["Server Name", "Listening Address", "Listening Port"]
        assert self.config is not None
        table.add_row(
            [
                self.config.server_name,
                self.config.public_address,
                self.config.listening_port,
            ]
        )
        print(table)

    def _launch_server(self) -> None:
        if self.server is None:
            raise UniSpyException("Create network server in child class")
        print("Press Ctrl+C to Quit")
        self.server.start()

    def _connect_to_backend(self):
        if CONFIG.unittest.is_collect_request:
            return
        try:
            # post our server config to backends to register
            assert self.config is not None
            resp = requests.post(
                url=CONFIG.backend.url + "/", data=self.config.model_dump_json()
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

    def _create_logger(self):
        assert self.config is not None
        short_name = _SERVER_FULL_SHORT_NAME_MAPPING[self.config.server_name]
        self.logger = LogManager.create(short_name)

    def __keep_running(self):
        _ = input("Press Q to Quit")
