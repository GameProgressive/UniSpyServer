from library.src.exceptions.general import UniSpyException
from servers.presence_search_player.src.aggregates.enums import GPErrorCode
from library.src.abstractions.contracts import ResponseBase


class GPException(UniSpyException, ResponseBase):
    error_code: GPErrorCode
    sending_buffer: str

    def __init__(
        self,
        message: str = "General error.",
        error_code=GPErrorCode.GENERAL,
    ) -> None:
        UniSpyException.__init__(self, message)
        self.error_code = error_code

    def build(self) -> None:
        self.sending_buffer = f"\\error\\\\err\\{int(self.error_code)}\\fatal\\\\errmsg\\{
            self.message}\\final\\"


class GPParseException(GPException):
    def __init__(
        self,
        message: str = "Request parsing error.",
        error_code=GPErrorCode.PARSE,
    ) -> None:
        super().__init__(message, error_code)


class GPUdpLayerException(GPException):
    def __init__(
        self,
        message: str = "Unknown UDP layer error.",
        error_code=GPErrorCode.UDP_LAYER,
    ) -> None:
        super().__init__(message, error_code)


class GPNotLoginException(GPException):
    def __init__(
        self,
        message: str = "You are not logged in, please login first.",
        error_code=GPErrorCode.NOT_LOGGED_IN,
    ) -> None:
        super().__init__(message, error_code)


class GPNetworkException(GPException):
    def __init__(
        self,
        message: str = "Unknown network error.",
        error_code=GPErrorCode.NETWORK,
    ) -> None:
        super().__init__(message, error_code)


class GPForceDisconnectException(GPException):
    def __init__(
        self,
        message: str = "Client is forced to disconnect.",
        error_code=GPErrorCode.FORCED_DISCONNECT,
    ) -> None:
        super().__init__(message, error_code)


class GPDatabaseException(GPException):
    def __init__(
        self,
        message: str = "Database error.",
        error_code=GPErrorCode.DATABASE_ERROR,
    ) -> None:
        super().__init__(message, error_code)


class GPConnectionCloseException(GPException):
    def __init__(
        self,
        message: str = "Client connection accidently closed.",
        error_code=GPErrorCode.CONNECTION_CLOSE,
    ) -> None:
        super().__init__(message, error_code)


class GPBadSessionKeyException(GPException):
    def __init__(
        self,
        message: str = "Session key is invalid.",
        error_code=GPErrorCode.BAD_SESSION_KEY,
    ) -> None:
        super().__init__(message, error_code)


class GPAddBuddyException(GPException):
    def __init__(
        self,
        message: str = "Unknown error occur at add buddy.",
        error_code=GPErrorCode.ADD_BUDDY,
    ) -> None:
        super().__init__(message, error_code)


class GPAddBuddyAlreadyException(GPAddBuddyException):
    def __init__(
        self,
        message: str = "The buddy you are adding is already in your buddy list.",
        error_code=GPErrorCode.ADD_BUDDY_ALREADY_BUDDY,
    ) -> None:
        super().__init__(message, error_code)


class GPAddBuddyBadFormatException(GPAddBuddyException):
    def __init__(
        self,
        message: str = "Add buddy format invalid.",
        error_code=GPErrorCode.ADD_BUDDY_BAD_FORM,
    ) -> None:
        super().__init__(message, error_code)


class GPAddBuddyBadNewException(GPAddBuddyException):
    def __init__(
        self,
        message: str = "The buddy name provided is invalid.",
        error_code=GPErrorCode.ADD_BUDDY_BAD_NEW,
    ) -> None:
        super().__init__(message, error_code)


class AuthAddException(GPException):
    def __init__(
        self,
        message: str = "The adding of authentication failed.",
        error_code=GPErrorCode.AUTH_ADD,
    ) -> None:
        super().__init__(message, error_code)


class AuthAddBadFormatException(AuthAddException):
    def __init__(
        self,
        message: str = "The authentication is in bad form.",
        error_code=GPErrorCode.AUTH_ADD_BAD_FORM,
    ) -> None:
        super().__init__(message, error_code)


class AuthAddBadSigException(AuthAddException):
    def __init__(
        self,
        message: str = "The signature in authentication is invalid.",
        error_code=GPErrorCode.AUTH_ADD_BAD_SIG,
    ) -> None:
        super().__init__(message, error_code)


class GPBuddyMsgException(GPException):
    def __init__(
        self,
        message: str = "Unknown error occur when processing buddy message.",
        error_code=GPErrorCode.BM,
    ) -> None:
        super().__init__(message, error_code)


class GPBuddyMsgExtInfoNotSupportedException(GPBuddyMsgException):
    def __init__(
        self,
        message: str = "Buddy message is not supported.",
        error_code=GPErrorCode.BM_EXT_INFO_NOT_SUPPORTED,
    ) -> None:
        super().__init__(message, error_code)


class GPBuddyMsgNotBuddyException(GPBuddyMsgException):
    def __init__(
        self,
        message: str = "The message receiver is not your buddy.",
        error_code=GPErrorCode.BM_NOT_BUDDY,
    ) -> None:
        super().__init__(message, error_code)


class CheckException(GPException):
    def __init__(
        self,
        message: str = "There was an error checking the user account.",
        error_code=GPErrorCode.CHECK,
    ) -> None:
        super().__init__(message, error_code)

    def build(self) -> None:
        self.sending_buffer = f"\\cur\\{int(self.error_code)}\\final\\"


