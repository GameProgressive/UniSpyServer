from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase, ServerFactory
from frontends.gamespy.library.network.tcp_handler import TcpServer
from frontends.gamespy.protocols.chat.applications.client import Client


class ServerLauncher(ServerLauncherBase):

    def __init__(self) -> None:
        super().__init__(
            config_name="Chat",
            client_cls=Client,
            server_cls=TcpServer,
        )


if __name__ == "__main__":
    from frontends.gamespy.library.extentions.debug_helper import DebugHelper

    chat = ServerLauncher()
    helper = DebugHelper(
        "./frontends/gamespy/protocols/chat", ServerFactory([chat]))
    helper.start()
