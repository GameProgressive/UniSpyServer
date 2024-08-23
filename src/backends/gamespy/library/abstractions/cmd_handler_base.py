import abc
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from library.src.abstractions.contracts import RequestBase


class CmdHandlerBase:
    _request: RequestBase

    def handle(self):
        self.request_check()

    def request_check(self):
        pass
