from frontends.gamespy.library.abstractions.contracts import ResponseBase
from frontends.gamespy.library.exceptions.general import (
    UniSpyException,
    get_exceptions_dict,
)
from frontends.gamespy.library.network.http_handler import HttpData
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import (
    SoapEnvelop,
)


class WebException(UniSpyException, ResponseBase):
    sending_buffer: str
    _content: SoapEnvelop
    _http_data: HttpData

    def __init__(self, message: str) -> None:
        super().__init__(message)


EXCEPTIONS = get_exceptions_dict(__name__)
