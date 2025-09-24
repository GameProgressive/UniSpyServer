import frontends.gamespy.protocols.web_services.abstractions.handler as h
import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
from frontends.gamespy.protocols.web_services.modules.sake.exceptions.general import SakeException
import xml.etree.ElementTree as ET

NAMESPACE = "http://gamespy.net/sake"


class RequestBase(lib.RequestBase):
    game_id: int
    secret_key: str
    login_ticket: str
    table_id: str

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise SakeException("gameid is missing from the request.")
        self.game_id = int(game_id.text)

        secret_key = self._content_element.find(
            f".//{{{NAMESPACE}}}secretKey")
        if secret_key is None or secret_key.text is None:
            raise SakeException("secretkey id is missing from the request.")
        self.secret_key = secret_key.text

        login_ticket = self._content_element.find(
            f".//{{{NAMESPACE}}}loginTicket")
        if login_ticket is None or login_ticket.text is None:
            raise SakeException("loginTicket is missing from the request.")
        self.login_ticket = login_ticket.text

        table_id = self._content_element.find(
            f".//{{{NAMESPACE}}}tableid")
        if table_id is None or table_id.text is None:
            raise SakeException("tableid is missing from the request.")
        self.table_id = table_id.text

    @staticmethod
    def remove_namespace(tree: ET.Element):
        tree.tag = tree.tag.split('}', 1)[-1]
        for elem in tree:
            # Remove the namespace by splitting the tag
            # Keep the part after the '}'
            elem.tag = elem.tag.split('}', 1)[-1]
        return tree


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    def __init__(self, result: ResultBase) -> None:
        self._content = SoapEnvelop(NAMESPACE)
        super().__init__(result)


class CmdHandlerBase(h.CmdHandlerBase):
    _request: "RequestBase"
    _result: "ResultBase"
