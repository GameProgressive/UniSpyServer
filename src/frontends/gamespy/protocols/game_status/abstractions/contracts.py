from typing import Optional
import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.library.extentions.gamespy_utils import convert_to_key_value
from frontends.gamespy.protocols.game_status.aggregations.exceptions import GSException


class RequestBase(lib.RequestBase):
    command_name: str
    raw_request: str
    local_id: Optional[int]
    request_dict: dict[str, str]

    @staticmethod
    def convert_game_data_to_key_values(game_data: str):
        assert isinstance(game_data, str)
        game_data = game_data.replace("\u0001", "\\")
        convert_to_key_value(game_data)

    def parse(self) -> None:
        self.request_dict = convert_to_key_value(self.raw_request)

        if "lid" in self.request_dict:
            try:
                self.local_id = int(self.request_dict["lid"])
            except:
                raise GSException("local id is not valid.")

        if "id" in self.request_dict:
            try:
                self.local_id = int(self.request_dict["id"])
            except:
                raise GSException("local id is not valid.")


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _request: RequestBase
    _result: ResultBase
    sending_buffer: str
