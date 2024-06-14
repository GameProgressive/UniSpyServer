import servers.webservices.abstractions.contracts as lib
from servers.webservices.aggregations.soap_envelop import SoapEnvelop

NAMESPACE = "http://gamespy.net/commerce/"


class RequestBase(lib.RequestBase):
    pass


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _content: SoapEnvelop = SoapEnvelop(NAMESPACE)
    pass
