from frontends.gamespy.library.abstractions.contracts import ResponseBase
from frontends.gamespy.library.exceptions.general import UniSpyException, get_exceptions_dict
from frontends.gamespy.protocols.presence_search_player.aggregates.enums import GPErrorCode


class GPException(UniSpyException, ResponseBase):
    error_code: GPErrorCode
    sending_buffer: str

    def __init__(
        self,
        message: str = "General error.",
        error_code: GPErrorCode = GPErrorCode.GENERAL,
    ) -> None:
        UniSpyException.__init__(self, message)
        self.error_code = error_code

    def build(self) -> None:
        self.sending_buffer = f"\\error\\\\err\\{int(self.error_code)}\\fatal\\\\errmsg\\{self.message}\\final\\"


class GPParseException(GPException):
    def __init__(
        self,
        message: str = "Request parsing error.",
    ) -> None:
        super().__init__(message, GPErrorCode.PARSE)


class GPUdpLayerException(GPException):
    def __init__(
        self,
        message: str = "Unknown UDP layer error.",
    ) -> None:
        super().__init__(message, GPErrorCode.UDP_LAYER)


class GPNotLoginException(GPException):
    def __init__(
        self,
        message: str = "You are not logged in, please login first.",
    ) -> None:
        super().__init__(message, GPErrorCode.NOT_LOGGED_IN)


class GPNetworkException(GPException):
    def __init__(
        self,
        message: str = "Unknown network error.",
    ) -> None:
        super().__init__(message, GPErrorCode.NETWORK)


class GPForceDisconnectException(GPException):
    def __init__(
        self,
        message: str = "Client is forced to disconnect.",
    ) -> None:
        super().__init__(message, GPErrorCode.FORCED_DISCONNECT)


class GPDatabaseException(GPException):
    def __init__(
        self,
        message: str = "Database error.",
    ) -> None:
        super().__init__(message, GPErrorCode.DATABASE_ERROR)


class GPConnectionCloseException(GPException):
    def __init__(
        self,
        message: str = "Client connection accidently closed.",
    ) -> None:
        super().__init__(message,  GPErrorCode.CONNECTION_CLOSE)


