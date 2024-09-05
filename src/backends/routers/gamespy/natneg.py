from fastapi import FastAPI

from backends.protocols.gamespy.chat.requests import PingRequest
from backends.protocols.gamespy.natneg.requests import ConnectRequest, ErtAckRequest, InitRequest, ReportRequest
from backends.urls import NATNEG
from servers.natneg.src.contracts.requests import AddressCheckRequest


app = FastAPI()


@app.post(f"{NATNEG}/AddressCheckHandler/")
async def address_check(request: AddressCheckRequest):
    raise NotImplementedError()


@app.post(f"{NATNEG}/ConnectHandler/")
async def connect(request: ConnectRequest):
    raise NotImplementedError()


@app.post(f"{NATNEG}/ErtAckHandler/")
async def ert_ack(request: ErtAckRequest):
    raise NotImplementedError()


@app.post(f"{NATNEG}/InitHandler/")
async def init(request: InitRequest):
    raise NotImplementedError()


@app.post(f"{NATNEG}/PingHandler/")
async def ping(request: PingRequest):
    raise NotImplementedError()


@app.post(f"{NATNEG}/ReportHandler/")
async def report(request: ReportRequest):
    raise NotImplementedError()



if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)