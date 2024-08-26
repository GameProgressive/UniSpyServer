from typing import Optional
from library.src.extentions.gamespy_utils import convert_to_key_value
from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.aggregates.user_status import UserStatus
from servers.presence_connection_manager.src.aggregates.user_status_info import (
    UserStatusInfo,
)
from servers.presence_connection_manager.src.enums.general import GPStatusCode
from servers.presence_search_player.src.exceptions.general import GPParseException


class AddBuddyRequest(RequestBase):
    friend_profile_id: int
    reason: str

    def parse(self):
        super().parse()
        if (
            ("sesskey" not in self.request_dict)
            or ("newprofileid" not in self.request_dict)
            or ("reason" not in self.request_dict)
        ):
            raise GPParseException("addbuddy request is invalid.")
        try:
            self.friend_profile_id = int(self.request_dict["newprofileid"])
        except:
            raise GPParseException("newprofileid format is incorrect.")

        self.reason = self.request_dict["reason"]


class DelBuddyRequest(RequestBase):
    friend_profile_id: int

    def parse(self):
        super().parse()
        if "delprofileid" not in self.request_dict:
            raise GPParseException("delprofileid is missing.")

        try:
            self.friend_profile_id = int(self.request_dict["delprofileid"])
        except:
            raise GPParseException("delprofileid format is incorrect.")


class InviteToRequest(RequestBase):
    product_id: int
    profile_id: int
    session_key: str
    """the invite target profile id"""

    def parse(self):
        super().parse()
        if "productid" not in self.request_dict:
            raise GPParseException("productid is missing.")

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing.")

        try:
            self.product_id = int(self.request_dict["productid"])
        except ValueError:
            raise GPParseException("productid format is incorrect.")

        try:
            self.profile_id = int(self.request_dict["profileid"])
        except ValueError:
            raise GPParseException("profileid format is incorrect.")

        self.session_key = self.request_dict["sesskey"]


class StatusInfoRequest(RequestBase):
    namespace_id: Optional[int] = None
    status_info: UserStatusInfo = UserStatusInfo()
    profile_id: int = 0

    def __init__(self, raw_request: Optional[str] = None) -> None:
        if raw_request is not None:
            self.raw_request = raw_request

    def parse(self):
        super().parse()

        if (
            "state" not in self.request_dict
            or "hostip" not in self.request_dict
            or "hprivip" not in self.request_dict
            or "qport" not in self.request_dict
            or "hport" not in self.request_dict
            or "sessflags" not in self.request_dict
            or "rechstatus" not in self.request_dict
            or "gametype" not in self.request_dict
            or "gamevariant" not in self.request_dict
            or "gamemapname" not in self.request_dict
        ):
            raise GPParseException("StatusInfo request is invalid.")

        self.status_info.status_state = self.request_dict["state"]
        self.status_info.host_ip = self.request_dict["hostip"]
        self.status_info.host_private_ip = self.request_dict["hprivip"]

        try:
            self.status_info.query_report_port = int(
                self.request_dict["qport"])
            self.status_info.host_port = int(self.request_dict["hport"])
            self.status_info.session_flags = self.request_dict["sessflags"]
        except ValueError:
            raise GPParseException(
                "qport, hport, or sessflags format is incorrect.")

        self.status_info.rich_status = self.request_dict["rechstatus"]
        self.status_info.game_type = self.request_dict["gametype"]
        self.status_info.game_variant = self.request_dict["gamevariant"]
        self.status_info.game_map_name = self.request_dict["gamemapname"]


class StatusRequest(RequestBase):
    status: UserStatus = UserStatus()
    is_get_status: bool

    def parse(self):
        self.request_dict = convert_to_key_value(self.raw_request)
        self.command_name = list(self.request_dict.keys())[0]

        if "status" not in self.request_dict:
            raise GPParseException("status is missing.")

        if "statstring" not in self.request_dict:
            raise GPParseException("statstring is missing.")

        if "locstring" not in self.request_dict:
            raise GPParseException("locstring is missing.")

        try:
            status_code = int(self.request_dict["status"])
            self.status.current_status = GPStatusCode(status_code)
        except ValueError:
            raise GPParseException("status format is incorrect.")

        self.status.location_string = self.request_dict["locstring"]
        self.status.status_string = self.request_dict["statstring"]
