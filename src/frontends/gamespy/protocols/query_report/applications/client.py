from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.connections import ConnectionBase
from frontends.gamespy.library.configs import ServerConfig
from frontends.gamespy.library.log.log_manager import LogWriter

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