class GPBadSessionKeyException(GPException):
    def __init__(
        self,
        message: str = "Session key is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.BAD_SESSION_KEY)


class GPAddBuddyException(GPException):
    def __init__(
        self,
        message: str = "Unknown error occur at add buddy.",
        error_code: GPErrorCode = GPErrorCode.ADD_BUDDY
    ) -> None:
        super().__init__(message, error_code)


class GPAddBuddyAlreadyException(GPAddBuddyException):
    def __init__(
        self,
        message: str = "The buddy you are adding is already in your buddy list.",
    ) -> None:
        super().__init__(message, GPErrorCode.ADD_BUDDY_ALREADY_BUDDY)


class GPAddBuddyBadFormatException(GPAddBuddyException):
    def __init__(
        self,
        message: str = "Add buddy format invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.ADD_BUDDY_BAD_FORM)


class GPAddBuddyBadNewException(GPAddBuddyException):
    def __init__(
        self,
        message: str = "The buddy name provided is invalid.",
    ) -> None:
        super().__init__(message,  GPErrorCode.ADD_BUDDY_BAD_NEW)


class AuthAddException(GPException):
    def __init__(
        self,
        message: str = "The adding of authentication failed.",
        error_code: GPErrorCode = GPErrorCode.AUTH_ADD,
    ) -> None:
        super().__init__(message, error_code)


class AuthAddBadFormatException(AuthAddException):
    def __init__(
        self,
        message: str = "The authentication is in bad form.",
    ) -> None:
        super().__init__(message, GPErrorCode.AUTH_ADD_BAD_FORM)


class AuthAddBadSigException(AuthAddException):
    def __init__(
        self,
        message: str = "The signature in authentication is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.AUTH_ADD_BAD_SIG)


class GPBuddyMsgException(GPException):
    def __init__(
        self,
        message: str = "Unknown error occur when processing buddy message.",
        error_code: GPErrorCode = GPErrorCode.BM,
    ) -> None:
        super().__init__(message, error_code)


class GPBuddyMsgExtInfoNotSupportedException(GPBuddyMsgException):
    def __init__(
        self,
        message: str = "Buddy message is not supported.",
    ) -> None:
        super().__init__(message, GPErrorCode.BM_EXT_INFO_NOT_SUPPORTED)


class GPBuddyMsgNotBuddyException(GPBuddyMsgException):
    def __init__(
        self,
        message: str = "The message receiver is not your buddy.",
    ) -> None:
        super().__init__(message, GPErrorCode.BM_NOT_BUDDY)


class CheckException(GPException):
    def __init__(
        self,
        message: str = "There was an error checking the user account.",
    ) -> None:
        super().__init__(message, GPErrorCode.CHECK)

    def build(self) -> None:
        self.sending_buffer = f"\\cur\\{int(self.error_code)}\\final\\"


class GPLoginException(GPException):
    def __init__(
        self,
        message: str = "Unknown login error.",
        error_code: GPErrorCode = GPErrorCode.LOGIN,
    ) -> None:
        super().__init__(message, error_code)


class GPLoginBadEmailException(GPLoginException):
    def __init__(
        self,
        message: str = "Email provided is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_BAD_EMAIL)


class GPLoginBadLoginTicketException(GPLoginException):
    def __init__(
        self,
        message: str = "The login ticket is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_TICKET_EXPIRED)


class GPLoginBadNickException(GPLoginException):
    def __init__(
        self,
        message: str = "Nickname is in valid.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_BAD_NICK)


class GPLoginBadPasswordException(GPLoginException):
    def __init__(
        self,
        message: str = "Password provided is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_BAD_PASSWORD)


class GPLoginBadPreAuthException(GPLoginException):
    def __init__(
        self,
        message: str = "Login pre-authentication failed.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_BAD_PRE_AUTH)


class GPLoginBadProfileException(GPLoginException):
    def __init__(
        self,
        message: str = "User profile is damaged.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_BAD_PROFILE)


class GPLoginBadUniquenickException(GPLoginException):
    def __init__(
        self,
        message: str = "The uniquenick provided is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_BAD_UNIQUENICK)


class GPLoginConnectionFailedException(GPLoginException):
    def __init__(
        self,
        message: str = "Login connection failed.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_CONNECTION_FAILED)


class GPLoginProfileDeletedException(GPLoginException):
    def __init__(
        self,
        message: str = "Login connection failed.",
    ) -> None:
        super().__init__(message,  GPErrorCode.LOGIN_PROFILE_DELETED)


class GPLoginServerAuthFailedException(GPLoginException):
    def __init__(
        self,
        message: str = "Login server authentication failed.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_SERVER_AUTH_FAILED)


class GPLoginTicketExpiredException(GPLoginException):
    def __init__(
        self,
        message: str = "The login ticket have expired.",
    ) -> None:
        super().__init__(message, GPErrorCode.LOGIN_TICKET_EXPIRED)


class GPLoginTimeOutException(GPLoginException):
    def __init__(
        self,
        message: str = "Login timeout.",
    ) -> None:
        super().__init__(message,  GPErrorCode.LOGIN_TIME_OUT)


class GPNewProfileException(GPException):
    def __init__(
        self,
        message: str = "An unknown error occur when creating new profile.",
        error_code: GPErrorCode = GPErrorCode.NEW_PROFILE,
    ) -> None:
        super().__init__(message, error_code)


class GPNewProfileBadNickException(GPNewProfileException):
    def __init__(
        self,
        message: str = "Nickname is invalid at creating new profile.",
    ) -> None:
        super().__init__(message, GPErrorCode.NEW_PROFILE_BAD_NICK)


class GPNewProfileBadOldNickException(GPNewProfileException):
    def __init__(
        self,
        message: str = "There is an already exist nickname.",
    ) -> None:
        super().__init__(message, GPErrorCode.NEW_PROFILE_BAD_OLD_NICK)


class GPNewUserException(GPException):
    def __init__(
        self,
        message: str = "There was an unknown error creating user account.",
    ) -> None:
        super().__init__(message, GPErrorCode.NEW_USER)


class GPNewUserBadNickException(GPException):
    def __init__(
        self,
        message: str = "The nickname provided is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.NEW_USER_BAD_NICK)


class GPNewUserBadPasswordException(GPException):
    def __init__(
        self,
        message: str = "Password is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.NEW_USER_BAD_PASSWORDS)


class GPNewUserUniquenickInUseException(GPException):
    def __init__(
        self,
        message: str = "Uniquenick is in use.",
        error_code: GPErrorCode = GPErrorCode.NEW_USER_UNIQUENICK_IN_USE,
    ) -> None:
        super().__init__(message, GPErrorCode.NEW_USER_UNIQUENICK_IN_USE)


class GPNewUserUniquenickInvalidException(GPException):
    def __init__(
        self,
        message: str = "Uniquenick is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.NEW_USER_UNIQUENICK_INVALID)


class GPStatusException(GPException):
    def __init__(
        self,
        message: str = "Unknown error happen when processing player status.",
    ) -> None:
        super().__init__(message, GPErrorCode.STATUS)


class GPUpdateProfileException(GPException):
    def __init__(
        self,
        message: str = "Update profile unknown error.",
        error_code: GPErrorCode = GPErrorCode.UPDATE_PRO,
    ) -> None:
        super().__init__(message, error_code)


class GPUpdateProBadNickException(GPUpdateProfileException):
    def __init__(
        self,
        message: str = "Nickname is invalid for updating profile.",
    ) -> None:
        super().__init__(message, GPErrorCode.UPDATE_PRO_BAD_NICK)


class GPUpdateUIException(GPException):
    def __init__(
        self,
        message: str = "Update user info unknown error.",
    ) -> None:
        super().__init__(message, GPErrorCode.UPDATE_UI)


class GPUpdateUIBadEmailException(GPException):
    def __init__(
        self,
        message: str = "Email is invalid.",
    ) -> None:
        super().__init__(message, GPErrorCode.UPDATE_UI_BAD_EMAIL)


EXCEPTIONS = get_exceptions_dict(__name__)
