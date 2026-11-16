import abc
from collections.abc import Callable
from typing import final

from frontends.gamespy.library.log.log_manager import LogWriter


class BrockerBase:
    _subscriber: object
    is_started: bool = False
    _name: str
    _call_back_func: Callable | None
    _logger: LogWriter
    """
    brocker subscribe name
    """

    def __init__(self, name: str, url: str, logger: LogWriter, call_back_func: Callable | None) -> None:
        assert isinstance(name, str)

        self._name = name
        self.url = url
        self._logger = logger
        if call_back_func is not None:
            assert callable(call_back_func)
        self._call_back_func = call_back_func

    @abc.abstractmethod
    def subscribe(self):
        """
        define the brocker event binding
        """
        pass

    @final
    def receive_message(self, message: str):
        assert isinstance(message, str)
        if self._call_back_func is None:
            return
        self._call_back_func(message)

    @abc.abstractmethod
    def publish_message(self, message: str):
        assert isinstance(message, str)

    @abc.abstractmethod
    def unsubscribe(self):
        pass
