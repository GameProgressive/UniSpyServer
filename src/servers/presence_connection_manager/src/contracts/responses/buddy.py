from servers.presence_connection_manager.src.abstractions.contracts import (
    RequestBase,
    ResponseBase,
)
from servers.presence_connection_manager.src.contracts.requests.buddy import AddBuddyRequest, StatusInfoRequest
from servers.presence_connection_manager.src.contracts.results.buddy import (
    AddBuddyResult,
    BlockListResult,
    BuddyListResult,
    StatusInfoResult,
)


class AddBuddyResponse(ResponseBase):
    def __init__(self, request: AddBuddyRequest, result: AddBuddyResult) -> None:
        assert issubclass(type(request), AddBuddyRequest)
        assert issubclass(type(result), AddBuddyResult)
        super().__init__(request, result)

    def build(self) -> None:
        # return super().build()
        raise NotImplementedError()
        # \bm\<buddy message type>\f\<profile id>\date\<date>
        # GPI_BM_MESSAGE: \msg\<msg>\
        # GPI_BM_UTM:\msg\<msg>\
        # GPI_BM_REQUEST:\msg\|signed|<signed data>\
        # GPI_BM_AUTH:
        # GPI_BM_REVOKE:
        # GPI_BM_STATUS:\msg\|s|<status code>\ or \msg\|ss|<status info status string>|ls|<location string>|ip|<ip>|p|<product id>|qm|<quiet mode flag>
        # GPI_BM_INVITE:\msg\|p|<product id>|l|<location string>
        # GPI_BM_PING:\msg\\


class BlockListResponse(ResponseBase):
    _result: BlockListResult

    def __init__(self, result: BlockListResult):
        assert isinstance(result, BlockListResult)
        self._result = result

    def build(self):
        # \blk\< num in list >\list\< profileid list - comma delimited >\final\
        self.sending_buffer = f"\\blk\\{len(self._result.profile_ids)}\\list\\"
        self.sending_buffer += ",".join(str(pid) for pid in self._result.profile_ids)
        self.sending_buffer += "\\final\\"


class BuddyListResponse(ResponseBase):
    _result: BuddyListResult

    def __init__(self, request: RequestBase, result: BuddyListResult):
        super().__init__(request, result)

    def build(self):
        # \bdy\< num in list >\list\< profileid list - comma delimited >\final\
        self.sending_buffer = f"\\bdy\\{len(self._result.profile_ids)}\\list\\"
        self.sending_buffer += ",".join(str(pid) for pid in self._result.profile_ids)
        self.sending_buffer += "\\final\\"


class StatusInfoResponse(ResponseBase):
    _result: StatusInfoResult

    def __init__(self, request: StatusInfoRequest, result: StatusInfoResult):
        assert isinstance(request, StatusInfoRequest)
        assert isinstance(result, StatusInfoResult)
        super().__init__(request, result)

    def build(self):
        # \bsi\\state\\profile\\bip\\bport\\hostip\\hprivip\\qport\\hport\\sessflags\\rstatus\\gameType\\gameVnt\\gameMn\\product\\qmodeflags\
        self.sending_buffer = (
            f"\\bsi\\state\\{self._result.status_info.status_state}\\"
            f"profile\\{self._result.profile_id}\\bip\\{self._result.status_info.buddy_ip}\\"
            f"hostIp\\{self._result.status_info.host_ip}\\hprivIp\\{self._result.status_info.host_private_ip}\\"
            f"qport\\{self._result.status_info.query_report_port}\\hport\\{self._result.status_info.host_port}\\"
            f"sessflags\\{self._result.status_info.session_flags}\\rstatus\\{self._result.status_info.rich_status}\\"
            f"gameType\\{self._result.status_info.game_type}\\gameVnt\\{self._result.status_info.game_variant}\\"
            f"gameMn\\{self._result.status_info.game_map_name}\\product\\{self._result.product_id}\\"
            f"qmodeflags\\{self._result.status_info.quiet_mode_flags}\\final\\"
        )
