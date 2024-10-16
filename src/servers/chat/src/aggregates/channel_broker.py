from library.src.network.brockers import WebsocketBrocker
import requests

from library.src.unispy_server_config import CONFIG
from servers.chat.src.applications.server_launcher import ServerLauncher
from servers.chat.src.exceptions.general import ChatException



class ChannelBrocker(WebsocketBrocker):
    def __init__(self, name: str, url: str, call_back_func: function) -> None:
        super().__init__(name, url, call_back_func)
        req = {"channel_name": name, "server_id": ServerLauncher.config.server_id,
               "server_ip": ServerLauncher.config.public_address}
        try:
            requests.post(
                f"{CONFIG.backend.url}/GameSpy/Chat/add_channel", req)
        except Exception as e:
            raise ChatException("Channel register on backend failed")
