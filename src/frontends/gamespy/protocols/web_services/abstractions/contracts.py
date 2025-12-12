import frontends.gamespy.library.abstractions.contracts as lib
import xml.etree.ElementTree as ET

from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop


class RequestBase(lib.RequestBase):
    raw_request: str
    _content_element: ET.Element

    def __init__(self, raw_request: str) -> None:
        assert isinstance(raw_request, str)
        super().__init__(raw_request)

    def parse(self) -> None:
        xelements = ET.fromstring(self.raw_request)
        self._content_element = xelements[0][0]


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _content: SoapEnvelop
    """
    Soap envelope content, should be initialized in response sub class
    """
    sending_buffer: str

    def __init__(self, result: ResultBase) -> None:
        assert issubclass(type(result), ResultBase)
        super().__init__(result)

    def build(self) -> None:
        self.sending_buffer = str(self._content)
