from fastapi import APIRouter

from backends.library.abstractions.contracts import RESPONSES_DEF, DataResponse, OKResponse, Response
from backends.protocols.gamespy.natneg.handlers import ConnectHandler, InitHandler, ReportHandler
from backends.protocols.gamespy.natneg.requests import AddressCheckRequest, ConnectRequest,  ErtAckRequest, InitRequest, ReportRequest
from backends.protocols.gamespy.natneg.responses import ConnectResponse
from backends.urls import NATNEG


router = APIRouter()


@router.post(f"{NATNEG}/AddressCheckHandler", responses=RESPONSES_DEF)
def address_check(request: AddressCheckRequest) -> Response:
    raise NotImplementedError()


@router.post(f"{NATNEG}/ConnectHandler", responses=RESPONSES_DEF)
def connect(request: ConnectRequest) -> ConnectResponse:
    handler = ConnectHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{NATNEG}/ErtAckHandler", responses=RESPONSES_DEF)
def ert_ack(request: ErtAckRequest) -> Response:
    raise NotImplementedError()


@router.post(f"{NATNEG}/InitHandler", responses=RESPONSES_DEF)
def init(request: InitRequest) -> Response:
    handler = InitHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{NATNEG}/ReportHandler", responses=RESPONSES_DEF)
def report(request: ReportRequest) -> OKResponse:
    handler = ReportHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
