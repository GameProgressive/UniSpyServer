from fastapi import FastAPI

from backends.protocols.gamespy.query_report.requests import ChallengeRequest, ClientMessageRequest, EchoRequest, HeartBeatRequest
from backends.urls import QUERY_REPORT
from servers.presence_connection_manager.src.contracts.requests.general import KeepAliveRequest
from servers.query_report.src.v2.contracts.requests import AvaliableRequest

app = FastAPI()


@app.post(f"/{QUERY_REPORT}/HeartBeatHandler")
def heartbeat(request: HeartBeatRequest):
    raise NotImplementedError()


@app.post(f"/{QUERY_REPORT}/ChallengeHanler")
def challenge(request: ChallengeRequest):
    raise NotImplementedError()


@app.post(f"/{QUERY_REPORT}/AvailableHandler")
def available(request: AvaliableRequest):
    raise NotImplementedError()


@app.post(f"/{QUERY_REPORT}/ClientMessageAckHandler")
def client_message(request: ClientMessageRequest):
    raise NotImplementedError()


@app.post(f"/{QUERY_REPORT}/EchoHandler")
def echo(request: EchoRequest):
    raise NotImplementedError()


@app.post(f"/{QUERY_REPORT}/KeepAliveHandler")
def keep_alive(request: KeepAliveRequest):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
