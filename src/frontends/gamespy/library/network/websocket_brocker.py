import threading
from uuid import UUID

from pydantic import BaseModel
from websockets import ConnectionClosed
from websockets.sync.client import ClientConnection, connect

from frontends.gamespy.library.abstractions.brocker import BrockerBase
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER


class WebsocketMessage(BaseModel):
    """
    The websocket message from backends
    """
    sender_server_id: UUID
    message: str


class WebSocketListener(BrockerBase):
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
        if self._call_back_func is None:
            return
        while True:
            try:
                message = self._subscriber.recv()
                self._call_back_func(message)
            except ConnectionClosed:
                self._logger.warn("websocket disconnected")
                break

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
    ws = WebSocketListener(
        name="test_channel",
        url="ws://127.0.0.1:8080/GameSpy/Chat/ws",
        logger=GLOBAL_LOGGER,
        call_back_func=call_back,
    )
    ws.subscribe()

    msg = WebsocketMessage(
        sender_server_id=UUID("08ed7859-1d9e-448b-8fda-dabb845d85f9"),
        message="hello",
    )
    # while True:
    while True:
        ws.publish_message(msg.model_dump_json())
        pass
