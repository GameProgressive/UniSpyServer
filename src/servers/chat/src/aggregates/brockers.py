import threading
import redis
from library.src.abstractions.brocker import BrockerBase
from library.src.unispy_server_config import CONFIG


class SocketIOBrocker(BrockerBase):
    pass


class RedisBrocker(BrockerBase):
    _subscriber: redis.client.PubSub

    def __init__(self, name: str) -> None:
        super().__init__(name)
        self.__redis = redis.from_url(CONFIG.redis.url)
        self._subscriber = self.__redis.pubsub()
        self.sub_thread = threading.Thread(target=self._subscribe)
        self.sub_thread.daemon = True

    def _subscribe(self):
        self._subscriber.subscribe(self._name)
        for message in self._subscriber.listen():
            if message["type"] == "message":
                print(message["data"])

    def publish_message(self, message):
        self.__redis.publish(self._name, message)

    def start(self):
        self._subscribe()
        # self.sub_thread.start()


if __name__ == "__main__":
    b = RedisBrocker("hello")
    b.start()
