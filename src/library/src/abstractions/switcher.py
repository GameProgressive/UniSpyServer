import abc
from library.src.abstractions.client import ClientBase
from library.src.abstractions.handler import CmdHandlerBase
from typing import List, Optional


class SwitcherBase:

    _client: ClientBase
    _raw_request: object
    _handlers: List[CmdHandlerBase] = []
    _requests: List[tuple] = []
    """
    [
        (request_name,raw_request),
        (request_name,raw_request),
        (request_name,raw_request),
        ...
    ]

    """

    def __init__(self, client: ClientBase, raw_request: Optional[bytes | str]) -> None:
        assert isinstance(client, ClientBase)
        self._client = client
        self._raw_request = raw_request

    def handle(self):
        from library.src.exceptions.general import UniSpyException

        try:
            self._process_raw_request()
            if len(self._requests) == 0:
                return
            for request in self._requests:
                handler = self._create_cmd_handlers(request[0], request[1])
                if handler is None:
                    self._client.log_warn("Request ignored.")
                    continue
                self._handlers.append(handler)
            if len(self._handlers) == 0:
                return

            for handler in self._handlers:
                handler.handle()
        except Exception as e:
            UniSpyException.handle_exception(e, self._client)

    @abc.abstractmethod
    def _process_raw_request(self) -> None:
        pass

    @abc.abstractmethod
    def _create_cmd_handlers(self, name: object, raw_request: object) -> Optional[CmdHandlerBase]:
        pass
