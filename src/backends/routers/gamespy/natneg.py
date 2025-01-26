from fastapi import APIRouter

from backends.protocols.gamespy.chat.requests import PingRequest
from backends.protocols.gamespy.natneg.handlers import InitHandler
from backends.protocols.gamespy.natneg.requests import AddressCheckRequest, ConnectRequest, ErtAckRequest, InitRequest, ReportRequest
from backends.urls import NATNEG


router = APIRouter()


@router.post(f"{NATNEG}/AddressCheckHandler")
async def address_check(request: AddressCheckRequest) -> dict:
    raise NotImplementedError()


@router.post(f"{NATNEG}/ConnectHandler")
async def connect(request: ConnectRequest):
    raise NotImplementedError()


@router.post(f"{NATNEG}/ErtAckHandler")
async def ert_ack(request: ErtAckRequest):
    raise NotImplementedError()


@router.post(f"{NATNEG}/InitHandler")
async def init(request: InitRequest):
    handler = InitHandler(request)
    await handler.handle()
    return handler.response


@router.post(f"{NATNEG}/PingHandler")
async def ping(request: PingRequest):
    raise NotImplementedError()


@router.post(f"{NATNEG}/ReportHandler")
async def report(request: ReportRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
