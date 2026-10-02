from socket import inet_ntoa

from frontends.gamespy.protocols.server_browser.v2.abstractions.contracts import (
    AdHocRequestBase,
    RequestBase,
    ServerListUpdateOptionRequestBase,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import (
    CommandName,
    ServerListUpdateOption,
)
from frontends.gamespy.protocols.server_browser.v2.aggregations.exceptions import (
    SBException,
)


class ServerListRequest(ServerListUpdateOptionRequestBase):

    @staticmethod
    def gs_parse(raw_request: bytes) -> "ServerListRequest":
        data = RequestBase.gs_parse(raw_request)
        data.command_name = CommandName(raw_request[0])
        if data.command_name != CommandName.SERVER_LIST_REQUEST:
            raise SBException("raw request is not valid server list request")
        data_dict = data.model_dump()

        data_dict["request_version"] = int(raw_request[2])
        data_dict["protocol_version"] = int(raw_request[3])
        data_dict["encoding_version"] = int(raw_request[4])
        data_dict["game_version"] = int.from_bytes(raw_request[5:9], "little")

        remain_data = raw_request[9:]
        dev_game_name_index = remain_data.index(0)
        data_dict["dev_game_name"] = remain_data[:dev_game_name_index].decode()

        remain_data = remain_data[dev_game_name_index + 1 :]
        game_name_index = remain_data.index(0)
        data_dict["game_name"] = remain_data[:game_name_index].decode()

        remain_data = remain_data[game_name_index + 1 :]
        data_dict["client_challenge"] = remain_data[:8].decode()

        remain_data = remain_data[8:]
        filter_index = remain_data.index(0)
        data_dict["filter"] = None
        if filter_index > 0:
            data_dict["filter"] = remain_data[:filter_index].decode()
        remain_data = remain_data[filter_index + 1 :]

        keys_index = remain_data.index(0)
        data_dict["keys"] = remain_data[1:keys_index].decode().split("\\")
        remain_data = remain_data[keys_index + 1 :]

        byte_update_options = remain_data[:4]
        data_dict["update_option"] = ServerListUpdateOption(
            int.from_bytes(byte_update_options)
        )
        remain_data = remain_data[4:]
        data_dict["source_ip"] = None
        if data_dict["update_option"] & ServerListUpdateOption.ALTERNATE_SOURCE_IP:
            data_dict["source_ip"] = inet_ntoa(remain_data[:4])
            remain_data = remain_data[4:]
        data_dict["max_servers"] = None
        if data_dict["update_option"] & ServerListUpdateOption.LIMIT_RESULT_COUNT:
            if len(remain_data) != 4:
                raise SBException("The max number of server is incorrect.")
            data_dict["max_servers"] = int(remain_data[:4][::-1])
        data = ServerListRequest(**data_dict)
        return data


class SendMessageRequest(AdHocRequestBase):
    prefix_message: bytes
    client_message: bytes
    natneg_cookie: int

    @staticmethod
    def gs_parse(raw_request: bytes) -> "SendMessageRequest":
        data = AdHocRequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["client_message"] = raw_request[9:]
        data_dict["natneg_cookie"] = int.from_bytes(data_dict["client_message"][6:10])
        data_dict["prefix_message"] = data_dict["client_message"][0:5]
        data = SendMessageRequest(**data_dict)
        return data


class ServerInfoRequest(AdHocRequestBase):
    @staticmethod
    def gs_parse(raw_request: bytes) -> "ServerInfoRequest":
        data = AdHocRequestBase.gs_parse(raw_request)
        data = ServerInfoRequest(**data.model_dump())
        return data
