import asyncio
from fastapi import WebSocket


class WebsocketManager:
    ws_pool: dict[str, WebSocket]

    def __init__(self) -> None:
        self.ws_pool = {}

    @staticmethod
    def _get_ip_port_str(ws: WebSocket):
        ip, port = ws.client  # type:ignore
        ip_port = f"{ip}:{port}"
        return ip_port

    def add_websocket(self, ws: WebSocket):
        assert ws
        WebsocketManager._add_websocket_to_dict(self.ws_pool, ws)

    def remove_websocket(self, ws: WebSocket):
        assert ws
        WebsocketManager._remove_websocket_from_dict(self.ws_pool, ws)

    @staticmethod
    def _add_websocket_to_dict(pool: dict, ws: WebSocket):
        ip_port = WebsocketManager._get_ip_port_str(ws)
        if ip_port not in pool:
            pool[ip_port] = ws
        else:
            print("client already in dict, so not add.")

    @staticmethod
    def _remove_websocket_from_dict(pool: dict, ws: WebSocket):
        ip_port = WebsocketManager._get_ip_port_str(ws)
        if ip_port in pool:
            del pool[ip_port]
        else:
            print("client not found in dict, so not removed.")

    def broadcast(self, message: dict):
        WebsocketManager._broadcast(self.ws_pool, message)

    @staticmethod
    def _broadcast(pool: dict[str, WebSocket], message: dict):
        loop = asyncio.get_event_loop()
        for ws in pool.values():
            loop.create_task(ws.send(message))


class ChatWebSocketManager(WebsocketManager):
    irc_channels: dict[str, dict[str, WebSocket]]

    def __init__(self) -> None:
        super().__init__()
        self.irc_channels = {}

    def add_client_to_channel(self, channel_name: str, ws: WebSocket):
        if channel_name not in self.irc_channels:
            self.irc_channels[channel_name] = {}

        channel_ws_pool = self.irc_channels[channel_name]
        WebsocketManager._add_websocket_to_dict(channel_ws_pool, ws)

    def remove_client_from_channel(self, channel_name: str, ws: WebSocket):
        if channel_name not in self.irc_channels:
            return
        channel_ws_pool = self.irc_channels[channel_name]
        WebsocketManager._remove_websocket_from_dict(channel_ws_pool, ws)

    def broadcast_channel(self, name: str, message: dict):
        if name not in self.irc_channels:
            return
        channel_pool = self.irc_channels[name]
        WebsocketManager._broadcast(channel_pool, message)
