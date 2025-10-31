from frontends.gamespy.library.abstractions.server_launcher import ServerFactory, ServerLauncherBase
from frontends.gamespy.library.network.tcp_handler import TcpServer
from frontends.gamespy.protocols.presence_connection_manager.applications.client import (
    Client,
)


class ServerLauncher(ServerLauncherBase):
    def __init__(self) -> None:
        super().__init__(
            config_name="PresenceConnectionManager",
            client_cls=Client,
            server_cls=TcpServer,
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper
    pcm = ServerLauncher()
    helper = DebugHelper("./frontends/", ServerFactory([pcm]))
    helper.start()
