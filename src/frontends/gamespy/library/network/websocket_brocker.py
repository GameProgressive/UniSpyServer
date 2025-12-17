import threading
from uuid import UUID
from websockets import ConnectionClosed
from frontends.gamespy.library.abstractions.brocker import BrockerBase

from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER
from websockets.sync.client import connect, ClientConnection


class WebSocketBrocker(BrockerBase):
    _subscriber: ClientConnection
    _publisher: ClientConnection

    def subscribe(self):
        self._publisher = self._subscriber = connect(self.url)
        th = threading.Thread(target=self._listen)
        th.start()

    @property
    def ip_port(self) -> str:
        name = self._subscriber.socket.getsockname()
        return f"{name[0]}:{name[1]}"

    def _listen(self):
        # we do not listen to channel, if the call back is none
        if self._call_back_func is None:
            return

        try:
            while True:
                message = self._subscriber.recv()
                self._call_back_func(message)
        except ConnectionClosed:
            GLOBAL_LOGGER.warn("backend websocket server is not avaliable")
            # raise UniSpyException("websocket connection is not established")

    def unsubscribe(self):
        self._subscriber.close()

    def publish_message(self, message: str):
        super().publish_message(message)
        if self._publisher is None:
            raise ValueError("websocket connection is not established")
        self._publisher.send(message)


COUNT = 0


def call_back(str):
    global COUNT
    COUNT += 1
    print(COUNT)
    # print(f"{datetime.now()}:{str}")


if __name__ == "__main__":
    ws = WebSocketBrocker(
        name="test_channel",
        url="ws://127.0.0.1:8080/GameSpy/Chat/ws",
        call_back_func=call_back,
    )
    ws.subscribe()
    from frontends.gamespy.protocols.chat.abstractions.contract import BrockerMessage

    msg = BrockerMessage(
        server_id=UUID("08ed7859-1d9e-448b-8fda-dabb845d85f9"),
        channel_name="gmtest",
        sender_ip_address="192.168.0.1",
        sender_port=80,
        message="hello",
    )
    # while True:
    while True:
        ws.publish_message(msg.model_dump_json())
        pass
