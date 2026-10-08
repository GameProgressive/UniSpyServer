DATA_SIZE = 2048
import abc


class Server(abc.ABC):

    @abc.abstractmethod
    def start(self):
        pass
