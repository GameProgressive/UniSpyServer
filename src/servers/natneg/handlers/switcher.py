from library.abstractions.switcher import SwitcherBase
from servers.natneg.applications.client import Client


class CmdSwitcher(SwitcherBase):
    _rawRequest: bytes
    _client: Client

    def __init__(self, client: Client, rawRequest: bytes) -> None:
        super().__init__(client, rawRequest)
        assert issubclass(client, Client)
        assert isinstance(rawRequest, bytes)

    def _process_raw_request(self) -> None:
        name = self._rawRequest[7]
        self._requests.append((name, self._rawRequest))

    def _create_cmd_handlers(self, name: int, rawRequest: bytes) -> None:
        assert isinstance(name, int)
        assert isinstance(rawRequest, bytes)
