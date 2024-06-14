import abc
from library.abstractions.handler import CmdHandlerBase as CHB
from servers.query_report.v2.abstractions.request_base import RequestBase
from servers.query_report.applications.client import Client


class CmdHandlerBase(CHB, abc.ABC):
    def __init__(self, client: Client, request: RequestBase) -> None:
        assert issubclass(type(request), RequestBase)
        assert isinstance(client, Client)
        super().__init__(client, request)
