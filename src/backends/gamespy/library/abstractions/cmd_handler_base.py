import abc
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from library.abstractions.request_base import RequestBase


class CmdHandlerBase(abc.ABC):
    _request: RequestBase

    def handle(self):
        self.request_check()

    def request_check(self):
        pass
