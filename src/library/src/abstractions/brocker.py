import abc

from servers.chat.src.aggregates.channel import MIN_CHANNEL_NAME_LENGTH
from servers.chat.src.exceptions.general import ChatException


class BrockerBase:
    _subscriber: object
    is_started: bool = False
    _name: str
    call_backs: list
    """
    brocker subscribe name
    """

    def __init__(self, name: str) -> None:
        assert isinstance(name, str)
        if len(name) < MIN_CHANNEL_NAME_LENGTH:
            raise ChatException(f"The channel name length must larget than {
                                MIN_CHANNEL_NAME_LENGTH}")
        self._name = name

    @abc.abstractmethod
    def subscribe(self):
        """
        define the brocker event binding
        """
        pass

    @abc.abstractmethod
    def receive_message(self, message):
        pass

    @abc.abstractmethod
    def publish_message(self, message):

        pass

    @abc.abstractmethod
    def unsubscribe(self):
        pass
