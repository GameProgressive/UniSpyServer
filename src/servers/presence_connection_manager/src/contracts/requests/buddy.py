from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.aggregates.user_status import UserStatus
from servers.presence_connection_manager.src.aggregates.user_status_info import UserStatusInfo
from servers.presence_connection_manager.src.enums.general import GPStatusCode
from servers.presence_search_player.src.exceptions.general import GPParseException


class AddBuddyRequest(RequestBase):
    friend_profile_id: int
    reason: str

    def parse(self):
        super().parse()
        if (
            ("sesskey" not in self.request_key_values)
            or ("newprofileid" not in self.request_key_values)
            or ("reason" not in self.request_key_values)
        ):
            raise GPParseException("addbuddy request is invalid.")

        self.friend_profile_id = int(self.request_key_values["newprofileid"])
        if self.friend_profile_id == 0:
            raise GPParseException("newprofileid format is incorrect.")

        self.reason = self.request_key_values["reason"]


class DelBuddyRequest(RequestBase):
    friend_profile_id: int

    def parse(self):
        super().parse()
        if "delprofileid" not in self.request_key_values:
            raise GPParseException("delprofileid is missing.")

        self.friend_profile_id = int(self.request_key_values["delprofileid"])
        if self.friend_profile_id == 0:
            raise GPParseException("delprofileid format is incorrect.")


class InviteToRequest(RequestBase):
    product_id: int
    profile_id: int
    session_key: str
    """the invite target profile id"""

    def parse(self):
        super().parse()
        if "productid" not in self.request_key_values:
            raise GPParseException("productid is missing.")

        if "sesskey" not in self.request_key_values:
            raise GPParseException("sesskey is missing.")

        try:
            self.product_id = int(self.request_key_values["productid"])
        except ValueError:
            raise GPParseException("productid format is incorrect.")

        try:
            self.profile_id = int(self.request_key_values["profileid"])
        except ValueError:
            raise GPParseException("profileid format is incorrect.")

        self.session_key = self.request_key_values["sesskey"]


class StatusInfoRequest(RequestBase):
    def __init__(self):
        super().__init__()
        self.is_get_status_info = False
        self.profile_id = 0
        self.namespace_id = None
        self.status_info = UserStatusInfo()

    def parse(self):
        super().parse()

        if (
            "state" not in self.request_key_values
            or "hostip" not in self.request_key_values
            or "hprivip" not in self.request_key_values
            or "qport" not in self.request_key_values
            or "hport" not in self.request_key_values
            or "sessflags" not in self.request_key_values
            or "rechstatus" not in self.request_key_values
            or "gametype" not in self.request_key_values
            or "gamevariant" not in self.request_key_values
            or "gamemapname" not in self.request_key_values
        ):
            raise GPParseException("StatusInfo request is invalid.")

        self.status_info.status_state = self.request_key_values["state"]
        self.status_info.host_ip = self.request_key_values["hostip"]
        self.status_info.host_private_ip = self.request_key_values["hprivip"]

        try:
            self.status_info.query_report_port = int(self.request_key_values["qport"])
            self.status_info.host_port = int(self.request_key_values["hport"])
            self.status_info.session_flags = int(self.request_key_values["sessflags"])
        except ValueError:
            raise GPParseException("qport, hport, or sessflags format is incorrect.")

        self.status_info.rich_status = self.request_key_values["rechstatus"]
        self.status_info.game_type = self.request_key_values["gametype"]
        self.status_info.game_variant = self.request_key_values["gamevariant"]
        self.status_info.game_map_name = self.request_key_values["gamemapname"]


class StatusRequest(RequestBase):
    def __init__(self, raw_request):
        super().__init__(raw_request)
        self.status = UserStatus()
        self.IsGetStatus = False

    def parse(self):
        super().parse()

        if "status" not in self.request_key_values:
            raise GPParseException("status is missing.")

        if "statstring" not in self.request_key_values:
            raise GPParseException("statstring is missing.")

        if "locstring" not in self.request_key_values:
            raise GPParseException("locstring is missing.")

        try:
            status_code = int(self.request_key_values["status"])
            self.status.current_status = GPStatusCode(status_code)
        except ValueError:
            raise GPParseException("status format is incorrect.")

        self.status.location_string = self.request_key_values["locstring"]
        self.status.status_string = self.request_key_values["statstring"]
