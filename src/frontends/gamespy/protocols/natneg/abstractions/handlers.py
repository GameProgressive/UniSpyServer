from frontends.gamespy.protocols.natneg.applications.client import Client
from frontends.gamespy.protocols.natneg.abstractions.contracts import RequestBase
import frontends.gamespy.library.abstractions.handler as lib


class CmdHandlerBase(lib.CmdHandlerBase):
    def __init__(self, client: Client, request: RequestBase) -> None:
        super().__init__(client, request)
        assert isinstance(client, Client)
        assert issubclass(type(request), RequestBase)

    def handle(self) -> None:
        try:
            # we first log this class
            self._log_current_class()
            # then we handle it
            self._request_check()
            self._response_construct()
            # first send the response
            if self._response is not None:
                self._response_send()
            # then send to backends
            self._data_operate()
        except Exception as ex:
            self._handle_exception(ex)


if __name__ == "__main__":
    # cmd = CmdHandlerBase(None, None)
    pass
