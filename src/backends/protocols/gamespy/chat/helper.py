from datetime import datetime
from enum import Enum
from uuid import UUID

from backends.library.database.pg_orm import (
    PG_SESSION,
    ChatChannelCaches,
    ChatChannelUserCaches,
    ChatUserCaches,
)
import backends.protocols.gamespy.chat.data as data
from backends.protocols.gamespy.chat.requests import ModeRequest
from frontends.gamespy.protocols.chat.abstractions.contract import SERVER_DOMAIN
from frontends.gamespy.protocols.chat.aggregates.enums import ModeOperationType
from frontends.gamespy.protocols.chat.aggregates.exceptions import (
    BadChannelKeyException,
    BannedFromChanException,
    InviteOnlyChanException,
    NoSuchChannelException,
)


class ChannelUserProperty(Enum):
    CHANNEL_CREATOR = "channel_creator"
    CHANNEL_OPERATOR = "channel_operator"


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

    @staticmethod
    def get_user_irc_prefix(user: ChatChannelUserCaches):
        irc = f"{user.nick_name}!{user.user_name}@{SERVER_DOMAIN}"
        return irc


class ChannelHelper:
    @staticmethod
    def join(channel: ChatChannelCaches, user: ChatUserCaches) -> None:
        assert isinstance(channel, ChatChannelCaches)
        assert isinstance(channel.modes, list)
        assert isinstance(user.nick_name, str)
        assert isinstance(channel.banned_nicks, list)
        # 1 check if is a invited channel
        # 1.1 check if user is in a invited list

        if ModeOperationType.SET_INVITED_ONLY in channel.modes:
            if user.nick_name not in channel.invited_nicks:
                raise InviteOnlyChanException(
                    f"You can only join channel: {channel.channel_name} when you are in invite list"
                )

        # 2 check if user is in ban list, if it is user can not join
        if user.nick_name in channel.banned_nicks:
            raise BannedFromChanException(
                "can not join channel, because you are in ban list"
            )
        if channel.creator == user.nick_name:  # type:ignore
            is_creator = True
        else:
            is_creator = False
        chan_user = ChatChannelUserCaches(
            nick_name=user.nick_name,
            user_name=user.user_name,
            channel_name=channel.channel_name,
            update_time=datetime.now(),
            is_voiceable=True,
            is_channel_operator=False,
            is_channel_creator=is_creator,
            remote_ip_address=user.remote_ip_address,
            remote_port=user.remote_port,
        )
        PG_SESSION.add(chan_user)
        PG_SESSION.commit()

    @staticmethod
    def quit(channel: ChatChannelCaches, quiter: ChatChannelUserCaches) -> None:
        assert isinstance(quiter, ChatChannelUserCaches)
        assert isinstance(channel, ChatChannelCaches)
        assert isinstance(quiter.channel_name, str)
        assert isinstance(channel.channel_name, str)

        if quiter.channel_name != channel.channel_name:  # type:ignore
            print("user is not in channel, so can not quit")
            return
        PG_SESSION.delete(quiter)
        PG_SESSION.commit()

    @staticmethod
    def kick(
        channel: ChatChannelCaches,
        kicker: ChatChannelUserCaches,
        kickee: ChatChannelUserCaches,
    ) -> None:
        assert isinstance(channel, ChatChannelCaches)
        assert isinstance(kicker, ChatChannelUserCaches)
        assert isinstance(kickee, ChatChannelUserCaches)
        assert isinstance(kicker.channel_name, str)
        assert isinstance(channel.channel_name, str)
        assert isinstance(kickee.channel_name, str)
        if kicker.channel_name != channel.channel_name:  # type:ignore
            raise BadChannelKeyException(
                f"kicker is not in channel: {channel.channel_name}"
            )
        if kickee.channel_name != channel.channel_name:  # type:ignore
            raise BadChannelKeyException(
                f"kickee is not in channel: {channel.channel_name}"
            )
        if not kicker.is_channel_operator:  # type:ignore
            raise BadChannelKeyException("kick failed, kicker is not channel operator")
        PG_SESSION.delete(kickee)
        PG_SESSION.commit()

    @staticmethod
    def invite(
        channel: ChatChannelCaches,
        inviter: ChatChannelUserCaches,
        invitee: ChatUserCaches,
    ) -> None:
        if str(inviter.channel_name) != str(channel.channel_name):
            raise InviteOnlyChanException(
                f"The inviter:{inviter.nick_name} is not a user in channel:{channel.channel_name}."
            )

        assert isinstance(channel.invited_nicks, list)
        channel.invited_nicks.append(invitee.nick_name)
        PG_SESSION.commit()

    @staticmethod
    def create(
        server_id: UUID,
        channel_name: str,
        password: str,
        game_name: str,
        room_name: str,
        topic: str,
        group_id: int,
        max_num_user: int,
        key_values: dict,
        update_time: datetime,
        modes: list,
        creator: str | None = None,
    ) -> ChatChannelCaches:
        # check whether if channel exist, if not user is creator
        is_exist = data.is_channel_exist(channel_name, game_name)
        if is_exist:
            raise NoSuchChannelException(
                f"Channel: {channel_name} is already exist, can not create a new one"
            )
        cache = ChatChannelCaches(
            server_id=server_id,
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
            modes=modes,
        )
        PG_SESSION.add(cache)
        PG_SESSION.commit()

        return cache

    @staticmethod
    def change_modes(
        channel: ChatChannelCaches,
        changer: ChatChannelUserCaches,
        request: ModeRequest,
    ):
        assert isinstance(channel, ChatChannelCaches)
        assert isinstance(changer, ChatChannelUserCaches)
        assert isinstance(channel.modes, list)
        assert all(isinstance(m, ModeOperationType) for m in request.mode_operations)

        for flag in request.mode_operations:
            if flag not in channel.modes:
                channel.modes.append(flag)
            match flag:
                case ModeOperationType.ENABLE_USER_QUIET_FLAG:
                    if changer.is_channel_operator:  # type:ignore
                        if (
                            ModeOperationType.ENABLE_USER_QUIET_FLAG
                            not in channel.modes
                        ):
                            channel.modes.append(
                                ModeOperationType.ENABLE_USER_QUIET_FLAG
                            )
                case ModeOperationType.DISABLE_USER_QUIET_FLAG:
                    if changer.is_channel_operator:  # type:ignore
                        if (
                            ModeOperationType.ENABLE_USER_QUIET_FLAG
                            not in channel.modes
                        ):
                            channel.modes.remove(
                                ModeOperationType.ENABLE_USER_QUIET_FLAG
                            )
                case ModeOperationType.ADD_CHANNEL_PASSWORD:
                    assert isinstance(request.password, str)
                    if changer.is_channel_operator:  # type:ignore
                        channel.password = request.password  # type:ignore
                case ModeOperationType.REMOVE_CHANNEL_PASSWORD:
                    if changer.is_channel_operator:  # type:ignore
                        channel.password = ""  # type:ignore
                case ModeOperationType.ADD_CHANNEL_USER_LIMITS:
                    channel.max_num_user = request.limit_number  # type: ignore
                case ModeOperationType.REMOVE_CHANNEL_USER_LIMITS:
                    channel.max_num_user = 200  # type: ignore
                case ModeOperationType.ADD_BAN_ON_USER:
                    assert isinstance(channel.banned_nicks, list)
                    # type: ignore
                    if request.nick_name not in list(channel.banned_nicks):
                        channel.banned_nicks.append(request.nick_name)
                case ModeOperationType.REMOVE_BAN_ON_USER:
                    assert isinstance(channel.banned_nicks, list)
                    if request.nick_name in list(channel.banned_nicks):
                        channel.banned_nicks.remove(request.nick_name)
                case ModeOperationType.ADD_CHANNEL_OPERATOR:
                    u = data.get_channel_user_cache_by_name(
                        request.channel_name, request.nick_name
                    )
                    if u is None:
                        raise BadChannelKeyException(
                            f"no user found with nick name:{request.nick_name}"
                        )

                    u.is_channel_operator = True  # type: ignore
                    PG_SESSION.commit()
                case ModeOperationType.REMOVE_CHANNEL_OPERATOR:
                    u = data.get_channel_user_cache_by_name(
                        request.channel_name, request.nick_name
                    )
                    u.is_channel_operator = False  # type: ignore
                    PG_SESSION.commit()

                case ModeOperationType.ENABLE_USER_VOICE_PERMISSION:
                    u = data.get_channel_user_cache_by_name(
                        request.channel_name, request.nick_name
                    )
                    u.is_voiceable = True  # type: ignore
                case ModeOperationType.DISABLE_USER_VOICE_PERMISSION:
                    u = data.get_channel_user_cache_by_name(
                        request.channel_name, request.nick_name
                    )
                    u.is_voiceable = False  # type: ignore

        PG_SESSION.commit()

    @staticmethod
    def get_all_user_nick_string(channel: ChatChannelCaches) -> str:
        assert isinstance(channel, ChatChannelCaches)
        assert isinstance(channel.channel_name, str)
        users = data.get_channel_user_caches_by_name(channel.channel_name)
        nicks = ""
        for user in users:
            assert isinstance(user.is_channel_creator, bool)
            assert isinstance(user.nick_name, str)
            if user.is_channel_creator:
                nicks += f"@{user.nick_name}"
            else:
                nicks += user.nick_name
            # use space as seperator
            if user != users[-1]:
                nicks += " "
        return nicks

    @staticmethod
    def get_channel_all_nicks(channel: ChatChannelCaches) -> list[str]:
        assert channel is not None
        assert isinstance(channel.channel_name, str)
        users = data.get_channel_user_caches_by_name(channel.channel_name)
        nicks = []
        for user in users:
            nicks.append(user.nick_name)
        return nicks
