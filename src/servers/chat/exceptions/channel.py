from servers.chat.enums.irc_error_code import IRCErrorCode
from servers.chat.exceptions.general import IRCChannelException


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
