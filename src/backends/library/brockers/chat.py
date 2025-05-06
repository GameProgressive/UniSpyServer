from backends.library.abstractions.websocket_manager import WebSocketClient, WebSocketManager

# from library.network.brockers import RedisBrocker
from fastapi import WebSocket

# init in fast api routers
class ChatWebSocketClient(WebSocketClient):
    channels: list[str]

    def __init__(self, ws: WebSocket) -> None:
        super().__init__(ws)
        self.channels = []


class ChatWebSocketManager(WebSocketManager):
    channel_info: dict[str, dict[str, ChatWebSocketClient]]
    """
    [channel_name,[ip_port,WebSocket]]
    """
    client_pool: dict[str, ChatWebSocketClient]
    """
    [ip_port,ChatWebSocketClient]
    """

    def __init__(self) -> None:
        super().__init__()
        self.channel_info = {}

    def create_client(self, ws: WebSocket) -> WebSocketClient:
        client = ChatWebSocketClient(ws)
        return client

    def add_to_channel(self, channel_name: str, ws: WebSocket):
        client = self.get_client(ws)
        assert isinstance(client, ChatWebSocketClient)
        # add channel to client.channels
        if channel_name not in client.channels:
            client.channels.append(channel_name)
        # add client to channel_info
        if channel_name not in self.channel_info:
            self.channel_info[channel_name] = {}
        channel = self.channel_info[channel_name]
        channel[client.ip_port] = client

    def remove_from_channel(self, channel_name, ws: WebSocket):
        # remove from channel_info
        if channel_name not in self.channel_info:
            return
        client = self.get_client(ws)
        assert isinstance(client, ChatWebSocketClient)
        # remove from client.channels
        if channel_name not in client.channels:
            return
        client.channels.remove(channel_name)

    def disconnect(self, ws: WebSocket):
        client = self.get_client(ws)
        assert isinstance(client, ChatWebSocketClient)
        for channel_name in client.channels:
            if channel_name in self.channel_info:
                channel_dict = self.channel_info[channel_name]
                if client.ip_port in channel_dict:
                    del channel_dict[client.ip_port]
        super().disconnect(ws)

    def channel_broad_cast(self):
        pass


MANAGER = ChatWebSocketManager()

# create redis pubsub to share message cross all backends
# currently we simply implement without redis pubsub
