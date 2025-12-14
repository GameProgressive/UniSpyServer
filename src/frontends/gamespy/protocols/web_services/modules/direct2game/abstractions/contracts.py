import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop

NAMESPACE = "http://gamespy.net/commerce/"


class RequestBase(lib.RequestBase):
    pass


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _content: SoapEnvelop