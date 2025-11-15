from enum import Enum
from typing import TYPE_CHECKING

from frontends.gamespy.library.exceptions.general import UniSpyException

if TYPE_CHECKING:
    from frontends.gamespy.protocols.game_traffic_relay.applications.client import Client


class ConnectStatus(Enum):
    WAITING_FOR_ANOTHER = 0
    CONNECTING = 1
    FINISHED = 2


class ConnectionListener:
    """
    manage and monitor the connections
    """
    cookie_pool: dict[int, list["Client"]] = {}
    client_pool: dict[str, "Client"] = {}

    @staticmethod
    def get_client_by_ip(ip_end: str) -> "Client|None":
        if ip_end in ConnectionListener.client_pool:
            return ConnectionListener.client_pool[ip_end]
        else:
            return None

    @staticmethod
    def is_client_exist(cookie: int, client: "Client"):
        if cookie not in ConnectionListener.cookie_pool:
            return False
        else:
            clients = ConnectionListener.cookie_pool[cookie]
            # if current client is not in the pair
            if client not in clients:
                return False
            else:
                return True

    @staticmethod
    def add_client(cookie: int, client: "Client"):
        # add client to client pool
        if client.connection.ip_endpoint not in ConnectionListener.client_pool:
            ConnectionListener.client_pool[client.connection.ip_endpoint] = client
        # then add to cookie pool
        if cookie not in ConnectionListener.cookie_pool:
            ConnectionListener.cookie_pool[cookie] = [client]
        else:
            clients = ConnectionListener.cookie_pool[cookie]
            ConnectionListener.check_whether_accept_new_connection(
                cookie, client, clients)

    @staticmethod
    def get_another_client(cookie: int, client: "Client"):
        if cookie not in ConnectionListener.cookie_pool:
            raise ValueError("cookie not add to pool")
        else:
            clients = ConnectionListener.cookie_pool[cookie]
            # if current client is not in the pair
            if client not in clients:
                raise ValueError("client not in cookie")
            if len(clients) != 2:
                raise ValueError("2 clients are not ready")
            return clients[0] if clients[1] == client else clients[1]

    @staticmethod
    def is_both_client_ready(cookie: int) -> bool:
        clients = ConnectionListener.cookie_pool[cookie]
        if len(clients) != 2:
            return False
        else:
            return True

    @staticmethod
    def check_whether_accept_new_connection(cookie: int, client: "Client", clients: list["Client"]):
        # if current clients is in the pair
        if len(clients) == 2:
            if client in clients:
                return
            else:
                raise UniSpyException(
                    f"cookie: {cookie} is alive, you can not neogotiate with exist connections")

        # if current client is not in the pair
        if len(clients) == 1 and client not in clients:
            clients.append(client)
