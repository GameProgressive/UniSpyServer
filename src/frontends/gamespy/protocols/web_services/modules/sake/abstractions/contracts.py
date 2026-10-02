import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.library.network.http_data import HttpData
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import (
    CommandName,
    SakePlatform,
)
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.exceptions import (
    SakeException,
)
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.utils import (
    RecordConverter,
)

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

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "RequestBase":
        data = lib.RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["platform"] = SakePlatform.Windows
        data_dict["game_id"] = data.int_mapping["gameid"]
        data_dict["secret_key"] = data.str_mapping.get("secretKey")
        data_dict["table_id"] = data.str_mapping["tableid"]
        data_dict["login_ticket"] = data.str_mapping.get("loginTicket")

        if data_dict["login_ticket"] is None:
            if data.raw_request.headers is None:
                raise SakeException(
                    "headers is missing in c# version gamespy", data.command_name
                )
            data_dict["login_ticket"] = data.raw_request.headers["SessionToken"]
        data = RequestBase(**data_dict)
        return data

    @staticmethod
    def _get_record_field(request: lib.RequestBase) -> dict:
        rf = request.dict_mapping["values"]
        if "RecordField" not in rf:
            raise SakeException("No record field tag found", request.command_name)
        values = rf["RecordField"]
        if (
            isinstance(values, dict)
            and len(values) == 2
            and "name" in values
            and "value" in values
        ):
            values = [values]
        if not isinstance(values, list):
            raise SakeException("record field value is not dict", request.command_name)
        s_value = RecordConverter.to_searchable_format(values)
        return s_value


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
            "Content-Length": len(body),
        }
        self.sending_buffer = HttpData(path=None, body=body, headers=headers)
