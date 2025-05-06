import asyncio
from uuid import UUID
from fastapi import WebSocket


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
    def ip_port(self):
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

    def get_client(self, ws: WebSocket) -> WebSocketClient | None:
        ip_port = WebSocketClient.get_ip_port_str(ws)
        client = None
        if ip_port in self.client_pool:
            client = self.client_pool[ip_port]
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

    def broadcast(self, message: dict):
        loop = asyncio.get_event_loop()
        for client in self.client_pool.values():
            loop.create_task(client.ws.send_json(message))
