import frontends.gamespy.protocols.natneg.applications.client as natneg


class ClientInfo:
    cookie: int
    pass


class ConnectionListener:
    pool: dict[int, list["Client"]] = {}

    def is_client_exist(self, cookie: int, client: "Client"):
        if cookie not in self.pool:
            return False
        else:
            clients = self.pool[cookie]
            # if current client is not in the pair
            if client not in clients:
                return False

    def add_client(self, cookie: int, client: "Client"):
        if cookie not in self.pool:
            self.pool[cookie] = [client]
        else:
            clients = self.pool[cookie]
            # if current clients is in the pair
            if len(clients) == 2 and client in clients:
                return
            # if current client is not in the pair
            if len(clients) == 1 and client not in clients:
                clients = (clients[0], client)

    def get_another_client(self, cookie: int, client: "Client"):
        if cookie not in self.pool:
            raise ValueError("cookie not add to pool")
        else:
            clients = self.pool[cookie]
            # if current client is not in the pair
            if client not in clients:
                raise ValueError("client not in cookie")
            if len(clients) != 2:
                raise ValueError("2 clients are not ready")
            return clients[0] if clients[1] == client else clients[1]

    def is_both_client_ready(self, cookie: int, client: "Client") -> bool:
        clients = self.pool[cookie]
        if len(clients) != 2:
            return False
        elif self.is_client_exist(cookie, client):
            return True
        else:
            return False


class Client(natneg.Client):
    info: ClientInfo
    client_pool: dict[str, "Client"] = {}
    listener: ConnectionListener = ConnectionListener()

    def on_received(self, buffer: bytes) -> None:
        """
        when we receive udp message, we check whether the client pair is ready.
        if client is ready we send the following data to the another client
        """
        if self.listener.is_both_client_ready(self.info.cookie, self):
            another_client = self.listener.get_another_client(self.info.cookie, self)
            another_client.connection.send(buffer)
            self.log_info(
                f"[{self.connection.ip_endpoint}] => [{another_client.connection.ip_endpoint}] {buffer}"
            )
        else:
            super().on_received(buffer)
