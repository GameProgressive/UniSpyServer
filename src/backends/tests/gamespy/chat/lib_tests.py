from typing import cast
import unittest

from fastapi import WebSocket
from fastapi.datastructures import Address

from backends.protocols.gamespy.chat.brocker_manager import (
    ChatWebSocketClient,
    ChatWebSocketManager,
)


class WebSocketMock:
    client: Address = Address("127.0.0.1", 123)


class LibTests(unittest.TestCase):
    def test_ws_manager(self):
        ws = WebSocketMock()
        manager = ChatWebSocketManager()
        ws = cast(WebSocket, ws)
        manager.connect(ws)
        self.assertEqual(list(manager.client_pool.values())[0].ws, ws)
        manager.disconnect(ws)
        self.assertEqual(len(manager.client_pool.values()), 0)

    def test_chat_ws_manager(self):
        ws = WebSocketMock()
        manager = ChatWebSocketManager()
        ws = cast(WebSocket, ws)
        manager.connect(ws)
        self.assertEqual(list(manager.client_pool.values())[0].ws, ws)

        channel_name = "gmtest"
        manager.add_to_channel(channel_name, ws)
        client = manager.get_client(ws)
        client = cast(ChatWebSocketClient, client)
        manager.channel_info
        self.assertTrue(channel_name in manager.channel_info)
        self.assertTrue(len(manager.channel_info.values()) != 0)
        self.assertTrue(channel_name in client.channels)
        manager.remove_from_channel(channel_name, ws)
        self.assertTrue(channel_name not in manager.channel_info)
        self.assertTrue(len(manager.channel_info.values()) == 0)
        self.assertTrue(channel_name not in client.channels)

        manager.disconnect(ws)
        self.assertEqual(len(manager.client_pool.values()), 0)
