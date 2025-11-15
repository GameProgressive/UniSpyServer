from types import MappingProxyType

from frontends.gamespy.library.abstractions.connections import NetworkServerBase
from frontends.gamespy.library.exceptions.general import UniSpyException
import schedule
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


class ServiceBase:
    config: ServerConfig
    _config_name: str
    _network_server_cls: type[NetworkServerBase]
    _client_cls: type[ClientBase]
    _logger: LogWriter
    _network_server: NetworkServerBase
    _available_checker: object

    def __init__(
        self,
        config_name: str,
        client_cls: type[ClientBase],
        network_server_cls: type[NetworkServerBase],
    ):
        assert issubclass(client_cls, ClientBase)
        assert issubclass(network_server_cls, NetworkServerBase)
        assert isinstance(config_name, str)
        assert config_name in CONFIG.servers
        self.config = CONFIG.servers[config_name]
        self._network_server_cls = network_server_cls
        self._client_cls = client_cls
        self._create_logger()
        self._create_network_server()

    @final
    def _create_logger(self):
        assert self.config is not None
        short_name = _SERVER_FULL_SHORT_NAME_MAPPING[self.config.server_name]
        self._logger = LogManager.create(short_name)

    @final
    def _create_network_server(self):
        assert self._logger is not None
        assert self._network_server_cls is not None
        assert self._client_cls is not None
        self._network_server = self._network_server_cls(
            self.config, self._client_cls, self._logger)

    @final
    def start(self):
        self._network_server.start()

    @final
    def stop(self):
        self._network_server.stop()

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
        ServiceBase.get_data_from_backends(url, json_str=json_str)

    def _connect_to_backend(self):
        """
        check backend availability
        """
        assert self.config is not None
        if CONFIG.unittest.is_collect_request:
            return
        self._heartbeat_to_backend(
            CONFIG.backend.url, self.config.model_dump_json())

    @final
    def start_post_tasks(self):
        """
        run post tasks
            - set the schedular to send heartbeat info to backend to keep the infomation update
        """
        # launch schedular
        schedule.every(30).seconds.do(self._post_task)

    def _post_task(self):
        """
        the post task after network server launched
        call this function in server factory
        """
        self._connect_to_backend()


class ServicesFactory:
    _lauchers: list[ServiceBase]

    def __init__(
        self,
        launchers: list[ServiceBase]
    ):
        self._lauchers = launchers

    def start(self):
        self.__show_unispy_logo()
        self._launch_servers()
        print("Server successfully launched.")
        print("Press ctr+c to Quit\n")
        self._run_post_tasks()
        self._keep_running()

    def _run_post_tasks(self):
        """
        run the launcher post task
        """
        for launcher in self._lauchers:
            launcher.start_post_tasks()
        # call all post tasks immediately
        schedule.run_all()

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
                    info.config.listening_address,
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
        from time import sleep
        try:
            while True:
                sleep(1)
                # run schedule here
                schedule.run_pending()
                pass
        except KeyboardInterrupt:
            for info in self._lauchers:
                info.stop()
            print("\nUniSpy shutdown.")
