from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop


class SakeException(WebException):
    """
    sake exception will send error in http header
    """

    def build(self) -> None:
        soap = SoapEnvelop("Sake")
        soap.add("Error", self.message)
        raise NotImplementedError()
