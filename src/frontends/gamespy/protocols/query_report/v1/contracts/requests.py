from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.query_report.aggregates.game_server_info import (
    NESSESARY_KEYS,
)
from frontends.gamespy.protocols.query_report.v1.abstractions.contracts import (
    RequestBase,
)
from frontends.gamespy.protocols.query_report.v1.aggregates.enums import (
    CommandName,
    ServerStatus,
)


class HeartbeatPreRequest(RequestBase):
    """
    heartbeat do not have final command
    \\heartbeat\\26900\\gamename\\gmtest
    """

    game_port: int
    game_name: str
    status: ServerStatus

    @staticmethod
    def gs_parse(raw_request: str) -> RequestBase:
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["game_name"] = data.request_dict["gamename"]
        data_dict["game_port"] = data.request_dict["heartbeat"]
        data_dict["status"] = data.request_dict.get("statechanged", ServerStatus.START)
        data = HeartbeatPreRequest(**data_dict)
        return data


class LegacyHeartbeatRequest(RequestBase):
    """
    heartbeat ack request is start with gamename:
    \\gamename\\gmtest\\gamever\\2.00\\location\\1\\hostname\\GameMaster Arena Server\\hostport\\25000\\mapname\\gmtmap1\\gametype\\arena\\numplayers\\4\\maxplayers\\32\\gamemode\\openplaying\\timelimit\\40\\fraglimit\\0\\teamplay\\1\\rankedserver\\1\\player_0\\Joe Player\\frags_0\\25\\deaths_0\\6\\skill_0\\272\\ping_0\\119\\team_0\\\\player_1\\L33t 0n3\\frags_1\\12\\deaths_1\\24\\skill_1\\136\\ping_1\\98\\team_1\\\\player_2\\Raptor\\frags_2\\11\\deaths_2\\26\\skill_2\\44\\ping_2\\445\\team_2\\Blue\\player_3\\Gr81\\frags_3\\23\\deaths_3\\18\\skill_3\\173\\ping_3\\129\\team_3\\Red\\final\\\\queryid\\2.1
    """

    query_id: str
    game_name: str
    group_id: int | None
    host_name: str
    host_port: int

    @staticmethod
    def gs_parse(raw_request: str) -> RequestBase:
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["command_name"] = CommandName.HEARTBEAT
        data_dict["query_id"] = data.request_dict["queryid"]
        data_dict["game_name"] = data.request_dict["gamename"]
        data_dict["group_id"] = data.request_dict.get("groupid")

        data_dict["host_name"] = data.request_dict["hostname"]
        data_dict["host_port"] = data.request_dict["hostport"]
        data = LegacyHeartbeatRequest(**data_dict)
        return data


if __name__ == "__main__":
    heartbeat = "\\heartbeat\\26900\\gamename\\gmtest"
    basic = "\\gamename\\gmtest\\gamever\\2.00\\location\\1\\queryid\\2.1"
    info = "\\hostname\\GameMaster Arena Server\\hostport\\25000\\mapname\\gmtmap1\\gametype\\arena\\numplayers\\5\\maxplayers\\32\\gamemode\\openplaying\\queryid\\2.2"
    rules = "\\timelimit\\40\\fraglimit\\0\\teamplay\\1\\rankedserver\\1\\queryid\\2.3"
    players = "\\player_0\\Joe Player\\frags_0\\3\\deaths_0\\6\\skill_0\\225\\ping_0\\137\\team_0\\Blue\\player_1\\L33t 0n3\\frags_1\\26\\deaths_1\\18\\skill_1\\829\\ping_1\\247\\team_1\\\\player_2\\Raptor\\frags_2\\30\\deaths_2\\17\\skill_2\\888\\ping_2\\233\\team_2\\Red\\player_3\\Gr81\\frags_3\\23\\deaths_3\\19\\skill_3\\415\\ping_3\\409\\team_3\\\\player_4\\Flubber\\frags_4\\23\\deaths_4\\31\\skill_4\\837\\ping_4\\443\\team_4\\\\final\\\\queryid\\2.4"

    status = "\\gamename\\gmtest\\gamever\\2.00\\location\\1\\hostname\\GameMaster Arena Server\\hostport\\25000\\mapname\\gmtmap1\\gametype\\arena\\numplayers\\4\\maxplayers\\32\\gamemode\\openplaying\\timelimit\\40\\fraglimit\\0\\teamplay\\1\\rankedserver\\1\\player_0\\Joe Player\\frags_0\\25\\deaths_0\\6\\skill_0\\272\\ping_0\\119\\team_0\\\\player_1\\L33t 0n3\\frags_1\\12\\deaths_1\\24\\skill_1\\136\\ping_1\\98\\team_1\\\\player_2\\Raptor\\frags_2\\11\\deaths_2\\26\\skill_2\\44\\ping_2\\445\\team_2\\Blue\\player_3\\Gr81\\frags_3\\23\\deaths_3\\18\\skill_3\\173\\ping_3\\129\\team_3\\Red\\final\\\\queryid\\2.1"
    req = LegacyHeartbeatRequest.gs_parse(status)
