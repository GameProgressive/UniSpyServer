from frontends.gamespy.library.abstractions.contracts import ResponseBase
from frontends.gamespy.library.exceptions.general import UniSpyException, get_exceptions_dict
from frontends.gamespy.library.network.http_handler import HttpData


class WebException(UniSpyException, ResponseBase):
    sending_buffer: HttpData

    def __init__(self, message: str) -> None:
        self.message = message


EXCEPTIONS = get_exceptions_dict(__name__)
