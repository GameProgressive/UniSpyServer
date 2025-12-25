from typing import TYPE_CHECKING
from frontends.gamespy.library.exceptions.general import UniSpyExceptionValidator, get_exceptions_dict
from frontends.gamespy.library.network.http_handler import HttpData
from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import SoapEnvelop
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import CommandName, SakeCode
if TYPE_CHECKING:
    from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import ResultBase


class SakeExceptionValidator(UniSpyExceptionValidator):
    command_name: CommandName


class SakeException(WebException):
    """
    sake exception will send error in http header
    """
    command_name: CommandName
    _validator: SakeExceptionValidator

    def __init__(self, message: str, command_name: CommandName) -> None:
        super().__init__(message)
        self.command_name = command_name

    def build(self) -> None:
        content = SoapEnvelop(f"{self.command_name.value}Response")
        content.add(f"{self.command_name.value}Result",
                    SakeCode.DATABASE_UNAVAILABLE.value)
        self.sending_buffer = HttpData(
            body=str(content), headers={
                "Error": self.message
            }
        )


EXCEPTIONS = get_exceptions_dict(__name__)
