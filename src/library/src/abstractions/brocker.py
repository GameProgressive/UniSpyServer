import abc


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
        self._name = name

    @abc.abstractmethod
    def _subscribe(self):
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

    def __del__(self):
        self.unsubscribe()
