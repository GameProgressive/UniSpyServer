from servers.natneg.src.abstractions.contracts import (
    CommonResponseBase,
    RequestBase,
    ResultBase,
)


class InitResponse(CommonResponseBase):
    def __init__(self, request: RequestBase, result: ResultBase) -> None:
        super().__init__(request, result)
        assert issubclass(request, RequestBase)
        assert issubclass(result, ResultBase)
