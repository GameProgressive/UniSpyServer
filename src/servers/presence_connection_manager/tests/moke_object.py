import unittest
from unittest.mock import Mock
from servers.presence_connection_manager.applications.client import Client

class MokeObject:
    client = None

    # TODO We first need Server implementation
    @staticmethod
    def create_client(ip_address="192.168.1.1", port=9990):
        manager_mock = Mock(IConnectionManager)
        connection_mock = Mock()
        connection_mock.RemoteIPEndPoint = (ip_address, port)
        connection_mock.Manager = manager_mock
        connection_mock.ConnectionType = "Tcp"
        server_mock = Server(manager_mock)

        return Client(connection_mock, server_mock)
