import frontends.gamespy.protocols.web_services.abstractions.contracts as lib
from frontends.gamespy.protocols.web_services.aggregations.soap_envelop import (
    SoapEnvelop,
)
from frontends.gamespy.protocols.web_services.modules.direct2game.aggregation.enums import (
    CommandName,
)

NAMESPACE = "http://gamespy.net/commerce/"


class RequestBase(lib.RequestBase):
    command_name: CommandName
    pass


class ResultBase(lib.ResultBase):
    pass


class ResponseBase(lib.ResponseBase):
    _content: SoapEnvelop
