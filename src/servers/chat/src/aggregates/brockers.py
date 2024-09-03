# import threading
import redis
import redis.client
from library.src.abstractions.brocker import BrockerBase
from library.src.unispy_server_config import CONFIG
import websocket


class WebSocketBrocker(BrockerBase):
    _subscriber: websocket.WebSocketApp

    def __init__(self, name: str) -> None:
        super().__init__(name)
        url = f"{CONFIG.backend.url}/{name}"
        self._subscriber = \
            websocket.WebSocketApp(self._backend_url,
                                   on_message=self.receive_message,
                                   on_error=None, on_close=None)

    def subscribe(self):
        if not self.is_started:
            self._subscriber.run_forever(reconnect=5)

    def receive_message(self, message):
        return super().receive_message(message)

    def unsubscribe(self):
        self._subscriber.close()


class RedisBrocker(BrockerBase):
    _subscriber: redis.client.PubSub

    def __init__(self, name: str) -> None:
        super().__init__(name)
        self._redis = redis.from_url(CONFIG.redis.url)
        self._subscriber = self._redis.pubsub()

    def subscribe(self):
        self._subscriber.subscribe(self._name)
        for message in self._subscriber.listen():
            if message["type"] == "message":
                print(message["data"])

    def publish_message(self, message):
        self._redis.publish(self._name, message)

    def unsubscribe(self):
        self._subscriber.unsubscribe()


if __name__ == "__main__":
    b = RedisBrocker("hello")
    b.subscribe()
