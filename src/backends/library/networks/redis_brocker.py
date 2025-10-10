
import threading
from redis.client import PubSub
from typing import Callable
from redis import Redis

from frontends.gamespy.library.abstractions.brocker import BrockerBase
from frontends.gamespy.library.configs import CONFIG


class RedisBrocker(BrockerBase):
    _client: Redis
    _subscriber: PubSub

    def __init__(self, name: str, url: str, call_back_func: Callable | None = None) -> None:
        super().__init__(name, url, call_back_func)
        self._client = Redis.from_url(url, socket_timeout=5)
        self._client.ping()
        self._subscriber = self._client.pubsub()

    def subscribe(self):
        self.is_started = True
        threading.Thread(target=self.get_message).start()

    def get_message(self):
        self._subscriber.subscribe(self._name)
        while True:
            m = self._subscriber.get_message(timeout=60)
            if m is not None:
                if "data" not in m:
                    continue
                if not isinstance(m['data'], bytes):
                    continue
                msg = m['data'].decode("utf-8")
                threading.Thread(target=self.receive_message, args=[msg]).start()

    def unsubscribe(self):
        self.is_started = False
        self._subscriber.unsubscribe(self._name)
        self._subscriber.close()

    def publish_message(self, message: str):
        assert isinstance(message, str)
        self._client.publish(self._name, message)


if __name__ == "__main__":
    pass

    brocker = RedisBrocker("master", CONFIG.redis.url)
    brocker.subscribe()
    pass
