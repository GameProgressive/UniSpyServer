from frontends.gamespy.library.network.http_handler import HttpData
from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException


class SakeException(WebException):
    """
    sake exception will send error in http header
    """

    def build(self) -> None:
        self.sending_buffer = HttpData(
            body="", headers={
                "Error": self.message
            }
        )
