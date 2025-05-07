import abc
from typing import final, Callable


class BrockerBase:
    _subscriber: object
    is_started: bool = False
    _name: str
    _call_back_func: Callable
    """
    brocker subscribe name
    """

    def __init__(self, name: str, url: str, call_back_func: Callable) -> None:
        assert isinstance(name, str)
        assert callable(call_back_func)

        self._name = name
        self._call_back_func = call_back_func
        self.url = url

    @abc.abstractmethod
    def subscribe(self):
        """
        define the brocker event binding
        """
        pass

    @final
    def receive_message(self, message):
        self._call_back_func(message)
        pass

    @abc.abstractmethod
    def publish_message(self, message):
        pass

    @abc.abstractmethod
    def unsubscribe(self):
        pass
