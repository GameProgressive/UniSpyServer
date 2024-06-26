import servers.webservices.abstractions.contracts as lib
from servers.webservices.aggregations.soap_envelop import SoapEnvelop
from servers.webservices.modules.sake.exceptions.general import SakeException

NAMESPACE = "http://gamespy.net/sake"


class RequestBase(lib.RequestBase):
    game_id: int
    secret_key: str
    login_ticket: str
    table_id: str

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None:
            raise SakeException("gameid is missing from the request.")
        self.game_id = int(game_id)

        self.secret_key = self._content_element.find(f".//{{{NAMESPACE}}}secretKey")
        if self.secret_key is None:
            raise SakeException("secretkey id is missing from the request.")

        self.login_ticket = self._content_element.find(f".//{{{NAMESPACE}}}loginTicket")
        if self.login_ticket is None:
            raise SakeException("loginTicket is missing from the request.")

        self.table_id = self._content_element.find(f".//{{{NAMESPACE}}}tableid")
        if self.table_id is None:
            raise SakeException("tableid is missing from the request.")


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    def __init__(self, request: RequestBase, result: ResultBase) -> None:
        self._content = SoapEnvelop(NAMESPACE)
        super().__init__(request, result)