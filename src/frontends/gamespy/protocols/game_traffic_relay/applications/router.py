from fastapi import FastAPI
from backends.urls import *
from frontends.gamespy.protocols.game_traffic_relay.contracts.general import InitPacketInfo

app = FastAPI()


@app.post(f"/GetNatNegotiationInfo")
async def get_natneg_info(request: InitPacketInfo):
    data = request.json
