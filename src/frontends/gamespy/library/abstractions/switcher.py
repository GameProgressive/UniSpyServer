from abc import abstractmethod
from frontends.gamespy.library.abstractions.client import ClientBase
from frontends.gamespy.library.abstractions.handler import CmdHandlerBase
from typing import Optional


class SwitcherBase:
    """
    class member type hint can use class static member, but you can not initialize any class static member here! Init it in the __init__() function 
    """
    _handlers: list[CmdHandlerBase]
    _requests: list[tuple[object, object]]
    _raw_request: object

    def __init__(self, client: ClientBase, raw_request: bytes | str) -> None:
        assert isinstance(client, ClientBase)
        assert isinstance(raw_request, bytes) or isinstance(raw_request, str)
        self._client: ClientBase = client
        self._raw_request: object = raw_request
        self._handlers: list[CmdHandlerBase] = []
        self._requests: list[tuple[object, object]] = []
        """
        [
            (request_name,raw_request),
            (request_name,raw_request),
            (request_name,raw_request),
            ...
        ]

        """

    def handle(self):
        from frontends.gamespy.library.exceptions.general import UniSpyException

        try:
            self._process_raw_request()
            if len(self._requests) == 0:
                return
            for request in self._requests:
                handler = self._create_cmd_handlers(request[0], request[1])
                if handler is None:
                    self._client.log_warn(
                        f"Request: <{request[0]}> is ignored.")
                    continue
                self._handlers.append(handler)
            if len(self._handlers) == 0:
                return

            for handler in self._handlers:
                handler.handle()
        except Exception as e:
            UniSpyException.handle_exception(e, self._client)

    @abstractmethod
    def _process_raw_request(self) -> None:
        pass

    @abstractmethod
    def _create_cmd_handlers(self, name: object, raw_request: object) -> Optional[CmdHandlerBase]:
        pass
