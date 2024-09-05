import abc
from library.src.exceptions.general import UniSpyException
from library.src.log.log_manager import LogManager, LogWriter
from library.src.unispy_server_config import CONFIG, ServerConfig
import pyfiglet
import requests

VERSION = 0.45

__SERVER_FULL_SHORT_NAME_MAPPING = {
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


class ServerLauncherBase:
    config: ServerConfig
    logger: LogWriter

    def start(self):
        self.__show_unispy_logo()
        self._connect_to_backend()
        self._create_logger()
        self._launch_server()

    def __show_unispy_logo(self):
        # display logo
        print(pyfiglet.Figlet().renderText("UniSpy.Server"))
        # display version info
        print(f"version {VERSION}")

    @abc.abstractmethod
    def _launch_server(self) -> None:
        pass

    def _connect_to_backend(self):
        try:
            # post our server config to backends to register
            resp: requests.Response = requests.post(
                url=CONFIG.backend.url,
                data=self.config.model_dump_json())
            if resp.status_code == 200:
                data = resp.json()
                if data["status"] != "online":
                    raise Exception(
                        f"backend server: {CONFIG.backend.url} not available."
                    )
        except:
            # fmt: off
            raise UniSpyException(f"backend server: {CONFIG.backend.url} not available.")
            # fmt: on

    def _create_logger(self):
        short_name = __SERVER_FULL_SHORT_NAME_MAPPING[self.config.server_name]
        self.logger = LogManager.create(CONFIG.logging.path, short_name)
