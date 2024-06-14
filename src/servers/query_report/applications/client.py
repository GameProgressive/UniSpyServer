from library.abstractions.client import ClientBase

# import servers.query_report.v1
from servers.query_report.v2.applications.switcher import CmdSwitcher as V2CmdSwitcher


class Client(ClientBase):
    is_log_raw: bool = True

    def _create_switcher(self, buffer):
        if buffer[0] == ord("\\"):
            raise NotImplementedError("v1 protocol not implemented")
            return V1CmdSwitcher(self, (buffer))
        else:
            return V2CmdSwitcher(self, buffer)
