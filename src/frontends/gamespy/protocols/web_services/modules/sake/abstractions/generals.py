import frontends.gamespy.protocols.web_services.abstractions.handler as h
import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.modules.sake.exceptions.general import SakeException

NAMESPACE = "http://gamespy.net/sake"


class RequestBase(lib.RequestBase):
    game_id: int
    secret_key: str | None
    login_ticket: str | None
    table_id: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.secret_key = None
        self.login_ticket = None

    def parse(self) -> None:
        super().parse()
        self.game_id = self._get_int("gameid")

        secret_key = self._get_value_by_key("secretKey")
        if secret_key is not None:
            self.secret_key = self._get_str("secretKey")

        login_ticket = self._get_value_by_key("loginTicket")
        if login_ticket is not None:
            self.login_ticket = self._get_str("loginTicket")

        self.table_id = self._get_str("tableid")

    def _get_str(self, attr_name: str) -> str:
        try:
            value = super()._get_str(attr_name)
            return value
        except:
            raise SakeException(f"{attr_name} is missing from the request.")

    def _get_int(self, attr_name: str) -> int:
        value = self._get_str(attr_name)
        result = int(value)
        return result

    
class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    def __init__(self, result: ResultBase) -> None:
        super().__init__(result)


class CmdHandlerBase(h.CmdHandlerBase):
    _request: "RequestBase"
    _result: "ResultBase"
