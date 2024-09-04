import servers.web_services.src.abstractions.contracts as lib
from servers.web_services.src.aggregations.soap_envelop import SoapEnvelop

NAMESPACE = "http://gamespy.net/commerce/"


class RequestBase(lib.RequestBase):
    pass


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _content: SoapEnvelop = SoapEnvelop(NAMESPACE)
    pass
