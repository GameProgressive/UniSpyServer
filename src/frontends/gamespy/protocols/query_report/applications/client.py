from frontends.gamespy.library.abstractions.client import ClientBase

# import servers.query_report.v1


class Client(ClientBase):
    pool: dict[str, "Client"]
    is_log_raw: bool = True

    def _create_switcher(self, buffer: bytes):
        from frontends.gamespy.protocols.query_report.v2.applications.switcher import Switcher as V2CmdSwitcher
        assert isinstance(buffer, bytes)
        if buffer[0] == ord("\\"):
            raise NotImplementedError("v1 protocol not implemented")
            return V1CmdSwitcher(self, (buffer))
        else:
            return V2CmdSwitcher(self, buffer)
