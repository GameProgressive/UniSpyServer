DATA_SIZE = 2048
import abc


class Server(abc.ABC):

    @abc.abstractclassmethod
    def start(self):
        pass
