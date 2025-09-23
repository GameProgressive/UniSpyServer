from fastapi import APIRouter

from backends.protocols.gamespy.chat.requests import PingRequest
from backends.protocols.gamespy.natneg.handlers import ConnectHandler, InitHandler, ReportHandler
from backends.protocols.gamespy.natneg.requests import AddressCheckRequest, ConnectRequest, ErtAckRequest, InitRequest, ReportRequest
from backends.urls import NATNEG


router = APIRouter()


@router.post(f"{NATNEG}/AddressCheckHandler")
def address_check(request: AddressCheckRequest):
    raise NotImplementedError()


@router.post(f"{NATNEG}/ConnectHandler")
def connect(request: ConnectRequest):
    handler = ConnectHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{NATNEG}/ErtAckHandler")
def ert_ack(request: ErtAckRequest):
    raise NotImplementedError()


@router.post(f"{NATNEG}/InitHandler")
def init(request: InitRequest):
    handler = InitHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{NATNEG}/ReportHandler")
def report(request: ReportRequest):
    handler = ReportHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
