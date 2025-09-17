import asyncio
from logging import getLogger
from uuid import UUID
from fastapi import WebSocket

from backends.protocols.gamespy.chat.requests import RequestBase
from frontends.gamespy.library.exceptions.general import UniSpyException


class WebSocketClient:
    server_id: UUID
    ip: str
    port: int
    ws: WebSocket

    def __init__(self, ws: WebSocket) -> None:
        assert ws.client
        ip, port = ws.client
        self.ip = ip
        self.port = port
        self.ws = ws

    @property
    def ip_port(self) -> str:
        return f"{self.ip}:{self.port}"

    @staticmethod
    def get_ip_port_str(ws: WebSocket):
        assert ws.client
        ip, port = ws.client
        return f"{ip}:{port}"


class WebSocketManager:
    client_pool: dict[str, WebSocketClient]

    def __init__(self) -> None:
        self.client_pool = {}

    def create_client(self, ws: WebSocket) -> WebSocketClient:
        client = WebSocketClient(ws)
        return client

    def get_client(self, ws: WebSocket) -> WebSocketClient:
        ip_port = WebSocketClient.get_ip_port_str(ws)
        client = None
        if ip_port in self.client_pool:
            client = self.client_pool[ip_port]
        if client is None:
            raise UniSpyException(
                "client is not existed in client pool. skip deleting."
            )
        return client

    def connect(self, ws: WebSocket):
        client = self.create_client(ws)
        if client.ip_port not in self.client_pool:
            self.client_pool[client.ip_port] = client

    def disconnect(self, ws: WebSocket):
        """
        call at last in inherited classs
        """
        temp = self.create_client(ws)
        if temp.ip_port in self.client_pool:
            del self.client_pool[temp.ip_port]
            # todo remove record in database

            # todo check channelcache,usercache,channelusercache

    def broadcast(self, message: RequestBase, ws: list[str] | None = None):
        try:
            loop = asyncio.get_running_loop()
        except RuntimeError:  # No event loop is running
            loop = asyncio.new_event_loop()
            asyncio.set_event_loop(loop)
        if ws is None:
            clients = self.client_pool.values()
        else:
            clients = []
            for w in ws:
                if w in self.client_pool:
                    clients.append(self.client_pool[w])
                else:
                    logger = getLogger("backend")
                    logger.info(f"{ws} not in websocket client list")

        for client in clients:
            loop.create_task(client.ws.send_json(message.model_dump()))
