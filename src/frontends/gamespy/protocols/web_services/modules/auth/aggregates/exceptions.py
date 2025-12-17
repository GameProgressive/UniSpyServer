import abc
from frontends.gamespy.library.exceptions.general import UniSpyExceptionValidator, get_exceptions_dict
from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
from frontends.gamespy.protocols.web_services.modules.auth.aggregates.enums import AuthCode, ResponseName


class AuthExceptionValidator(UniSpyExceptionValidator):
    response_code: AuthCode
    command_name: ResponseName


class AuthException(WebException):
    response_code: AuthCode
    command_name: ResponseName
    _validator: AuthExceptionValidator

    def __init__(self, message: str, response_code: AuthCode, command_name: ResponseName) -> None:
        super().__init__(message)
        self.response_code = response_code
        self.command_name = command_name

    @abc.abstractmethod
    def build(self) -> None:
        xml = SoapEnvelop(self.command_name.value)
        xml.add("responseCode", self.response_code.value)
        self.sending_buffer = str(xml)


class LoginServerInitFailedException(AuthException):
    def __init__(self, message: str, command_name: ResponseName) -> None:
        super().__init__(message, AuthCode.SERVER_INIT_FAILED, command_name)


class UserNotFoundException(AuthException):
    def __init__(self, message: str, command_name: ResponseName) -> None:
        super().__init__(message, AuthCode.USER_NOT_FOUND, command_name)


class InvalidPasswordException(AuthException):
    def __init__(self, message: str, command_name: ResponseName) -> None:
        super().__init__(message, AuthCode.INVALID_PASSWORD, command_name)


class InvalidProfileException(AuthException):
    def __init__(self, message: str, command_name: ResponseName) -> None:
        super().__init__(message, AuthCode.INVALID_PROFILE, command_name)


class ParseException(AuthException):
    def __init__(self, message: str, command_name: ResponseName) -> None:
        super().__init__(message, AuthCode.PARSE_ERROR, command_name)


class InvalidGameId(AuthException):
    def __init__(self, message: str, command_name: ResponseName) -> None:
        super().__init__(message, AuthCode.INVALID_GAMEID, command_name)


class InvalidAccessKey(AuthException):
    def __init__(self, message: str, command_name: ResponseName) -> None:
        super().__init__(message, AuthCode.INVALID_GAMEID, command_name)


EXCEPTIONS = get_exceptions_dict(__name__)
