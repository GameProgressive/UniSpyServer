from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.connections import ConnectionBase
from frontends.gamespy.library.configs import CONFIG, ServerConfig
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.library.network.websocket_brocker import WebSocketBrocker

# import servers.query_report.v1


class Client(ClientBase):
    pool: dict[str, "Client"]
    is_log_raw: bool

    def __init__(self, connection: ConnectionBase, server_config: ServerConfig, logger: LogWriter):
        super().__init__(connection, server_config, logger)
        self.is_log_raw = True

    def _create_switcher(self, buffer: bytes):
        from frontends.gamespy.protocols.query_report.v2.applications.switcher import Switcher as V2CmdSwitcher
        assert isinstance(buffer, bytes)
        if buffer[0] == ord("\\"):
            raise NotImplementedError("v1 protocol not implemented")
            return V1CmdSwitcher(self, (buffer))
        else:
            return V2CmdSwitcher(self, buffer)

    def start_brocker(self):
        self.brocker = WebSocketBrocker(
            name=self.server_config.server_name,
            url=f"{CONFIG.backend.url.replace('http', 'ws')}/GameSpy/Chat/ws",
            call_back_func=self._process_brocker_message,
        )
        self.brocker.subscribe()

    def stop_brocker(self):
        assert self.brocker is not None
        self.brocker.unsubscribe()

    def _process_brocker_message(self, message: str):
        from frontends.gamespy.protocols.query_report.v2.contracts.requests import ClientMessageRequest
        from frontends.gamespy.protocols.query_report.v2.applications.handlers import ClientMessageHandler

        # responsible for receive message and send out
        # TODO: check whether exception here will cause brocker stop
        try:
            assert isinstance(message, str)
            # deserialize str to object
            request = ClientMessageRequest()
            handler = ClientMessageHandler(self,  request)
            handler.handle()
        except Exception as e:
            # todo change to log and handle exception here
            print(e)
