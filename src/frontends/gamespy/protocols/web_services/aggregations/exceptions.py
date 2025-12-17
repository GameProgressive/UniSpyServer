from frontends.gamespy.library.abstractions.contracts import ResponseBase
from frontends.gamespy.library.exceptions.general import UniSpyException, get_exceptions_dict


class WebException(UniSpyException, ResponseBase):
    def __init__(self, message: str) -> None:
        self.message = message



EXCEPTIONS = get_exceptions_dict(__name__)
