from frontends.gamespy.library.abstractions.contracts import ResponseBase
from frontends.gamespy.protocols.chat.abstractions.contract import SERVER_DOMAIN
from frontends.gamespy.protocols.chat.aggregates.enums import IRCErrorCode

from frontends.gamespy.library.exceptions.general import UniSpyException as ER, UniSpyExceptionValidator, get_exceptions_dict

import abc


class ChatException(ER):
    def __init__(self, message: str):
        self.message = message


class IRCException(ChatException, ResponseBase):
    error_code: IRCErrorCode

    def __init__(self, message: str, error_code: IRCErrorCode) -> None:
        ChatException.__init__(self, message)
        self.error_code = error_code

    @abc.abstractmethod
    def build(self):
        pass


class IrcChannelExceptionValidator(UniSpyExceptionValidator):
    channel_name: str


class IrcChannelException(IRCException):
    channel_name: str
    _validator: IrcChannelExceptionValidator

    def __init__(self, message: str,
                 channel_name: str,
                 error_code: IRCErrorCode) -> None:
        super().__init__(message, error_code)
        self.channel_name = channel_name

    def build(self):
        self.sending_buffer = f":{SERVER_DOMAIN} {self.error_code} * {self.channel_name} :{self.message}\r\n"  # noqa: E501


class ErrOneUSNickNameException(IRCException):
    def __init__(self, message: str) -> None:
        super().__init__(message, IRCErrorCode.ERR_ONE_US_NICK_NAME)


class LoginFailedException(IRCException):
    def __init__(self, message: str,) -> None:
        super().__init__(message, IRCErrorCode.LOGIN_FAILED)


class MoreParametersException(IRCException):
    def __init__(self, message: str,) -> None:
        super().__init__(message, IRCErrorCode.MORE_PARAMETERS)


class NickNameInUseExceptionValidator(UniSpyExceptionValidator):
    old_nick: str
    new_nick: str


class NickNameInUseException(IRCException):
    old_nick: str
    new_nick: str
    _validator: NickNameInUseExceptionValidator

    def __init__(
        self,
        old_nick: str,
        new_nick: str,
        message: str,
    ) -> None:
        super().__init__(message, IRCErrorCode.NICK_NAME_IN_USE)
        self.old_nick = old_nick
        self.new_nick = new_nick

    def build(self):
        self.sending_buffer = f"{SERVER_DOMAIN} {self.error_code} {self.old_nick} {self.new_nick} *\r\n"  # noqa: E501


class NoSuchNickException(IRCException):
    def __init__(self, message: str) -> None:
        super().__init__(message, IRCErrorCode.NO_SUCH_NICK)


class NoUniqueNickException(IRCException):
    def __init__(self, message: str) -> None:
        super().__init__(message, IRCErrorCode.NO_UNIQUE_NICK)


class RegisterNickFaildException(IRCException):
    def __init__(
        self, message: str
    ) -> None:
        super().__init__(message, IRCErrorCode.REGISTER_NICK_FAILED)


class TooManyChannelsException(IRCException):
    def __init__(
        self, message: str
    ) -> None:
        super().__init__(message, IRCErrorCode.TOO_MANY_CHANNELS)


class UniqueNickExpiredException(IRCException):
    def __init__(
        self, message: str
    ) -> None:
        super().__init__(message, IRCErrorCode.UNIQUE_NICK_EXPIRED)


# region Channel Exceptions


class BadChannelMaskException(IrcChannelException):
    def __init__(
        self,
        message: str,
        channel_name: str
    ) -> None:
        super().__init__(message, channel_name, IRCErrorCode.BAD_CHAN_MASK)


class BadChannelKeyException(IrcChannelException):
    def __init__(
        self, message: str,
        channel_name: str
    ) -> None:
        super().__init__(message, channel_name, IRCErrorCode.BAD_CHANNEL_KEY)


class BannedFromChanException(IrcChannelException):
    def __init__(
        self, message: str,
        channel_name: str
    ) -> None:
        super().__init__(message, channel_name, IRCErrorCode.BANNED_FROM_CHAN)


class ChannelIsFullException(IrcChannelException):
    def __init__(
        self, message: str,
        channel_name: str
    ) -> None:
        super().__init__(message, channel_name, IRCErrorCode.CHANNEL_IS_FULL)


class InviteOnlyChanException(IrcChannelException):
    def __init__(
        self, message: str,
        channel_name: str
    ) -> None:
        super().__init__(message, channel_name, IRCErrorCode.INVITE_ONLY_CHAN)


class NoSuchChannelException(IrcChannelException):
    def __init__(
        self, message: str,
        channel_name: str
    ) -> None:
        super().__init__(message, channel_name, IRCErrorCode.NO_SUCH_CHANNEL)


EXCEPTIONS = get_exceptions_dict(__name__)
