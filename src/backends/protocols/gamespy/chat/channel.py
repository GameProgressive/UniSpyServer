from datetime import datetime
from typing import TYPE_CHECKING
from uuid import UUID

from backends.library.database.pg_orm import PG_SESSION, ChatChannelCaches, ChatChannelUserCaches, ChatUserCaches
import backends.protocols.gamespy.chat.data as data
from protocols.chat.aggregates.enums import ModeOperationType
from protocols.chat.aggregates.exceptions import InviteOnlyChanException, NoSuchChannelException


class ChannelUserHelper:
    @staticmethod
    def get_mode_string(user: ChatChannelUserCaches):
        assert isinstance(user.is_channel_operator, bool)
        assert isinstance(user.is_voiceable, bool)
        buffer = ""
        if user.is_channel_operator:
            buffer += "@"
        if user.is_voiceable:
            buffer += "+"
        return buffer



class ChannelHelper:
    @staticmethod
    def join(channel: ChatChannelCaches, user: ChatUserCaches) -> None:
        # 1 check if is a invited channel
        # 1.1 check if user is in a invited list
        assert isinstance(channel.modes, list)
        # assert isinstance(channel.invited_nicks,list)
        if ModeOperationType.SET_INVITED_ONLY in channel.modes:
            if user.nick_name not in channel.invited_nicks:
                raise InviteOnlyChanException(
                    f"You can only join channel: {channel.channel_name} when you are in invite list")

        # 2 check if user is in ban list, if it is user can not join
        raise NotImplementedError()

    @staticmethod
    def quit(channel: ChatChannelCaches, user: ChatUserCaches) -> None:
        raise NotImplementedError()

    @staticmethod
    def kick(channel: ChatChannelCaches, kicker: ChatUserCaches, kickee: ChatUserCaches) -> None:
        raise NotImplementedError()

    @staticmethod
    def invite(channel: ChatChannelCaches, inviter: ChatChannelUserCaches, invitee: ChatUserCaches) -> None:

        if str(inviter.channel_name) != str(channel.channel_name):
            raise InviteOnlyChanException(
                f"The inviter:{inviter.nick_name} is not a user in channel:{channel.channel_name}.")

        assert isinstance(channel.invited_nicks, list)
        channel.invited_nicks.append(invitee.nick_name)
        PG_SESSION.commit()

    @staticmethod
    def create(server_id: UUID,
               channel_name: str,
               password: str,
               game_name: str,
               room_name: str,
               topic: str,
               group_id: int,
               max_num_user: int,
               key_values: dict,
               update_time: datetime,
               modes: str,
               creator: str | None = None) -> ChatChannelCaches:
        # check whether if channel exist, if not user is creator
        is_exist = data.is_channel_exist(channel_name, game_name)
        if is_exist:
            raise NoSuchChannelException(
                f"Channel: {channel_name} is already exist, can not create a new one")
        cache = ChatChannelCaches(server_id=server_id,
                                  channel_name=channel_name,
                                  password=password,
                                  game_name=game_name,
                                  room_name=room_name,
                                  topic=topic,
                                  group_id=group_id,
                                  max_num_user=max_num_user,
                                  key_values=key_values,
                                  update_time=update_time,
                                  creator=creator,
                                  modes=modes)
        PG_SESSION.add(cache)
        PG_SESSION.commit()

        return cache

    @staticmethod
    def change_modes(channel: ChatChannelCaches, changer: ChatChannelUserCaches, modes: list[ModeOperationType], args: list):
        assert isinstance(channel, ChatChannelCaches)
        assert isinstance(changer, ChatChannelUserCaches)
        assert isinstance(channel.modes, list)
        assert all(isinstance(m, ModeOperationType) for m in modes)

        for flag in modes:
            if flag not in channel.modes:
                channel.modes.append(flag)
            match flag:
                case ModeOperationType.ENABLE_USER_QUIET_FLAG:
                    if changer.is_channel_operator:  # type:ignore
                        if ModeOperationType.ENABLE_USER_QUIET_FLAG not in channel.modes:
                            channel.modes.append(
                                ModeOperationType.ENABLE_USER_QUIET_FLAG)
                case ModeOperationType.DISABLE_USER_QUIET_FLAG:
                    if changer.is_channel_operator:  # type:ignore
                        if ModeOperationType.ENABLE_USER_QUIET_FLAG not in channel.modes:
                            channel.modes.remove(
                                ModeOperationType.ENABLE_USER_QUIET_FLAG)
                case ModeOperationType.ADD_CHANNEL_PASSWORD:
                    assert isinstance(args[0], str)
                    if changer.is_channel_operator:  # type:ignore
                        channel.password = args[0]  # type:ignore
                case ModeOperationType.REMOVE_CHANNEL_PASSWORD:
                    if changer.is_channel_operator:  # type:ignore
                        channel.password = ""  # type:ignore
                case ModeOperationType.ADD_CHANNEL_USER_LIMITS:
                    channel.max_num_user = args[0]  # type: ignore
                case ModeOperationType.REMOVE_CHANNEL_USER_LIMITS:
                    channel.max_num_user = 200  # type: ignore
                case ModeOperationType.ADD_BAN_ON_USER:
                    assert isinstance(channel.banned_nicks, list)
                    if args[0] not in list(channel.banned_nicks):  # type: ignore
                        channel.banned_nicks.append(args[0])
                case ModeOperationType.REMOVE_BAN_ON_USER:
                    assert isinstance(channel.banned_nicks, list)
                    if args[0] in list(channel.banned_nicks):
                        channel.banned_nicks.remove(args[0])
                case ModeOperationType.ADD_CHANNEL_OPERATOR:
                    assert isinstance(args[0], ChatChannelUserCaches)
                    user: ChatChannelUserCaches = args[0]
                    user.is_channel_operator = True  # type: ignore
                case ModeOperationType.REMOVE_CHANNEL_OPERATOR:
                    assert isinstance(args[0], ChatChannelUserCaches)
                    user: ChatChannelUserCaches = args[0]
                    user.is_channel_operator = False  # type: ignore
                case ModeOperationType.ENABLE_USER_VOICE_PERMISSION:
                    assert isinstance(args[0], ChatChannelUserCaches)
                    user: ChatChannelUserCaches = args[0]
                    user.is_voiceable = True  # type: ignore
                case ModeOperationType.DISABLE_USER_VOICE_PERMISSION:
                    assert isinstance(args[0], ChatChannelUserCaches)
                    user: ChatChannelUserCaches = args[0]
                    user.is_voiceable = False  # type: ignore

        PG_SESSION.commit()
