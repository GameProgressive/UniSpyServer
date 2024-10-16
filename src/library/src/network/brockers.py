import threading
import websocket
from redis import Redis
from library.src.abstractions.brocker import BrockerBase
from redis.client import PubSub
websocket.enableTrace(True)


class RedisBrocker(BrockerBase):
    _client: Redis
    _subscriber: PubSub

    def __init__(self, name: str, call_back_func: "function") -> None:
        super().__init__(name, call_back_func)
        self._subscriber = self._client.pubsub()

    def subscribe(self):
        self.is_started = True
        self._subscriber.subscribe(self._name)
        threading.Thread(target=self.get_message).start()

    def get_message(self):
        for message in self._subscriber.get_message():
            if not self.is_started:
                break
            if message["type"] == "message":
                self.receive_message(message['data'].decode('utf-8'))

    def unsubscribe(self):
        self.is_started = False
        self._subscriber.unsubscribe(self._name)
        self._subscriber.close()

    def publish_message(self, message):
        self._client.publish(self._name, message)


class WebsocketBrocker(BrockerBase):
    _publisher: websocket.WebSocket = None
    _subscriber: websocket.WebSocketApp

    def __init__(self, name: str, url: str, call_back_func: "function") -> None:
        super().__init__(name, call_back_func)
        self._subscriber = websocket.WebSocketApp(
            url,
            on_message=lambda _, m: self.receive_message(m),
            on_error=print,
            on_close=print,
            on_open=self._on_open)

    def _on_open(self, ws):
        self._publisher: websocket.WebSocket = ws

    def _on_message(self, _, message):
        self.receive_message(message)

    def subscribe(self):
        threading.Thread(target=self._subscriber.run_forever).start()
        # wait for connection establish
        while self._publisher is not None:
            break

    def unsubscribe(self):
        self._subscriber.close()

    def publish_message(self, message):
        self._publisher.send(message)


if __name__ == "__main__":

    ws = WebsocketBrocker(name="test_channel",
                          url="ws://127.0.0.1:8000/channel", call_back_func=print)
    ws.subscribe()
    ws.publish_message("hello")
    while True:
        pass
