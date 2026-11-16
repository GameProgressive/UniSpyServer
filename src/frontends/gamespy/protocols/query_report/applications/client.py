from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.connections import ConnectionBase
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.library.log.log_manager import LogWriter
from frontends.gamespy.protocols.query_report.v2.applications.message import WebsocketMessage

class Client(ClientBase):
    """
    v1 protocol request is send with different packet, the \\final\\ tells whether request is finished
    """
    pool: dict[str, "Client"] = {}
    is_log_raw: bool

    def __init__(
        self, connection: ConnectionBase, server_config: ServerConfig, logger: LogWriter
    ):
        super().__init__(connection, server_config, logger)
        self.is_log_raw = True

    def _create_switcher(self, buffer: bytes):
        from frontends.gamespy.protocols.query_report.v1.applications.switcher import (
            Switcher as V1Switcher,
        )
        from frontends.gamespy.protocols.query_report.v2.applications.switcher import (
            Switcher as V2Switcher,
        )

        assert isinstance(buffer, bytes)
        # !! qr v1 doesn't actually need encryption because encryption isn't part of the main communication process.
        if buffer[0] == ord("\\"):
            return V1Switcher(self, buffer.decode())
        else:
            return V2Switcher(self, buffer)

    @staticmethod
    def websocket_message_callback(message: str) -> None:
        # todo process channel message
        try:
            assert isinstance(message, str)
            ws_message = WebsocketMessage.model_validate_json(message)
            client = Client.pool.get(ws_message.message.target_ip_endpoint)
            if client is None:
                print(
                    f"can not find the game server:{ws_message.message.target_ip_endpoint}")
                return
            from frontends.gamespy.protocols.query_report.v2.applications.handlers import ClientMessageHandler
            handler = ClientMessageHandler(client, ws_message.message)
            handler.handle()
        except Exception as e:
            print(e)

    def _process_brocker_message(self, message: str):
        from frontends.gamespy.protocols.query_report.v2.contracts.requests import (
            ClientMessageRequest,
        )
        from frontends.gamespy.protocols.query_report.v2.applications.handlers import (
            ClientMessageHandler,
        )

        # responsible for receive message and send out
        # TODO: check whether exception here will cause brocker stop
        try:
            assert isinstance(message, str)
            # deserialize str to object
            request = ClientMessageRequest.model_validate_strings(message)
            handler = ClientMessageHandler(self, request)
            handler.handle()
        except Exception as e:
            # todo change to log and handle exception here
            print(e)