class GPLoginException(GPException):
    def __init__(
        self,
        message: str = "Unknown login error.",
        error_code=GPErrorCode.LOGIN,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginBadEmailException(GPLoginException):
    def __init__(
        self,
        message: str = "Email provided is invalid.",
        error_code=GPErrorCode.LOGIN_BAD_EMAIL,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginBadLoginTicketException(GPLoginException):
    def __init__(
        self,
        message: str = "The login ticket is invalid.",
        error_code=GPErrorCode.LOGIN_TICKET_EXPIRED,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginBadNickException(GPLoginException):
    def __init__(
        self,
        message: str = "Nickname is in valid.",
        error_code=GPErrorCode.LOGIN_BAD_NICK,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginBadPasswordException(GPLoginException):
    def __init__(
        self,
        message: str = "Password provided is invalid.",
        error_code=GPErrorCode.LOGIN_BAD_PASSWORD,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginBadPreAuthException(GPLoginException):
    def __init__(
        self,
        message: str = "Login pre-authentication failed.",
        error_code=GPErrorCode.LOGIN_BAD_PRE_AUTH,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginBadProfileException(GPLoginException):
    def __init__(
        self,
        message: str = "User profile is damaged.",
        error_code=GPErrorCode.LOGIN_BAD_PROFILE,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginBadUniquenickException(GPLoginException):
    def __init__(
        self,
        message: str = "The uniquenick provided is invalid.",
        error_code=GPErrorCode.LOGIN_BAD_UNIQUENICK,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginConnectionFailedException(GPLoginException):
    def __init__(
        self,
        message: str = "Login connection failed.",
        error_code=GPErrorCode.LOGIN_CONNECTION_FAILED,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginProfileDeletedException(GPLoginException):
    def __init__(
        self,
        message: str = "Login connection failed.",
        error_code=GPErrorCode.LOGIN_PROFILE_DELETED,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginServerAuthFailedException(GPLoginException):
    def __init__(
        self,
        message: str = "Login server authentication failed.",
        error_code=GPErrorCode.LOGIN_SERVER_AUTH_FAILED,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginTicketExpiredException(GPLoginException):
    def __init__(
        self,
        message: str = "The login ticket have expired.",
        error_code=GPErrorCode.LOGIN_TICKET_EXPIRED,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginTimeOutException(GPLoginException):
    def __init__(
        self,
        message: str = "Login timeout.",
        error_code=GPErrorCode.LOGIN_TIME_OUT,
    ) -> None:
        super().__init__(message, error_code)


class GPNewProfileException(GPException):
    def __init__(
        self,
        message: str = "An unknown error occur when creating new profile.",
        error_code=GPErrorCode.NEW_PROFILE,
    ) -> None:
        super().__init__(message, error_code)


class GPNewProfileBadNickException(GPNewProfileException):
    def __init__(
        self,
        message: str = "Nickname is invalid at creating new profile.",
        error_code=GPErrorCode.NEW_PROFILE_BAD_NICK,
    ) -> None:
        super().__init__(message, error_code)


class GPNewProfileBadOldNickException(GPNewProfileException):
    def __init__(
        self,
        message: str = "There is an already exist nickname.",
        error_code=GPErrorCode.NEW_PROFILE_BAD_OLD_NICK,
    ) -> None:
        super().__init__(message, error_code)


class GPNewUserException(GPException):
    def __init__(
        self,
        message: str = "There was an unknown error creating user account.",
        error_code=GPErrorCode.NEW_USER,
    ) -> None:
        super().__init__(message, error_code)


class GPNewUserBadNickException(GPException):
    def __init__(
        self,
        message: str = "The nickname provided is invalid.",
        error_code=GPErrorCode.NEW_USER_BAD_NICK,
    ) -> None:
        super().__init__(message, error_code)


class GPNewUserBadPasswordException(GPException):
    def __init__(
        self,
        message: str = "Password is invalid.",
        error_code=GPErrorCode.NEW_USER_BAD_PASSWORDS,
    ) -> None:
        super().__init__(message, error_code)


class GPNewUserUniquenickInUseException(GPException):
    def __init__(
        self,
        message: str = "Uniquenick is in use.",
        error_code=GPErrorCode.NEW_USER_UNIQUENICK_IN_USE,
    ) -> None:
        super().__init__(message, error_code)


class GPNewUserUniquenickInvalidException(GPException):
    def __init__(
        self,
        message: str = "Uniquenick is invalid.",
        error_code=GPErrorCode.NEW_USER_UNIQUENICK_INVALID,
    ) -> None:
        super().__init__(message, error_code)


class GPStatusException(GPException):
    def __init__(
        self,
        message: str = "Unknown error happen when processing player status.",
        error_code=GPErrorCode.STATUS,
    ) -> None:
        super().__init__(message, error_code)


class GPUpdateProfileException(GPException):
    def __init__(
        self,
        message: str = "Update profile unknown error.",
        error_code=GPErrorCode.UPDATE_PRO,
    ) -> None:
        super().__init__(message, error_code)


class GPUpdateProBadNickException(GPUpdateProfileException):
    def __init__(
        self,
        message: str = "Nickname is invalid for updating profile.",
        error_code=GPErrorCode.UPDATE_PRO_BAD_NICK,
    ) -> None:
        super().__init__(message, error_code)


class GPUpdateUIException(GPException):
    def __init__(
        self,
        message: str = "Update user info unknown error.",
        error_code=GPErrorCode.UPDATE_UI,
    ) -> None:
        super().__init__(message, error_code)


class GPUpdateUIBadEmailException(GPException):
    def __init__(
        self,
        message: str = "Email is invalid.",
        error_code=GPErrorCode.UPDATE_UI_BAD_EMAIL,
    ) -> None:
        super().__init__(message, error_code)


MAPPING = {
    "GPException": GPException,

}
