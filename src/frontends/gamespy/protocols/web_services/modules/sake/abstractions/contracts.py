from frontends.gamespy.library.network.http_handler import HttpData
import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import SakePlatform
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.exceptions import SakeException

NAMESPACE = "http://gamespy.net/sake"


class RequestBase(lib.RequestBase):
    game_id: int
    table_id: str
    secret_key: str | None
    login_ticket: str
    platform: SakePlatform
    """
    c sdk require gp to get login_ticket
    dotnet sdk require auth service to get session token
    """

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.secret_key = None
        self.platform = SakePlatform.Windows

    def parse(self) -> None:
        super().parse()
        self.game_id = self._get_int("gameid")

        secret_key = self._get_value_by_key("secretKey")
        if secret_key is not None:
            self.secret_key = self._get_str("secretKey")

        login_ticket = self._get_value_by_key("loginTicket")
        if login_ticket is not None:
            self.login_ticket = self._get_str("loginTicket")
        else:
            if self.raw_request.headers is None:
                raise SakeException("headers is missing in c# version gamespy")
            if "SessionToken" not in self.raw_request.headers:
                raise SakeException("session token is missing")
            self.login_ticket = self.raw_request.headers["SessionToken"]
        self.table_id = self._get_str("tableid")

    def parse_headers(self):
        """
        parse headers from http request
        """
        # todo check profileid is same in xml body
        if self.raw_request.headers is not None:
            # if "GameID" not in self.raw_request.headers:
            #     raise SakeException("game id is missing")
            # self.game_id = int(self.raw_request.headers["GameID"])

            # if "ProfileID" not in self.raw_request.headers:
            #     raise SakeException("profile id is missing")
            # self.profile_id = int(self.raw_request.headers["ProfileID"])
            if "SessionToken" not in self.raw_request.headers:
                raise SakeException("session token is missing")
            self.login_ticket = self.raw_request.headers["SessionToken"]

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
    login_ticket: str
    pass


class ResponseBase(lib.ResponseBase):
    _result: ResultBase

    def __init__(self, result: ResultBase) -> None:
        super().__init__(result)

    def build(self) -> None:
        """
        in c# sdk session token is like login ticket 
        """
        self.sending_buffer = HttpData(str(self._content), headers={
            "SessionToken": self._result.login_ticket
        })
