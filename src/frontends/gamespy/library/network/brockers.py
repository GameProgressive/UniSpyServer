import threading
from typing import Callable
from uuid import UUID
from redis import Redis
from websockets import ConnectionClosed
from frontends.gamespy.library.abstractions.brocker import BrockerBase
from redis.client import PubSub

from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER
from frontends.gamespy.protocols.chat.abstractions.contract import BrockerMessage
from websockets.sync.client import connect, ClientConnection


class RedisBrocker(BrockerBase):
    _client: Redis
    _subscriber: PubSub

    def __init__(self, name: str, url: str, call_back_func: Callable) -> None:
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
                msg = message["data"].decode("utf-8")
                # run receive message in background do not block receiving
                threading.Thread(target=self.receive_message, args=msg)

    def unsubscribe(self):
        self.is_started = False
        self._subscriber.unsubscribe(self._name)
        self._subscriber.close()

    def publish_message(self, message):
        self._client.publish(self._name, message)


class WebSocketBrocker(BrockerBase):
    _subscriber: ClientConnection
    _publisher: ClientConnection

    def subscribe(self):
        self._publisher = self._subscriber = connect(self.url)
        th = threading.Thread(target=self._listen)
        th.start()

    def _listen(self):
        try:
            while True:
                message = self._subscriber.recv()
                self._call_back_func(message)
        except ConnectionClosed:
            GLOBAL_LOGGER.warn("backend websocket server is not avaliable")
            # raise UniSpyException("websocket connection is not established")

    def unsubscribe(self):
        self._subscriber.close()

    def publish_message(self, message: BrockerMessage):
        if self._publisher is None:
            raise ValueError("websocket connection is not established")
        self._publisher.send(message.model_dump_json())


if __name__ == "__main__":
    ws = WebSocketBrocker(
        name="test_channel",
        url="ws://127.0.0.1:8080/GameSpy/Chat/ws",
        call_back_func=print,
    )
    ws.subscribe()
    msg = BrockerMessage(
        server_id=UUID("08ed7859-1d9e-448b-8fda-dabb845d85f9"),
        channel_name="gmtest",
        sender_ip_address="192.168.0.1",
        sender_port=80,
        message="hello",
    )
    ws.publish_message(msg)
