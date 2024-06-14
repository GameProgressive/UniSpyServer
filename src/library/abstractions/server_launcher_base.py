import abc
from library.log.log_manager import LogManager, LogWriter
from library.unispy_server_config import CONFIG, ServerConfig
import pyfiglet
import requests

__server_name_mapping = {
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


class ServerLauncherBase(abc.ABC):
    config: ServerConfig
    logger: LogWriter

    def start(self):
        self.__show_unispy_logo()
        self._connect_to_backend()
        self._create_logger()
        self._launch_server()

    def __show_unispy_logo(self):
        print(pyfiglet.Figlet().renderText("UniSpy.Server"))

    @abc.abstractmethod
    def _launch_server(self) -> None:
        pass

    def _connect_to_backend(self):
        try:
            resp: requests.Response = requests.get(url=CONFIG.backend.url)
            if resp.status_code == 200:
                data = resp.json()
                if data["status"] != "online":
                    raise Exception(
                        f"backend server: {CONFIG.backend.url} not available."
                    )
        except:
            raise Exception(f"backend server: {CONFIG.backend.url} not available.")

    def _create_logger(self):
        short_name = __server_name_mapping[self.config.server_name]
        self.logger = LogManager.create(CONFIG.logging.path, short_name)
