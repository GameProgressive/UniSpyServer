from frontends.gamespy.library.network.http_handler import HttpData
import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import (
    CommandName,
    SakePlatform,
)
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.exceptions import (
    SakeException,
)
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.utils import RecordConverter

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
    command_name: CommandName

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.secret_key = None
        self.platform = SakePlatform.Windows

    def parse(self) -> None:
        super().parse()
        self.command_name = CommandName(self.command_name)
        self.game_id = self._get_int("gameid")

        secret_key = self._get_value_by_key("secretKey")
        if secret_key is not None:
            self.secret_key = self._get_str("secretKey")

        login_ticket = self._get_value_by_key("loginTicket")
        if login_ticket is not None:
            self.login_ticket = self._get_str("loginTicket")
        else:
            if self._http_data.headers is None:
                raise SakeException(
                    "headers is missing in c# version gamespy", self.command_name
                )
            if "SessionToken" not in self._http_data.headers:
                raise SakeException(
                    "session token is missing", self.command_name)
            self.login_ticket = self._http_data.headers["SessionToken"]
        self.table_id = self._get_str("tableid")

    def parse_headers(self):
        """
        parse headers from http request
        """
        # todo check profileid is same in xml body
        if self._http_data.headers is not None:
            # if "GameID" not in self._http_data.headers:
            #     raise SakeException("game id is missing")
            # self.game_id = int(self._http_data.headers["GameID"])

            # if "ProfileID" not in self._http_data.headers:
            #     raise SakeException("profile id is missing")
            # self.profile_id = int(self._http_data.headers["ProfileID"])
            if "SessionToken" not in self._http_data.headers:
                raise SakeException(
                    "session token is missing", self.command_name)
            self.login_ticket = self._http_data.headers["SessionToken"]

    def _get_record_field(self) -> dict:
        rf = self._get_dict("values")
        if "RecordField" not in rf:
            raise SakeException("No record field tag found", self.command_name)
        values = rf["RecordField"]
        if isinstance(values, dict) and len(values) == 2 and "name" in values and "value" in values:
            values = [values]
        if not isinstance(values, list):
            raise SakeException(
                "record field value is not dict", self.command_name)
        s_value = RecordConverter.to_searchable_format(values)
        return s_value

    def _get_str(self, attr_name: str) -> str:
        try:
            value = super()._get_str(attr_name)
            return value
        except Exception as _:
            raise SakeException(
                f"{attr_name} is missing from the request.", self.command_name
            )

    def _get_int(self, attr_name: str) -> int:
        value = self._get_str(attr_name)
        result = int(value)
        return result


class ResultBase(lib.ResultBase):
    login_ticket: str
    command_name: CommandName
    """
    CreateRecordResponse,
    UpdateRecordResponse,
    ....
    """
    pass


class ResponseBase(lib.ResponseBase):
    _result: ResultBase

    def __init__(self, result: ResultBase) -> None:
        super().__init__(result)

    def build(self) -> None:
        """
        in c# sdk session token is like login ticket
        """
        body = str(self._content)
        headers = {
            "SessionToken": self._result.login_ticket,
            "Content-Length": len(body)
        }
        self.sending_buffer = str(
            HttpData(
                body=body,
                headers=headers
            )
        )
