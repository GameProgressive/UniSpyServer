from uuid import UUID
from datetime import datetime
from pydantic import BaseModel

import frontends.gamespy.library.abstractions.contracts as lib
from frontends.gamespy.protocols.chat.aggregates.enums import CommandName


class IrcParam(BaseModel):
    prefix: str
    command: str
    params: list[str] = []
    trailing: str


class RequestBase(lib.RequestBase):
    raw_request: str
    command_name: CommandName | None
    irc: IrcParam
    websocket_address: str | None = None

    @staticmethod
    def gs_parse(raw_request: str) -> "RequestBase":
        # at most 2 colon character
        # we do not sure about all command
        # so i block this code here
        data = lib.RequestBase.gs_parse(raw_request)
        raw = raw_request.replace("\r", "").replace("\n", "").strip()

        if not raw:
            raise ValueError("IRC request cannot be empty")

        prefix = ""
        message = ""

        # IRC format：
        # [:prefix] COMMAND [params] [:message]
        if raw.startswith(":"):
            prefix_end = raw.find(" ")

            if prefix_end == -1:
                raise ValueError(f"IRC request is invalid: {raw}")

            prefix = raw[1:prefix_end]
            raw = raw[prefix_end:].lstrip()

        # trailing message only get the first :
        if " :" in raw:
            command_part, message = raw.split(" :", 1)
        elif raw.startswith(":"):
            command_part = ""
            message = raw[1:]
        else:
            command_part = raw

        rest_data = command_part.split()

        if not data:
            raise ValueError(f"IRC request is invalid: {raw_request}")

        command = rest_data[0]
        params = rest_data[1:]
        irc = IrcParam(
            prefix=prefix,
            command=command,
            params=params,
            trailing=message,
        )
        data.command_name = irc.command
        data = RequestBase(**data.model_dump(), irc=irc)
        return data


class ResultBase(lib.ResultBase):
    pass


SERVER_DOMAIN = "unispy.net"


class ResponseBase(lib.ResponseBase):
    sending_buffer: str
    _result: ResultBase

    def __init__(self, result: ResultBase) -> None:
        super().__init__(result)
        if result is not None:
            assert issubclass(type(result), ResultBase)

    @staticmethod
    def build_irc_user_prefix(nick_name: str, user_name: str):
        return f"{nick_name}!{user_name}@{SERVER_DOMAIN}"


# region Brocker


class BrockerMessage(BaseModel):
    server_id: UUID
    channel_name: str
    sender_ip_address: str
    sender_port: int
    message: str


class ChatChannelCaches(BaseModel):
    channel_name: str
    server_id: UUID
    creator: str
    game_name: str
    room_name: str
    topic: str | None
    password: str | None
    group_id: int
    max_num_user: int
    key_values: dict
    invited_nicks: list[str]
    modes: list[str]
    banned_nicks: list[str]
    update_time: datetime
