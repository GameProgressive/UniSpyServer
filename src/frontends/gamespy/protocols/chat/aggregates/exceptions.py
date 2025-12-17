from frontends.gamespy.protocols.chat.abstractions.contract import SERVER_DOMAIN
from frontends.gamespy.protocols.chat.aggregates.enums import IRCErrorCode

from frontends.gamespy.library.exceptions.general import UniSpyException as ER


class ChatException(ER):
    pass


class IRCException(ChatException):
    error_code: "IRCErrorCode"
    sending_buffer: "str"

    def __init__(self, message: "str", error_code: "IRCErrorCode") -> None:
        super().__init__(message)
        assert isinstance(error_code, IRCErrorCode)
        self.error_code = error_code

    def build(self):
        raise ChatException(
            "IRCException is abstracted class, should not be initialized")


class IRCChannelException(IRCException):
    channel_name: str

    def __init__(self, message: str, error_code: IRCErrorCode) -> None:
        super().__init__(message, error_code)

    def build(self):
        self.sending_buffer = f":{SERVER_DOMAIN} {self.error_code} * {self.channel_name} :{self.message}\r\n"  # noqa: E501


class ErrOneUSNickNameException(IRCException):
    def __init__(
        self,
        message: "str",
        error_code: "IRCErrorCode" = IRCErrorCode.ERR_ONE_US_NICK_NAME,
    ) -> None:
        super().__init__(message, error_code)


class LoginFailedException(IRCException):
    def __init__(
        self,
        message: "str",
        error_code: "IRCErrorCode" = IRCErrorCode.LOGIN_FAILED,
    ) -> None:
        super().__init__(message, error_code)


class MoreParametersException(IRCException):
    def __init__(
        self,
        message: "str",
        error_code: "IRCErrorCode" = IRCErrorCode.MORE_PARAMETERS,
    ) -> None:
        super().__init__(message, error_code)


class NickNameInUseException(IRCException):
    old_nick: str
    new_nick: str

    def __init__(
        self,
        old_nick: str,
        new_nick: str,
        message: str,
        error_code: IRCErrorCode = IRCErrorCode.NICK_NAME_IN_USE,
    ) -> None:
        super().__init__(message, error_code)
        self.old_nick = old_nick
        self.new_nick = new_nick

    def build(self):
        self.sending_buffer = f"{SERVER_DOMAIN} {self.error_code} {self.old_nick} {self.new_nick} *\r\n"  # noqa: E501


class NoSuchNickException(IRCException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.NO_SUCH_NICK
    ) -> None:
        super().__init__(message, error_code)


class NoUniqueNickException(IRCException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.NO_UNIQUE_NICK
    ) -> None:
        super().__init__(message, error_code)


class RegisterNickFaildException(IRCException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.REGISTER_NICK_FAILED
    ) -> None:
        super().__init__(message, error_code)


class TooManyChannelsException(IRCException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.TOO_MANY_CHANNELS
    ) -> None:
        super().__init__(message, error_code)


class UniqueNickExpiredException(IRCException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.UNIQUE_NICK_EXPIRED
    ) -> None:
        super().__init__(message, error_code)


# region Channel Exceptions


class BadChannelMaskException(IRCChannelException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.BAD_CHAN_MASK
    ) -> None:
        super().__init__(message, error_code)


class BadChannelKeyException(IRCChannelException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.BAD_CHANNEL_KEY
    ) -> None:
        super().__init__(message, error_code)


class BannedFromChanException(IRCChannelException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.BANNED_FROM_CHAN
    ) -> None:
        super().__init__(message, error_code)


class ChannelIsFullException(IRCChannelException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.CHANNEL_IS_FULL
    ) -> None:
        super().__init__(message, error_code)


class InviteOnlyChanException(IRCChannelException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.INVITE_ONLY_CHAN
    ) -> None:
        super().__init__(message, error_code)


class NoSuchChannelException(IRCChannelException):
    def __init__(
        self, message: str, error_code: IRCErrorCode = IRCErrorCode.NO_SUCH_CHANNEL
    ) -> None:
        super().__init__(message, error_code)
