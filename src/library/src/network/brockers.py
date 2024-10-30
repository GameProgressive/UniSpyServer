import asyncio
import threading
from typing import Optional
import websocket
from redis import Redis
from library.src.abstractions.brocker import BrockerBase
from redis.client import PubSub

from servers.chat.src.aggregates.exceptions import ChatException
websocket.enableTrace(True)


class RedisBrocker(BrockerBase):
    _client: Redis
    _subscriber: PubSub

    def __init__(self, name: str, url: str, call_back_func: "function") -> None:
        super().__init__(name, url, call_back_func)
        self._client = Redis.from_url(url)
        self._subscriber = self._client.pubsub()

    def subscribe(self):
        self.is_started = True
        self._subscriber.subscribe(self._name)
        threading.Thread(target=self.get_message).start()

    def get_message(self):
        for message in self._subscriber.listen():
            if not self.is_started:
                break
            if message["type"] == "message":
                msg = message['data'].decode('utf-8')
                # run receive message in background do not block receiving
                threading.Thread(target=self.receive_message, args=msg)

    def unsubscribe(self):
        self.is_started = False
        self._subscriber.unsubscribe(self._name)
        self._subscriber.close()

    def publish_message(self, message):
        self._client.publish(self._name, message)


class WebsocketBrocker(BrockerBase):
    _subscriber: websocket.WebSocketApp
    _publisher: Optional[websocket.WebSocket] = None

    def __init__(self, name: str, url: str, call_back_func: "function") -> None:
        super().__init__(name, url, call_back_func)
        self._subscriber = websocket.WebSocketApp(
            url,
            on_message=lambda _, m: self.receive_message(m),
            on_error=print,
            on_close=print)
        self._subscriber.on_open = self._on_open

    def _on_open(self, ws):
        self._publisher = ws

    def _on_message(self, _, message):
        threading.Thread(target=self.receive_message, args=message).start()

    def subscribe(self):
        threading.Thread(target=self._subscriber.run_forever).start()
        # # wait for connection establish
        if self._publisher is None:
            raise ChatException("brocker backend is not available")

    def unsubscribe(self):
        self._subscriber.close()

    def publish_message(self, message):
        if self._publisher is None:
            raise ValueError("websocket connection is not established")
        self._publisher.send(message)


if __name__ == "__main__":

    ws = WebsocketBrocker(name="test_channel",
                          url="ws://127.0.0.1:8080/GameSpy/Chat/Channel", call_back_func=print)
    ws.subscribe()
    import json
    ws.publish_message(json.dumps(
        {"channel_name": "test", "message": "hello"}))
    while True:
        pass
